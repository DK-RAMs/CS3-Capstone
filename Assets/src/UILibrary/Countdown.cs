using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{

    public TextMeshProUGUI textBox;
    public GameObject countPanel;
    private float startTime = 4;
    public static int start;
    public int trigger = 1;
    public int currentTrigger;
    public bool active;
    public float time;

	private void Start()
	{
        time = Time.time;
        Time.timeScale = 0f;
	}

	// Update is called once per frame
	void Update()
    {
        time = Time.time;
        if (start == trigger)
        {
            Time.timeScale = 1f;
            StartCoroutine(countTrigger());
        }
    }

    IEnumerator countTrigger()
	{
        currentTrigger = 1;
        countPanel.SetActive(true);
        active = true;
        float t = startTime - Time.time; 
        if ((int)(t % 60) <= 0)
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
            string seconds = (t % 60).ToString("f2");
            seconds = seconds.Remove(1);
            textBox.text = seconds;
        }
    }
}
