using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve_go : MonoBehaviour
{
    //walking and chasing
    public float speed;
    public float stoppingDistance;
    public Animator animator;

    public Transform target;        //finding player target

    //flip sprite
    bool facingRight;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();  //get player transform component
        animator = transform.GetComponent<Animator>();      //get steve animator
    }

    void Update()
    {
        if(Vector2.Distance(transform.position,target.position) <= stoppingDistance)        //if the distance to player is smaller than Value, 
        {
            animator.SetBool("playerIsClose", true);                               //then start chasing player and go to run anim
                                                                            //also check for player direction and flip sprite
            if (target.position.x > transform.position.x && !facingRight) //if the target is to the right of enemy and the enemy is not facing right
                Flip();
            if (target.position.x < transform.position.x && facingRight)
                Flip();
        }
        else
        {
            animator.SetBool("playerIsClose", false);
        }
    }
    void Flip()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }
}
