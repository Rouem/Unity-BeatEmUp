using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody rb;
    EnemyAnimations ea;
    public Attributes attributes;
    public GameObject player;//alvo do inimigo

    public int minDist,//distancia minima para ficar do player antes de começar a atacar
     timeAtk, //tempo minimo de espera para começar a perseguir o player
     combo, 
     maxHits;
    public string damageAnimation = "lightHit";//animação de dano que o jogado vai executar
    public float atkDist;//distancia minima para ele atacar o player
    public bool useImpulse;//faz ele deslizar durante o ataque

    float tp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ea = GetComponentInChildren<EnemyAnimations>();
        ea.SetController(this);
        attributes = GetComponentInChildren<Attributes>();
    }

    // Update is called once per frame
    void Update()
    {
        //começa verificado se o persoagem esta vivo, se não estiver, o if é executado 
        if(attributes.hp <= 0){
            ea.animator.SetBool("dead",true);
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject,4);
            return;
        }

        if(ea.stunned)return;//verifica se o personagem esta tonto

        //verifica se ja passou tempo minimo para começar perseguir o player.
        //Também verifica se o player esta vivo e não esta tonto.
        if(
            AttackTime(tp) && 
            player.GetComponent<Attributes>().hp > 0 && 
            !player.GetComponent<PlayerController>().IsStunned()
        )
        //verifica se possui a distancia minima para atacar e se já não está atacando
            if(DistanceOf(player.transform,atkDist) && !ea.isAttacking)
                AttackPlayer("lightAttack");//ataca o alvo
            else
                FollowTarget(player.transform);// perseguir o alvo
        else
            if(!ea.isAttacking)
            KeepDistance(player.transform);//se não estiver executando a animaçao de ataque, o personagem foge

        tp += Time.deltaTime;

    }
    /// <summary>
    /// Verifica se este personagem está a uma distancia minima do alvo.
    /// Retorna verdadeiro ou falso.
    /// </summary>
    /// <param name="target">Alvo para verificar</param>
    /// <param name="dist">Distancia minima para verificar</param>
    bool DistanceOf(Transform target,float dist){
        return Vector3.Distance(transform.position, target.position) < dist;
    }

    /// <summary>
    /// Verifica se já passou o tempo minimo para começar o ataque.
    /// Retorna verdadeiro ou falso.
    /// </summary>
    /// <param name="t">Tempo mínimo</param>
    bool AttackTime(float t){
        return t >= timeAtk;
    }

    /// <summary>
    /// Faz o personagem fugir do alvo.
    /// </summary>
    /// <param name="target">Alvo</param>
    void KeepDistance(Transform target){
        if(!DistanceOf(player.transform,minDist)){
            ea.Play(0);
        }else{
            ea.Play(1);
            transform.LookAt(target);
            rb.velocity = transform.forward * -attributes.spd*0.85f +
            transform.right * Random.Range(-1f,1f) * attributes.spd*1.2f;
            FixLook();
        }
    }

    /// <summary>
    /// Faz o personagem seguir o alvo.
    /// </summary>
    /// <param name="target">Alvo</param>
    void FollowTarget(Transform target){
        
        if(ea.isAttacking) return;

        transform.LookAt(target);
        rb.velocity = transform.forward * attributes.spd +
        transform.right * Random.Range(-1f,1f) * attributes.spd/2;
        FixLook();
        ea.Play(1);
    }

    /// <summary>
    /// Corrigi a rotação do personagem em relação ao alvo
    /// </summary>
    public void FixLook(){
        if(player.transform.position.x >= transform.position.x){
            transform.rotation = Quaternion.Euler(0,90,0);
        }else{
            transform.rotation = Quaternion.Euler(0,-90,0);
        }
    }

    /// <summary>
    /// Faz o personagem atacar o alvo.
    /// </summary>
    /// <param name="atkType">Nome da animação de ataque</param>
    void AttackPlayer(string atkType){
        ea.Play(atkType);
        combo++;
        if(useImpulse)
            rb.velocity = transform.forward * 1.5f;
        if(combo > maxHits){
            combo = 0;
            tp = 0;
            return;
        }
    }

    public void TakeDamage(string hitType, int damage)
    {
        if(ea.stunned && attributes.invencibleOnStun || ea.stunned && hitType.Equals("fly")) return;
        Debug.Log(transform.name+" take "+damage+" of damage!");
        if(attributes.IsStunned()){
            ea.Play(hitType);
            FixLook();
            if(hitType.Equals("fly"))
                    ea.StunOn(true);
                else
                    ea.StunOn(attributes.invencibleOnStun);
        }
        attributes.hp -= damage;
        attributes.hitsCount++;
        combo = 0;
    }

}
