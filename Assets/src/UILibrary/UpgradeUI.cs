using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace src.UILibrary
{
    public class UpgradeUI : MonoBehaviour
    {
        public static bool bought = false;

        [SerializeField] private Button button;

        public static int cost = 300000;

        public void Start()
        {
            button.onClick.AddListener(() => buy());
        }
        
        public void buy()
        {
            if (cost <= Game.town.Money)
            {
                Game.town.processDeltaMoney(cost*-1);
                bought = true;
            }
        }
    }
}