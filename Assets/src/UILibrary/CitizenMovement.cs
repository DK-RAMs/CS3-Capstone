using UnityEngine;

namespace src.UILibrary
{
    public class CitizenMovement : MonoBehaviour
    {
        public bool isWalking; // checks if citizen is currently walking

        public float moveSpeed; // speed at which citizens move
        private Rigidbody2D mybody; // represents the citizen
        private float waitCounter;
        public float waitTime;
        private float walkCounter;

        private int walkDirection; // direction in which citizen walks 

        public float walkTime;

        // Start is called before the first frame update
        private void Start()
        {
            mybody = GetComponent<Rigidbody2D>();
            waitCounter = waitTime;
            walkCounter = walkTime;

            chooseTime(); // selects citizen walk time and wait time
            chooseDirection(); // selects walk direction
        }

        // Update is called once per frame
        private void Update()
        {
            if (isWalking)
            {
                walkCounter -= Time.deltaTime;
                Vector2 cscale = transform.localScale;
                switch (walkDirection)
                {
                    case 0: // citizen moves right
                        cscale.x = 83;
                        mybody.velocity = new Vector2(-moveSpeed, 0);
                        break;
                    case 1: // citizen moves left
                        cscale.x = -83;
                        mybody.velocity = new Vector2(moveSpeed, 0);
                        break;
                }

                transform.localScale = cscale;


                if (walkCounter < 0)
                {
                    isWalking = false; // citizen is now standing
                    waitCounter = waitTime;
                }
            }
            else
            {
                waitCounter -= Time.deltaTime;
                mybody.velocity = Vector2.zero;

                if (waitCounter < 0) chooseDirection();
            }
        }

        //chooses citizen direction
        public void chooseDirection()
        {
            walkDirection = Random.Range(0, 2);
            isWalking = true;
            walkCounter = walkTime;
        }

        // chooses wait time and walk time
        public void chooseTime()
        {
            waitTime = Random.Range(2, 4);
            walkTime = Random.Range(0, 5);
        }
    }
}