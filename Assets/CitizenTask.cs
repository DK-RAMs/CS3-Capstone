using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using CitizenLibrary;
using UnityEngine;
using UnityEngine.Analytics;
using Random = System.Random;

namespace CitizenLibrary
{
    public class CitizenTask
    {
        string taskName;
        private static double MAXBASEHAPPINESSGAIN = 2;
        private double happinessModifier;
        private int taskID;
        public static Dictionary<int, (string, bool)> taskKeys = new Dictionary<int, (string, bool)>();
        bool completed;
        private bool available;
        int startTime, endTime;
        private Building taskLocation;
        
        #region Constructors
        public CitizenTask(int taskID, int startTime, int endTime, int completed)
        {
            (string, bool) task;
            if (!taskKeys.TryGetValue(taskID, out task))
            {
                Console.WriteLine("ERROR! Task #" + taskID + " is not defined");
            }
            taskName = taskKeys[taskID].Item1;
            available = taskKeys[taskID].Item2;
            this.startTime = startTime;
            this.endTime = endTime;
            available = true;
            if (completed == 0)
            {
                this.completed = false;
            }
            else{
                this.completed = true;
            }

        }
        #endregion
        
        #region Task Selection methods
        public double calculateTaskHappiness(Random random, bool rebel, double modifier) // Happiness Modifier is applied in the case where a citizen completes their "favorite task"
        {
            double rebelFactor = 1;
            if (rebel)
            {
                rebelFactor = 1.15;
            }
            return 1+random.NextDouble()*MAXBASEHAPPINESSGAIN*rebelFactor + modifier;
        }

        public void generateNewTask(Random random, Citizen citizen)
        {
            switch (citizen.CitizenOccupation) // Will do this later maybe
            {
                case Citizen.Occupation.Employed:
                    break;
                case Citizen.Occupation.Retired:
                    break;
                case Citizen.Occupation.Student:
                    break;
                case Citizen.Occupation.Unemployed:
                    break;
            }
            Debug.Log(Citizen.Town.Time);
            taskID = random.Next(0, taskKeys.Count-1);
            
            startTime = Citizen.Town.Time;
            taskName = taskKeys[taskID].Item1;
            available = taskKeys[taskID].Item2;
            switch (taskID)
            {
                case 0:
                    if (available)
                    {
                        HashSet<Building> bars = Citizen.Town.Recreational;
                        int attempts = 0;
                        while (true)
                        {
                            attempts++;
                            int selection = random.Next(0, bars.Count - 1);
                            // NB!!! Collection was changed to HashSet (easier to access individual buildings if the data is stored in a hashset
                            
                            /*
                            if (bars[selection].enterBuilding(citizen))
                            {
                                taskLocation = bars[selection]; // Change hash table accessors
                                endTime = startTime + random.Next(1, 3);
                                break;
                            }*/
                            
                            if (attempts == 10)
                            {
                                Debug.Log("Citizen wasn't able to go to the bar. They went home feeling upset");
                                happinessModifier = random.Next(1, 5)*-1;
                            }
                        }
                        break;
                    }
                    else
                    {
                        break;
                    }
                case 1:
                    endTime = startTime + random.Next(6, 8);
                    break;
                case 2:
                    endTime = startTime + 8;
                    break;
                case 3:
                    endTime = startTime + random.Next(2, 4);
                    break;
                case 4:
                    endTime = startTime + random.Next(1, 3);
                    break;
                default:
                    break;
            }
            
            if (endTime >= 24) // Checks if the time the task ends is past the midnight clock
            {
                endTime -= 24;
            }

            completed = false;
        }


        public void update(Citizen citizen, Town town)
        {
            if (endTime == town.Time)
            {
                Debug.Log("Citizen " + citizen.ID + " has completed their task");
                completed = true;
                taskLocation.exitBuilding(citizen);
                
            }
        }
        
        #endregion
        
        #region getters & setters
        
        public bool Completed
        {
            get => completed;
            set => completed = value;
        }

        public bool Available => available;

        public string TaskName
        {
            get => taskName;
            
        }

        public int EndTime => endTime;


        #endregion
    }
}