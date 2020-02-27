using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animator;
    public bool isAttacking, stunned;
    public BoxCollider hitBox;

    public GameObject hitEffect; 

    public bool concatAttack;
    public int combo;
    string damageType;
    PlayerController controller;

    public int damagePoints;

    public void SetController(PlayerController c){
        this.controller = c;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        hitBox.enabled = false;

    }

    

    public void MovementAnimations()
    {

        if(IfInputtingArrows()) 
        Play(1);
        else
        Play(0);

    }

    public void AttackAnimations()
    {
        
        int anim = animator.GetInteger("action");

        if(combo > 3 || !isAttacking)
        combo = 0;

        if(anim == 1 || controller.specialAtivated) return;


        if(Input.GetKeyDown("d") && controller.canUseSpecial){
            damagePoints = (int)controller.attributes.atk;
            Play("special");
            controller.UseSpecial();
            return;
        }

        if(Input.GetKeyDown("a")){
            Play("lightAttack");
            damagePoints = controller.attributes.atk;

            if(combo == 2){
                if(!concatAttack)
                damageType = "heavyHit";
                concatAttack = false;
                damagePoints = (int) (controller.attributes.atk*1.5f);
            }else
            if(combo == 1){
                if(!concatAttack)
                damageType = "lightHit";
                concatAttack = true;
            }else{
                damageType = "lightHit";
                concatAttack = true;
            }

            if(concatAttack)   
            combo++;
        }

        if(Input.GetKeyDown("s")){
            Play("heavyAttack");
            damagePoints = (int) (controller.attributes.atk*1.2f);

            if(combo == 2){
                if(!concatAttack)
                damageType = "fly";
                concatAttack = false;
            damagePoints = (int) (controller.attributes.atk*2f);
            }else
            if(combo == 1){
                if(!concatAttack)
                damageType = "lightHit";
                concatAttack = true;
            }else{
                damageType = "heavyHit";
                concatAttack = true;
            }

            if(concatAttack) 
            combo++;
        }


        
    }

    bool IfInputtingArrows(){
        return Input.GetKey(KeyCode.RightArrow) || 
                Input.GetKey(KeyCode.LeftArrow) || 
                Input.GetKey(KeyCode.UpArrow) || 
                Input.GetKey(KeyCode.DownArrow);
    }

    void StartAttack(){
        isAttacking = true;
        concatAttack = false;
    }

    void EnableHitBox(){
        hitBox.enabled = true;
    }

    void DisableHitBox(){
        hitBox.enabled = false;
    }

    void EndAttack(){
        if(!concatAttack)
            combo = 0;
        isAttacking = false;
    }

    void StartSpecial(){
        isAttacking = true;
        controller.specialAtivated = true;
        controller.GetComponent<Rigidbody>().isKinematic = true;
        controller.GetComponent<Collider>().enabled = false;
    }

    void EndSpecial(){
        isAttacking = false;
        controller.specialAtivated = false;
        controller.GetComponent<Collider>().enabled = true;
        controller.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void StunOn(){
        if(animator.GetBool("dead")) return;
        isAttacking = false;
        stunned = true;
        controller.GetComponent<Rigidbody>().isKinematic = true;
        controller.GetComponent<Collider>().enabled = false;
    }
    public void StunOn(bool invulnerable){
        if(animator.GetBool("dead")) return;
        isAttacking = false;
        stunned = true;
        if(invulnerable){
            controller.GetComponent<Rigidbody>().isKinematic = true;
            controller.GetComponent<Collider>().enabled = false;
        }
    }
    void StunOff(){
        stunned = false;
        concatAttack = false;
        controller.GetComponent<Collider>().enabled = true;
        controller.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void Play(string animation)
    {
        animator.SetTrigger(animation);
    }

    public void Play(int animation)
    {
        animator.SetInteger("action",animation);
    }

    public void CheckCollision(){
        
        Collider[] enemy = Physics.OverlapBox(
            hitBox.transform.position,
            hitBox.transform.localScale,
            hitBox.transform.rotation
        );

        if(enemy.Length == 0) return;

        foreach(Collider e in enemy){
            if(e.gameObject.tag.Equals("Enemy")){
                e.gameObject.GetComponent<EnemyController>().TakeDamage(damageType,damagePoints);
                controller.IncreaseSpecialScore(1);
                GameObject.Instantiate(hitEffect,hitBox.transform.position,new Quaternion());
            }
        }

    }
    

}
