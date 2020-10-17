using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class happinessBar : MonoBehaviour
{
    public Slider slider;

    public void setMaxHappy(int happy){
        slider.maxValue = happy;
        slider.value = happy;
    }

    public void setHealth(int happy){
        slider.value = happy;
    }
}
