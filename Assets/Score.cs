using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//don't know if actually gonna end up using this class but here as a template if I end up wanting to use cloud saves

public class Score : MonoBehaviour
{
    public static int score = 0;
    public TextMeshProUGUI scoreText;

    public void IncrementScore()
    {
        score++;
        scoreText.text = score.ToString();
        PlayerPrefs.SetInt("ScoreToUpdate", PlayerPrefs.GetInt("ScoreToUpdate", 0) + 1); // sending scoretoupdate to server, we wipe it afterwards but this is here for the purpose of sending the new score to server - Z
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
