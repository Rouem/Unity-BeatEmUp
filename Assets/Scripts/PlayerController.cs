using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody rb;

    public Attributes attributes;
    public float horizontalVelocity;
    public float verticalVelocity;
    int horizontalDirection, verticalDirection;
    PlayerAnimations pa;
    public GameObject cam, specialEffect;
    int specialScore;
    public int minScore;

    public bool canUseSpecial = false, specialAtivated = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pa = GetComponentInChildren<PlayerAnimations>();
        pa.SetController(this);
        attributes = GetComponentInChildren<Attributes>();
        GetComponent<PlayerUI>().ReloadUI();
        specialEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        specialEffect.transform.rotation = Quaternion.Euler(0,cam.transform.eulerAngles.y,0);
        
        if(attributes.hp <= 0){
            pa.animator.SetBool("dead",true);
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            GetComponent<PlayerUI>().Reload.SetActive(true);
            GetComponent<PlayerUI>().SideBar.SetActive(false);
            if(Input.anyKeyDown)
                SceneManager.LoadScene(0);
            else
                return;
        }

        if(pa.stunned) return;

        Direction();
        pa.AttackAnimations();        
    }
    
    void Move(){

        Vector3 move = transform.position + 
        transform.forward * horizontalDirection * horizontalVelocity * Time.deltaTime + 
        cam.transform.forward * verticalDirection * verticalVelocity * Time.deltaTime;

        rb.MovePosition(move);
    }

    void Direction(){
        
        if(pa.isAttacking || pa.stunned) return;

        pa.MovementAnimations();

        if(Input.GetKey(KeyCode.RightArrow)){
            transform.rotation = Quaternion.Euler(0,90,0);
            horizontalDirection = 1;
            Move();
        }else
        if(Input.GetKey(KeyCode.LeftArrow)){
            transform.rotation = Quaternion.Euler(0,-90,0);
            horizontalDirection = 1;
            Move();
        }else{
            horizontalDirection = 0;
        }
        
        if(Input.GetKey(KeyCode.UpArrow)){
            verticalDirection = 1;
            Move();
        }else
        if(Input.GetKey(KeyCode.DownArrow)){
            verticalDirection = -1;
            Move();
        }else{
            verticalDirection = 0;
        }
    }

    public void FixLook(Transform target){
        if(target.transform.position.x >= transform.position.x){
            transform.rotation = Quaternion.Euler(0,90,0);
        }else{
            transform.rotation = Quaternion.Euler(0,-90,0);
        }
    }

    public void TakeDamage(string hitType, int damage, Transform target)
    {
        if(pa.stunned && attributes.invencibleOnStun || pa.stunned && hitType.Equals("fly")) return;
        Debug.Log(transform.name+" take "+damage+" of damage!");
        if(attributes.IsStunned()){
            pa.Play(hitType);
            FixLook(target);
            if(hitType.Equals("fly"))
                    pa.StunOn(true);
                else
                    pa.StunOn(attributes.invencibleOnStun);
        }
        attributes.hp -= damage;
        pa.combo = 0;
        GetComponent<PlayerUI>().ReloadUI();
    }

    public void IncreaseSpecialScore(int increasePoints){
        specialScore += increasePoints;
        if(specialScore >= minScore){
            specialEffect.SetActive(true);
            canUseSpecial = true;
        }
    }

    public void UseSpecial(){
        specialScore -= minScore;
        if(specialScore < minScore){
            specialEffect.SetActive(false);
            canUseSpecial = false;
        }

    }

    public bool IsStunned(){
        return pa.stunned;
    }

}
