using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;
using System.Globalization;
using System.Threading;
using Debug = UnityEngine.Debug;
using Random = System.Random;
using src.UILibrary;

namespace src.CitizenLibrary
{
    public class CitizenWorkerThread
    {
        public static Collection<Citizen> citizens = new Collection<Citizen>();
        private int lo, hi, numRebels, numInfected, time;
        private bool happinessUpdated;
        private double averageHappiness;
        private Stopwatch updateTick;
        private static Town town;

        public CitizenWorkerThread(int lo, int hi, bool happinessUpdated)
        {
            this.lo = lo;
            this.hi = hi;
            this.happinessUpdated = happinessUpdated;
            numRebels = 0;
            numInfected = 0;
        }

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
                            }

                        }

                        if (PauseMenu.GameQuit)
                        {
                            Thread.Sleep(new Random().Next(500, 3000));
                            // Save game data here
                            break;
                        }

                        if (!citizens[i].Dead)
                        {
                            citizens[i].Update();
                        }

                        if (Citizen.town.Time == 0 && !happinessUpdated)
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

                        if (Citizen.town.Time == 1 && happinessUpdated)
                        {
                            Debug.Log("Happiness updated");
                            happinessUpdated = false;
                        }
                    }
                }
                time = Town.Time;
            }
        }

        #endregion

        #region Getters and Setters

        public double AverageHappiness()
        {
            double avg = 0;
            Interlocked.Exchange(ref avg, averageHappiness);
            return avg;
        }
        
        
        public int NumRebels => numRebels;

        public int NumInfected => numInfected;

        #endregion
    }
}
