using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve_Death : MonoBehaviour
{
    ScoreBar scorebar;
    AI_Animation ai;
    // Start is called before the first frame update
    void Start()
    {
        ai= GameObject.FindGameObjectWithTag("Enemy").GetComponent<AI_Animation>();
        scorebar = GameObject.FindGameObjectWithTag("scoreTracker").GetComponent<ScoreBar>();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    SteveDeath();
    //}

    //void SteveDeath()
    //{
    //    if (scorebar.slider.value >= 20)
    //    {
    //        ai.Death();
    //        //this.gameObject.SetActive(false);
    //        //GameObject.FindWithTag("Player").GetComponent<Pamuk_Move>().enabled = false;

    //    }
    //}
}
