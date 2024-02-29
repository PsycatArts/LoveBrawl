using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Animator camAnim;

    public void CamShake()
    {
        camAnim.SetTrigger("shake");
    }
    public void CamShakeHeavy()
    {
        camAnim.SetTrigger("heavyShake");
    }

    //put following code in any script to call shake
    //private Shake shake;
    //shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
    //shake.CamShake();
}
