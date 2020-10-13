using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace src.UILibrary
{
    public class happinessBar : MonoBehaviour
    {
        public static Slider slider;

        public static void setMaxHappy(int happy)
        {
            slider.maxValue = happy;
            slider.value = happy;
        }

        public static void setHealth(int happy)
        {
            slider.value = happy;
        }
    }
}