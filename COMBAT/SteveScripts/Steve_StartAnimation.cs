using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;

public class Steve_StartAnimation : MonoBehaviour
{
    private Animator animator;



    private void Awake()
    {
        GameObject.FindWithTag("Enemy").GetComponent<AI_Movement>().enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        animator = transform.GetComponent<Animator>();
       // moveScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if animation with name "Attack" finished
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Steve_DummyIntro"))
        {
            animator.SetBool("isTransformed", true);
           
            StartCoroutine(EnableMovement());

        }
        IEnumerator EnableMovement()
        {
            yield return new WaitForSeconds(2);
            GameObject.FindWithTag("Enemy").GetComponent<AI_Movement>().enabled = true;
        }

    }
}
