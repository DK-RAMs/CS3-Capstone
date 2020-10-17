using src.CitizenLibrary;
using UnityEngine;
using UnityEngine.UI;

namespace src.UILibrary
{
    public class HealthBar : MonoBehaviour
    {
        public Image fill;
        public Gradient gradient;
        public Slider slider;
        public Town town;
        
        // sets maximum player health
        public void setMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;

            fill.color = gradient.Evaluate(1f);
        }

        // sets current player health as game progresses 
        public void setHealth(int health)
        {
            slider.value = health;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}