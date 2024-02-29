using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    public Slider slider;

    //fürTestzwecke
    public bool buff;
    public bool debuff;
    public int buffValue;
    
    void Start()
    {
        SetStartValue(buffValue);
    }

    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.A))
        {
            changeScore(-1); //put the taken damage in o3o
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            changeScore(1); //put the damage you make in o3o
        }*/
    }

    public void SetMinMaxValue(int maxValue)
    {
        slider.minValue = -maxValue;
        slider.maxValue = maxValue;
    }

    public void SetStartValue(int buffValue)
    {
        if(buff == true)
        {
            slider.value = buffValue;
        }
        else if(debuff == true)
        {
            slider.value = -buffValue;
        }
        else
        {
            slider.value = 0;
        }
    }

    public void SetValueBar(int valueScore)
    {
        slider.value = Mathf.Clamp(valueScore, slider.minValue, slider.maxValue); 
    }

    public void changeScore(int valueScore)
    {
        slider.value += valueScore;
    }

    public int GetScoreValue()
    {
        return (int)slider.value;
    }
}
