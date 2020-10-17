using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace src.UILibrary
{
    public class UpgradeUI : MonoBehaviour
    {

        public int cost = 300000;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void buy()
        {
            if (cost <= playerUI.money)
            {
                playerUI.money -= cost;
            }
        }
    }
}