using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

using src.CitizenLibrary;

namespace src
{
    public class Init : MonoBehaviour
    {

        public Town town;
        public static string townID;
        public void Start() // Linked to a Scene. Starts everything
        {
            CitizenTask.taskKeys.Add(0, ("Go Drinking", true));
            CitizenTask.taskKeys.Add(1, ("Sleep", true));
            CitizenTask.taskKeys.Add(2, ("Work", true));
            CitizenTask.taskKeys.Add(3, ("go shopping", true));
            CitizenTask.taskKeys.Add(4, ("Visit friend", true));
            CitizenTask.taskKeys.Add(5, ("Stay at home", true));
            CitizenTask.taskKeys.Add(6, ("Self Quarantine", true));
            CitizenTask.taskKeys.Add(7, ("Recover in hospital", true));
            Citizen.initializeRandomizer();
            town = new Town("yay", "Matimba", 0, 8, 60000, 60000 / 24, 1, 15, 5, 20);
            town.addGamePolicies(7);
            town.Start();
            Debug.Log(Town.townReady);
            // town = FileManagerSystem.LoadTown(townID);
        }

        public void Update()
        {
            town.Update(); // Update is called on this script's Town object
        }
    }
}