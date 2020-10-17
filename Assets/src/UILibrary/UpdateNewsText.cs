using UnityEngine;
using UnityEngine.UI;

namespace src.UILibrary
{
    public class UpdateNewsText : MonoBehaviour
    {
        private Text newsText;

        // Start is called before the first frame update
        private void Start()
        {
            //Text newsText = news.GetComponent<Text>();
            //newsText.text = "test";
            //news.text = "test";
            //Text newsText = GameObject.Find("Canvas/news").GetComponent<Text>();
            //newsText.text = "pls work";
            newsText = GameObject.Find("Canvas/news")
                .GetComponent<Text>(); // find the correct component and make it a text component, i think? - Z
            var count = 0; // counter for getting the correct article - Z
            foreach (var i in Newspaper.content)
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
        private void Update()
        {
            var count = 0; // counter for getting the correct article - Z
            foreach (var i in Newspaper.content)
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
}