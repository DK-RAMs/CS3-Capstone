using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static Slider slider; // Must be static since there is only 1 global health bar
    public static Gradient gradient; // Must be static since there is only 1 global health bar
    public static Image fill; // Must be static since there is only 1 global health bar

    public static void setMaxHealth(int health){
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public static void setHealth(int health){
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
