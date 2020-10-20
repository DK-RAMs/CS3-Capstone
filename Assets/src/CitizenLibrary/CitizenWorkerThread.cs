﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;
using System.Globalization;
using System.Threading;
using src.SaveLoadLibrary;
using Debug = UnityEngine.Debug;
using Random = System.Random;
using src.UILibrary;

namespace src.CitizenLibrary
{
    public class CitizenWorkerThread
    {
        public static Collection<Citizen> citizens = new Collection<Citizen>();
        private int lo, hi, numRebels, numInfected, numDead, time;
        private bool happinessUpdated;
        private double averageHappiness;
        private SemaphoreSlim CitizenAccessor;

        public CitizenWorkerThread(int lo, int hi, bool happinessUpdated)
        {
            this.lo = lo;
            this.hi = hi;
            this.happinessUpdated = happinessUpdated;
            numRebels = 0;
            numInfected = 0;
        }

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
                    Debug.Log("Citizen thread managing citizens " + lo + " to " + hi + " is Updating...");
                    for (int i = lo; i < hi; i++)
                    {
                        if (Game.GAMEPAUSED)
                        {
                            Debug.Log("Game is Paused");
                            while (Game.GAMEPAUSED)
                            {
                                if (Game.GAMEQUIT) // Game Quit somewhere dude
                                {
                                    return;
                                }
                            }
                            Debug.Log("Game has been resumed");
                        }

                        if (Game.GAMEQUIT)
                        {
                            return;
                        }

                        if (!citizens[i].Dead)
                        {
                            citizens[i].Update();
                        }

                        if (Game.town.Time == 0 && !happinessUpdated)
                        {
                            double happiness = 0;
                            averageHappiness = 0;
                            happinessUpdated = true;
                            for (int j = lo; j < hi; j++)
                            {
                                if (!citizens[i].Dead)
                                {
                                    happiness += citizens[i].Happiness;
                                }
                            }

                            averageHappiness = happiness;

                            Interlocked.Exchange(ref this.averageHappiness, averageHappiness /= (hi - lo));
                        }
                        if (Game.town.Time == 1 && happinessUpdated)
                        {
                            Debug.Log("Happiness updated");
                            happinessUpdated = false;
                        }
                        Debug.Log("Citizen thread managing citizens " + lo + " to " + hi + " has finished updating");
                    }
                    time = Game.town.Time;
                    
                }
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