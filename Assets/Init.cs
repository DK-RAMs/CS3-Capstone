using System;
using UnityEngine;

namespace CitizenLibrary
{
    public class Init : MonoBehaviour
    {

        public Town town;
        public void Start() // Linked to a Scene. Starts everything
        {
            CitizenTask.taskKeys.Add(0, ("Go Drinking", true));
            CitizenTask.taskKeys.Add(1, ("Sleep", true));
            CitizenTask.taskKeys.Add(2, ("Work", true));
            CitizenTask.taskKeys.Add(3, ("go shopping", true));
            CitizenTask.taskKeys.Add(4, ("Stay at home", true));
            CitizenTask.taskKeys.Add(5, ("Self Quarantine", true));
            Citizen.initializeRandomizer();
            town = new Town("yay", 0, 8, 60000, 60000 / 24, 1, 15, 5, 20);
            town.Start();
        }

        public void Update()
        {
            town.Update(); // Update is called on this script's Town object
        }
    }
}