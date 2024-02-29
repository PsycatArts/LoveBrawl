using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to play Transform animation at beginning and go over to idle when finished
/// still needs to freeze player input for a moment
/// </summary>
public class StartAnim : MonoBehaviour
{
    private Animator animator;

    //bool Froze = true;
    private Pamuk_Move moveScript;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<Pamuk_Move>();
        animator = transform.GetComponent<Animator>();
        moveScript.enabled = false;
       // SoundManager.PlaySound("Fight_Start");
    }

    // Update is called once per frame
    void Update()
    {
        //if animation with name "Attack" finished
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Wolf_Transformation"))
        {
            animator.SetBool("isTransformed", true);
            StartCoroutine(EnableMovement());
        }
        IEnumerator EnableMovement()
        {
            yield return new WaitForSeconds(2);
            moveScript.enabled = true;

        }
    }
}
