using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    public Animator animator;
    public Collider hitBox;

    public GameObject hitEffect;

    public bool stunned;
    public bool isAttacking;

    EnemyController controller;

    public void SetController(EnemyController c){
        this.controller = c;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void StartAttack(){
        isAttacking = true;
    }

    void EnableHitBox(){
        hitBox.enabled = true;
    }

    void DisableHitBox(){
        hitBox.enabled = false;
    }

    void EndAttack(){
        isAttacking = false;
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
        isAttacking = false;
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
        Collider[] player = Physics.OverlapBox(
            hitBox.transform.position,
            hitBox.transform.localScale,
            hitBox.transform.rotation
        );

        if(player.Length == 0) return;

        foreach(Collider p in player){
            if(p.gameObject.tag.Equals("Player")){
                p.gameObject.GetComponentInParent<PlayerController>().TakeDamage(
                    controller.damageAnimation,
                    controller.attributes.atk,
                    controller.transform
                );
                GameObject.Instantiate(hitEffect,hitBox.transform.position,new Quaternion());
            }
        }

    }
}
