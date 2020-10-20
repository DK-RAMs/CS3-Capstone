using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace src.UILibrary
{
    public class UpgradeUI : MonoBehaviour
    {
        public static bool bought = false;

        public static int cost = 300000;

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