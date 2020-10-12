using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalAchievements : MonoBehaviour
{
    //General variables
    public GameObject acNote;
    public AudioSource acSound;
    public bool acActive;

    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;


    // Achievement 1 specific
    public GameObject acImage;
    public static int ac1Count;
    public int ac1Trigger = 1;
    public int ac1Code;

    // Achievement 2 specific
    //public GameObject acImage;
    public static int ac2Count;
    public int ac2Trigger = 1;
    public int ac2Code;

    // Achievement 3 specific
    //public GameObject acImage;
    public static int ac3Count;
    public int ac3Trigger = 1;
    public int ac3Code;

    // Update is called once per frame
    void Update()
    {
        ac1Code = PlayerPrefs.GetInt("ac1");
        if (ac1Count == ac1Trigger && ac1Code != 1)
		{
            StartCoroutine(Trigger1ac());
		}

        ac2Code = PlayerPrefs.GetInt("ac2");
        if (ac2Count == ac2Trigger && ac2Code != 1)
        {
            StartCoroutine(Trigger2ac());
        }

        ac3Code = PlayerPrefs.GetInt("ac3");
        if (ac3Count == ac3Trigger && ac3Code != 1)
        {
            StartCoroutine(Trigger3ac());
        }
    }

    IEnumerator Trigger1ac()
	{
        acActive = true;
        ac1Code = 1;
        PlayerPrefs.SetInt("ac1", ac1Code);
        acSound.Play();
        acImage.SetActive(true);
        acNote.SetActive(true);
        title.text = "Hello World";
        desc.text = "Complete the Tutorial";

        yield return new WaitForSeconds(5);
        //Reset UI
        resetUI();
    }

    IEnumerator Trigger2ac()
    {
        acActive = true;
        ac2Code = 1;
        PlayerPrefs.SetInt("ac2", ac2Code);
        acSound.Play();
        acImage.SetActive(true);
        acNote.SetActive(true);
        title.text = "Ease of Access";
        desc.text = "Reduce the lockdown level for the first time.";

        yield return new WaitForSeconds(5);
        //Reset UI
        resetUI();
    }

    IEnumerator Trigger3ac()
    {
        acActive = true;
        ac3Code = 1;
        PlayerPrefs.SetInt("ac3", ac3Code);
        acSound.Play();
        acImage.SetActive(true);
        acNote.SetActive(true);
        title.text = "Tough Times";
        desc.text = "Raise the lockdown level for the first time.";

        yield return new WaitForSeconds(5);
        //Reset UI
        resetUI();
    }

    void resetUI()
	{
        acNote.SetActive(false);
        acImage.SetActive(false);
        title.text = "";
        desc.text = "";
        acActive = false;
    }
}
