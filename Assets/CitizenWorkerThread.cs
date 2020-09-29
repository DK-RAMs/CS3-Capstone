using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;
using System.Globalization;


namespace CitizenLibrary
{
    public class CitizenWorkerThread
    {
        public static Collection<Citizen> citizens;
        private int lo, hi;
        public bool closed;
        private bool happinessUpdated;
        private double averageHappiness;
        private Stopwatch updateTick;
        private int numRebels;

        public CitizenWorkerThread(int lo, int hi, bool happinessUpdated)
        {
            this.lo = lo;
            this.hi = hi;
            this.happinessUpdated = happinessUpdated;
        }

        public void update()
        {
            while (true)
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
                        closed = true;
                        break;
                    }
                    
                    citizens[i].update();
                    if (Town.Time == 0 && !happinessUpdated)
                    {
                        happinessUpdated = true;
                        for (int j = lo; j < hi; j++)
                        {
                            
                        }
                    }
                    if (Town.Time == 1 && happinessUpdated)
                    {
                        happinessUpdated = false;
                    }
                }
            }

        }
    }
}
