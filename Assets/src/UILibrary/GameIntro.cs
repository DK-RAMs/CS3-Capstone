using UnityEngine;

namespace src.UILibrary
{
    public class GameIntro : MonoBehaviour
    {
        private int show;
        public GameObject welcome;


        // Start is called before the first frame update
        private void Start()
        {
            if (PlayerPrefs.HasKey("welcome"))
            {
                // not the firs start
                welcome.SetActive(false);
            }
            else
            {
                //first start
                PlayerPrefs.SetInt("welcome", 0);
                PlayerPrefs.Save();
                //show welcome
                welcome.SetActive(true);
            }
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}