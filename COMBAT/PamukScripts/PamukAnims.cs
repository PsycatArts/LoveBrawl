using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Script takes care of the walking animation of PAMUK
/// </summary>
public class PamukAnims : MonoBehaviour
{
    
    float horizontalMove = 0f;      //variable to store our value so we can set the float value of our animation trigger to higher(activate animation)
                                    //the run animation will trigger if Speed is greather than 0.01f

    public Animator animator;       //refference animator in inspector

    // every frame check...
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");    //set variable to our Input (which is left&right/ will go up to -1 or  1) 
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));      //set the float of our animator trigger (Mathf.Abs makes sure it's not a negative value)

    }
}
