using System.Collections;
using TMPro;
using UnityEngine;

namespace src.UILibrary
{
    public class Countdown : MonoBehaviour
    {
        public static int start;
        public bool active;
        public GameObject countPanel;
        public int currentTrigger;
        private readonly float startTime = 4;

        public TextMeshProUGUI textBox;
        public float time;
        public int trigger = 1;

        private void Start()
        {
            time = Time.time;
            Time.timeScale = 0f;
        }

        // Update is called once per frame
        private void Update()
        {
            time = Time.time;
            if (start == trigger)
            {
                Time.timeScale = 1f;
                StartCoroutine(countTrigger());
            }
        }

        private IEnumerator countTrigger()
        {
            currentTrigger = 1;
            countPanel.SetActive(true);
            active = true;
            var t = startTime - Time.time;
            if ((int) (t % 60) <= 0)
            {
                t = 0;
                textBox.text = "Go";
                yield return new WaitForSeconds(1);
                QuickTimer.play = 1;
                Destroy(textBox);
                countPanel.SetActive(false);
                active = false;
                start = 0;
                Time.timeScale = 0f;
            }
            else
            {
                var seconds = (t % 60).ToString("f2");
                seconds = seconds.Remove(1);
                textBox.text = seconds;
            }
        }
    }
}