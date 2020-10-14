using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace src.UILibrary
{
    public class HealthBar
    {
        public Slider slider; // Must be static since there is only 1 global health bar
        public Gradient gradient; // Must be static since there is only 1 global health bar
        public Image fill; // Must be static since there is only 1 global health bar

        public HealthBar()
        {
            
        }
        
        public void setMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;
            fill.color = gradient.Evaluate(1f);
        }

        public void setHealth(int health)
        {
            slider.value = health;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}