using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Policy : MonoBehaviour
{
    public TextMeshProUGUI currentLevel;
    public TextMeshProUGUI policy;
    public TextMeshProUGUI stats;

    public static int level;
    public int max = 5;
    public int min = 1;

    // Start is called before the first frame update
    void Start()
    {
        level = 5;
    }

    // Update is called once per frame
    void Update()
    {
        currentLevel.text = "Current Level: " + level;
    }

    public void policy1()
	{
        policy.text = "Business as almost usual";
	}

    public void policy2()
    {
        policy.text = "60% freedom";
    }
    public void policy3()
    {
        policy.text = "40% freedom";
    }
    public void policy4()
    {
        policy.text = "20% freedom";
    }
    public void policy5()
    {
        policy.text = "Community Policies\n" +
            "1.all non - essential business closed\n" +
            "2.all non - essential businesses limited capacity opening" +
            "\nIndividual Policies\n" +
            "1.mask wearing required at all times in public\n" +
            "2. no gatherings of any sort allowed";
    }

    public void showstats()
	{
        stats.text = "Population: 200\nConfirmed cases: 1\nRecovered: 0\nDead: 0";
	}
    public void levelReduce()
	{
        if (level != min)
        {
            Policy.level -= 1;
        }
    }

    public void levelRaise()
    {
        if (level != max)
        {
            Policy.level += 1;
        }
    }
}
