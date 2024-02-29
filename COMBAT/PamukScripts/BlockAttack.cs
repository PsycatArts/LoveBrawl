using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAttack : MonoBehaviour
{
    public BoxCollider2D hurtbox;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInParent<Animator>();
        hurtbox = transform.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
       Block();
    }

    //block (deactivate collider & set animation)
    void Block()
    {
        if (Input.GetKeyDown(KeyCode.Space))        //if blocking button pressed
        {
            hurtbox.enabled = false;         //set hurtbox collider box DEACTIVATED
            animator.SetBool("isBlocking", true);       //play animation
            StartCoroutine(CollDeac());         //wait for seconds and activate collider again
        }
    }
    //method to wait for seconds before doing: reactivate collider and set animation to wrong
    IEnumerator CollDeac()      
    {
        yield return new WaitForSeconds(.8f);
        hurtbox.enabled = true;
        animator.SetBool("isBlocking", false);
    }
    //if block has been hit, then do sth..
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy"))
    //    {
    //        Debug.Log("I blocked the attack of sth");
    //       // SoundManager.PlaySound("P_Block");
    //    }
    //}
}
