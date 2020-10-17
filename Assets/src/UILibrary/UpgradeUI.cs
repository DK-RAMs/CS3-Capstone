using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public static bool bought = false;
    public static int cost = 300000;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bought == true)
		{
            //playerUI.money -= cost;
            bought = false;
		}
    }

    public void buy()
    {
        if (cost <= playerUI.money)
        {
            bought = true;
        }
    }
}
