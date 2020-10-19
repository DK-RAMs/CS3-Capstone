<<<<<<< Updated upstream
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace src.NewspaperLibrary
{
    public class UpdateNewsText : MonoBehaviour // This file doesn't have to exist. See the Newspaper.cs file
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
            newsText = GameObject.Find("Canvas/news")
                .GetComponent<Text>(); // find the correct component and make it a text component, i think? - Z
            int count = 0; // counter for getting the correct article - Z
            /*
            foreach (string i in Newspaper.content)
            {
                if (count == Newspaper.id)
                {
                    newsText.text = i; // set the text equal to this article once count is equal to the id - Z
                    break;
                }

                count++; // increment count to test against the id again - Z
            }*/
        }

        // Update is called once per frame
        void Update()
        {
            /*
            int count = 0; // counter for getting the correct article - Z
            foreach (string i in Newspaper.content)
            {
                if (count == Newspaper.id)
                {
                    newsText.text = i; // set the text equal to this article once count is equal to the id - Z
                    break;
                }

                count++; // increment count to test against the id again - Z
            }*/
        }
    }
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace src.NewspaperLibrary
{
    public class UpdateNewsText : MonoBehaviour // Turns out, we do need this file
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
            newsText = GameObject.Find("Canvas/news")
                .GetComponent<Text>(); // find the correct component and make it a text component, i think? - Z
            int count = 0; // counter for getting the correct article - Z
            /*
            foreach (string i in Newspaper.content)
            {
                if (count == Newspaper.id)
                {
                    newsText.text = i; // set the text equal to this article once count is equal to the id - Z
                    break;
                }

                count++; // increment count to test against the id again - Z
            }*/
        }

        // Update is called once per frame
        void Update()
        {
            int count = 0; // counter for getting the correct article - Z
            foreach (string i in Newspaper.content)
            {
                /*
                if (count == Newspaper.id)
                {
                    newsText.text = i; // set the text equal to this article once count is equal to the id - Z
                    break;
                }*/

                count++; // increment count to test against the id again - Z
            }
        }
    }
>>>>>>> Stashed changes
}