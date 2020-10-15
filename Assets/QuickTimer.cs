using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickTimer : MonoBehaviour
{

    public TextMeshProUGUI textBox;
    private float startTime = 4f;
    public static int play;
    public float time;
    public float t;
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
        //startTime = Time.time;
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (play == 1)
        {
            Time.timeScale = 1f;
            time = Time.time;
            t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f3");

            textBox.text = minutes + ":" + seconds;
        }
    }
}
