﻿using TMPro;
using UnityEngine;

namespace src.UILibrary
{
    public class Timer : MonoBehaviour
    {
        private int day;
        public TextMeshProUGUI dayBox;
        public bool paused;
        private float startTime;
        public TextMeshProUGUI textBox;

        private void Start()
        {
            startTime = Time.time;
            day = 1;
        }

        private void Update()
        {
            if (paused) return;

            var t = Time.time - startTime;

            var minutes = ((int) t / 60).ToString();
            var seconds = (t % 60).ToString("f2");


            var m = (int) t / 60;
            if (m == 5)
            {
                startTime = Time.time;
                day += (int) t / 300;
            }

            seconds = seconds.Remove(2);
            if (seconds.Contains(","))
                textBox.text = minutes + ":" + seconds.Remove(1);
            else
                textBox.text = minutes + ":" + seconds;

            dayBox.text = "Day " + day;
        }
    }
}