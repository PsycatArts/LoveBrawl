using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerCountdown : MonoBehaviour
{
    private float maxDuration = 60;
    private float currentTime;


    [SerializeField] private float maxDurationTest; //testing stuff yey

    [SerializeField] private Image img;
    [SerializeField] private TMP_Text timeText;

   // private GameObject fightOver;

    private void Start()
    {
        setDuration(maxDurationTest); //testing stuff yey
        img.fillAmount = 0f;
        StartRoutine();

        //fightOver = GameObject.Find("FightEndScreen");
       // fightOver.SetActive(false);
    }
    private void Update()
    {
       // TimeEnd();
    }

    void setDuration(float duration)
    {
        maxDuration = currentTime;
        currentTime = duration;
    }

    public void StartRoutine()
    {
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(currentTime >= 0)
        {
            updateUI(currentTime);
            currentTime--;
            yield return new WaitForSeconds(1f);
        } 
    }

    void updateUI(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay);

        if(timeToDisplay < 10)
        {
            timeText.SetText("0{00}", seconds);
        }
        else
        {
            timeText.SetText("{00}", seconds);
        }

        img.fillAmount = Mathf.InverseLerp(maxDuration, 0, timeToDisplay);
    }
    //void TimeEnd()
    //{
    //    if (currentTime <= 0)
    //    {
    //        Debug.Log("Time is up");
    //        
    //        GameObject.FindWithTag("Enemy").GetComponent<AI_Movement>().enabled = false;
    //        GameObject.FindWithTag("Player").GetComponent<Pamuk_Move>().enabled = false;
    //        GameObject.FindWithTag("Player").GetComponent<PamukAnims>().enabled = false;
    //        fightOver.SetActive(true);
    //    }
    //}
}
