using TMPro;
using UnityEngine;

namespace src.UILibrary
{
    public class playerUI : MonoBehaviour
    {
        public static int maxHealth = 100; //replresents the population of the town
        public static int cases = 1;
        public static int money = 500000;
        public static int nowImmune = 0; // number of recovered people
        public static int dead = 0;
        public static int score = 0; //player score
        public TextMeshProUGUI confirmed;
        public int currentHealth; //replesents the population without covid

        public HealthBar healthBar;
        public TextMeshProUGUI levelIndicator;
        public TextMeshProUGUI PlayerScore;
        public TextMeshProUGUI recovered;
        public TextMeshProUGUI revenue;
        public float revWait = 60f; // time till revenue stats update
        public float revWaitCounter;
        public float waitCounter;
        public float waitTime = 5f; // time till infection stats update

        // Start is called before the first frame update
        private void Start()
        {
            currentHealth = maxHealth;
            healthBar = gameObject.AddComponent<HealthBar>();
            healthBar.setMaxHealth(maxHealth);
            revenue.text = "R 500000";
            waitCounter = waitTime;
            revWaitCounter = revWait;
            levelIndicator.text = "LEVEL 5";
        }

        // Update is called once per frame
        private void Update()
        {
            levelIndicator.text = "LEVEL " + PolicyUI.level;
            waitCounter -= Time.deltaTime;
            revWaitCounter -= Time.deltaTime;

            //check the implemented policy and its rate of infection
            if (waitCounter < 0)
            {

                waitCounter = waitTime;
            }

            if (revWaitCounter < 0)
            {
                getMoney();
                revWaitCounter = revWait;
            }
        }

        //reduce the number of people without the virus
        private void takeDamage(int damage)
        {
            currentHealth -= damage;
            healthBar.setHealth(currentHealth);
            cases += damage;
            confirmed.text = "Confirmed Cases: " + cases;
        }

        //count the number of people who have recovered from the virus
        private void recoveredCount(int people)
        {
            recovered.text = "Recovered: " + people;
        }

        //increase revenue based on the current policy level
        private void getMoney()
        {
            money = money + 100000 / PolicyUI.level;
            revenue.text = "R " + money;
        }
    }
}