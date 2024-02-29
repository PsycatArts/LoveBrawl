using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve_FireballAbility : MonoBehaviour
{
    public Transform firePoint;
    public GameObject firePrefab;

    //// Update is called once per frame
    //void Update() 
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        Shoot();
    //    }
    //}

    public void Shoot()
    {
        GameObject a = Instantiate(firePrefab, firePoint.position, firePoint.rotation);
       
        Destroy(a, 2);
        //SoundManager.PlaySound("Steve_FireBall");            //play attacksound
    }
}
