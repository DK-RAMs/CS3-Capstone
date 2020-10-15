﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateNewsText : MonoBehaviour
{
    Text newsText;
    // Start is called before the first frame update
    void Start()
    {
        //Text newsText = news.GetComponent<Text>();
        //newsText.text = "test";
        //news.text = "test";
        //Text newsText = GameObject.Find("Canvas/news").GetComponent<Text>();
        //newsText.text = "pls work";
        newsText = GameObject.Find("Canvas/news").GetComponent<Text>(); // find the correct component and make it a text component, i think? - Z
        int count = 0; // counter for getting the correct article - Z
        foreach (string i in Newspaper.content)
        {
            if (count == Newspaper.id)
            {
                newsText.text = i; // set the text equal to this article once count is equal to the id - Z
                break;
            }
            count++; // increment count to test against the id again - Z
        }
    }

    // Update is called once per frame
    void Update()
    {
        int count = 0; // counter for getting the correct article - Z
        foreach (string i in Newspaper.content)
        {
            if (count == Newspaper.id)
            {
                newsText.text = i; // set the text equal to this article once count is equal to the id - Z
                break;
            }
            count++; // increment count to test against the id again - Z
        }
    }
}