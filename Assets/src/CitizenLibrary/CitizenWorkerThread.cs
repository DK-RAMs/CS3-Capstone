using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;
using System.Globalization;
using System.Threading;
<<<<<<< HEAD
=======
using src.SaveLoadLibrary;
>>>>>>> DK-Branch
using Debug = UnityEngine.Debug;
using Random = System.Random;
using src.UILibrary;

namespace src.CitizenLibrary
{
    public class CitizenWorkerThread
    {
        public static Collection<Citizen> citizens = new Collection<Citizen>();
<<<<<<< HEAD
        private int lo, hi, numRebels, numInfected, time;
        private bool happinessUpdated;
        private double averageHappiness;
        private Stopwatch updateTick;
        private static Town town;
=======
        private int lo, hi, numRebels, numInfected, numDead, time;
        private bool happinessUpdated;
        private double averageHappiness;
>>>>>>> DK-Branch

        public CitizenWorkerThread(int lo, int hi, bool happinessUpdated)
        {
            this.lo = lo;
            this.hi = hi;
            this.happinessUpdated = happinessUpdated;
            numRebels = 0;
            numInfected = 0;
        }

<<<<<<< HEAD
        public static Town Town
        {
            get => town;
            set { town = value; }
        }

        #region Update Methods

        public void update()
        {
            while (!Town.townReady)
            {

            }

            while (true)
            {
                if (time != town.Time)
                {
                    for (int i = lo; i < hi; i++)
                    {
                        while (PauseMenu.GameIsPaused)
                        {
                            if (PauseMenu.GameQuit) // Game Quit somewhere dude
                            {
                                break;
=======
        #region Update Methods

        public void Update()
        {
            while (!Game.GAMESTART)
            {
                
            }
            Debug.Log("Citizen thread managing citizens " + lo + " to " + hi + " have begun executing their tasks");
            while (true)
            {
                if (time != Game.town.Time)
                {
                    for (int i = lo; i < hi; i++)
                    {
                        while (Game.GAMEPAUSED)
                        {
                            if (Game.GAMEQUIT) // Game Quit somewhere dude
                            {
                                while (!Game.GAMECLOSED)
                                {
                                    
                                }
                                return;
>>>>>>> DK-Branch
                            }

                        }

<<<<<<< HEAD
                        if (PauseMenu.GameQuit)
                        {
                            Thread.Sleep(new Random().Next(500, 3000));
                            // Save game data here
                            break;
=======
                        if (Game.GAMEQUIT)
                        {
                            return;
>>>>>>> DK-Branch
                        }

                        if (!citizens[i].Dead)
                        {
                            citizens[i].Update();
                        }

<<<<<<< HEAD
                        if (Citizen.town.Time == 0 && !happinessUpdated)
=======
                        if (Game.town.Time == 0 && !happinessUpdated)
>>>>>>> DK-Branch
                        {
                            averageHappiness = 0;
                            happinessUpdated = true;
                            for (int j = lo; j < hi; j++)
                            {
                                if (!citizens[i].Dead)
                                {
                                    averageHappiness += citizens[i].Happiness;
                                }
                            }

                            Interlocked.Exchange(ref this.averageHappiness, averageHappiness /= (hi - lo));
                        }

<<<<<<< HEAD
                        if (Citizen.town.Time == 1 && happinessUpdated)
=======
                        if (Game.town.Time == 1 && happinessUpdated)
>>>>>>> DK-Branch
                        {
                            Debug.Log("Happiness updated");
                            happinessUpdated = false;
                        }
                    }
                }
<<<<<<< HEAD
                time = Town.Time;
=======
                time = Game.town.Time;
>>>>>>> DK-Branch
            }
        }

        #endregion

<<<<<<< HEAD
=======
        #region Save State

        #endregion
        
>>>>>>> DK-Branch
        #region Getters and Setters

        public double AverageHappiness()
        {
            double avg = 0;
            Interlocked.Exchange(ref avg, averageHappiness);
            return avg;
        }
<<<<<<< HEAD
        
        
=======

>>>>>>> DK-Branch
        public int NumRebels => numRebels;

        public int NumInfected => numInfected;

        #endregion
    }
}
