using System;
using src.CitizenLibrary;
using src.SaveLoadLibrary;
using UnityEngine;

namespace src
{
    public class Game : MonoBehaviour
    {
        public static Town town;
        public static bool gameQuit = false;
        public static bool gamePaused = false;
        
        public void Start()
        {
            string townID = "mainTown";
            town = FileManagerSystem.LoadTown(townID);
            town.Start();
        }

        public void Update()
        {
            town.Update();
        }
        
        
        
        public void OnApplicationQuit()
        {
            gameQuit = true;
            for (int i = 0; i < Town.Threads.Length; i++)
            {
                Town.Threads[i].Join();
            }
            FileManagerSystem.SaveCitizens(town, CitizenWorkerThread.citizens);
            FileManagerSystem.SaveTown(town);
        }
    }
}