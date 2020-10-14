using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using UnityEngine;
using UnityEngine.Analytics;
using Random = System.Random;

namespace src.CitizenLibrary
{
    public class CitizenTask
    {
        string taskName;
        private static double MAXBASEHAPPINESSGAIN = 2;
        private double happinessModifier;
        private int taskID;
        public static Dictionary<int, (string, bool)> taskKeys = new Dictionary<int, (string, bool)>(); // 2nd Key in dictionary is the availability of the task
        bool completed;
        private bool available, firstsuccess;
        int startTime, endTime, startDay, endDay;
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

        public CitizenTask(Random random, Citizen citizen)
        {
            generateNewTask(random, citizen);
        }
        #endregion
        
        #region Task Selection methods
        public double calculateTaskHappiness(Random random, bool rebel, double modifier) // Happiness Modifier is applied in the case where a citizen completes their "favorite task"
        {
            if (firstsuccess)
            {
                if (taskID == 4)
                {
                    return 0.75;
                }
                double rebelFactor = 1;
                if (rebel)
                {
                    rebelFactor = 1.15;
                }

                return 1 + random.NextDouble() * MAXBASEHAPPINESSGAIN * rebelFactor * modifier;
            }
            return 1 - random.NextDouble() * MAXBASEHAPPINESSGAIN * modifier;
        }

        private void generateUnhappyTask(Random random, Citizen citizen) // This is done technically
        {
            firstsuccess = false;
            taskID = 4;
            taskName = taskKeys[taskID].Item1;
            endTime = startTime + random.Next(1, 3);
            endDay = startDay + (endTime / 24);
            taskLocation = citizen.citizenHome;
        }

        private void generateNewTask(Random random, Citizen citizen)
        {
            startTime = Citizen.Town.Time;
            startDay = Citizen.Town.Day;
            if (citizen.Infected && !citizen.Rebel) // Checks if an infected citizen is a rebel. If not, they will self quarantine
            {
                if (random.Next(1, 100) <= 50 || Citizen.town.PolicyImplementation[0]) // PolicyImplementation[1] - All citizens that are infected must self-quarantine
                {
                    Debug.Log("Citizen contracted COVID and they must self quarantine");
                    taskID = 5;
                    taskName = taskKeys[taskID].Item1;
                    endTime = startTime + 2;
                    endDay = startDay + random.Next(12, 14);
                    taskLocation = citizen.citizenHome;
                    return;
                }
            }
            taskID = random.Next(0, taskKeys.Count-3);
            if (!taskKeys[taskID].Item2) // Checks if task with specified key is available
            {
                Debug.Log("Citizen tried to " + taskKeys[taskID].Item1 + " but can't. They decided to stay home, feeling upset");
                generateUnhappyTask(random, citizen);
                return;
            }
            firstsuccess = true;
            taskName = taskKeys[taskID].Item1;
            switch (taskID)
            {
                case 0: // Citizen Decided to go drinking at the bar/eat out at a restaurant
                    generateRecreationalTask(random, citizen);
                    break;
                case 1: 
                    generateSleepTask(random, citizen);
                    break;
                case 2:
                    generateWorkTask(random, citizen);
                    break;
                case 3:
                    generateShoppingTask(random, citizen);
                    endTime = startTime + random.Next(2, 4);
                    break;
                case 4:
                    generateHomeTask(random, citizen);
                    endTime = startTime + random.Next(1, 3);
                    break;
                default:
                    break;
            }

            int numDays = endTime / 24;
            
            endTime %= 24;

            endDay = startDay + numDays;

            completed = false;
        }
        #endregion
        
        #region Task Generators
        private void generateSleepTask(Random random, Citizen citizen)
        {
            endTime = startTime + random.Next(6, 8);
            taskLocation = citizen.citizenHome;
        }
        
        private void generateRecreationalTask(Random random, Citizen citizen)
        {
            Collection<Building> spots = Citizen.Town.Recreational;
            int numSpots = spots.Count;
            int startPos = random.Next(0, spots.Count-1);
            
            for (int i = 0; i < numSpots; i++)
            {
                int pos = (startPos + i) % numSpots;
                if (spots[pos].enterBuilding(citizen))
                {
                    taskLocation = spots[pos];
                    endTime = startTime + random.Next(1, 3); // Task will take between 1 and 3 hours
                    endDay = startDay + endTime / 24; // All services are open 24/7, I can add a
                    endTime %= 24;
                    return;
                }
            }
            generateUnhappyTask(random, citizen);
        }

        private void generateWorkTask(Random random, Citizen citizen)
        {
            endTime = startTime + 8;
            endDay = startDay + endTime / 24;
            endTime %= 24;
            if (citizen.workLocation.Equals(citizen.citizenHome))
            {
                citizen.citizenHome.enterBuilding(citizen);
                taskLocation = citizen.citizenHome;
                return;
            }
            if (citizen.workLocation.enterBuilding(citizen))
            {
                taskLocation = citizen.workLocation;
                return;
            }
            Debug.Log("Citizen was not able to go to work. They stayed home feeling upset");
            generateUnhappyTask(random, citizen);
        }

        private void generateShoppingTask(Random random, Citizen citizen)
        {
            Collection<Supermarket> spots = Citizen.Town.Essentials;
            int numSpots = spots.Count;
            int startPos = random.Next(0, spots.Count-1);
            for (int i = 0; i < numSpots; i++)
            {
                int pos = (startPos + i) % numSpots;
                if (spots[pos].enterBuilding(citizen))
                {
                    taskLocation = spots[pos];
                    endTime = startTime + random.Next(2, 4);
                    endDay = startDay + endTime / 24;
                    endTime %= 24;
                    return;
                }
            }
            endTime = startTime + random.Next(1, 4);
        }

        private void generateHomeTask(Random random, Citizen citizen)
        {
            endTime = startTime + random.Next(1, 3);
            endDay = startDay + endTime / 24;
            endTime %= 24;
            taskLocation = citizen.citizenHome;
        }

        private void admitToHospital(Random random, Citizen citizen)
        {
            Collection<Hospital> emergencyRooms = Citizen.Town.Emergency;
            int spot = random.Next(0, emergencyRooms.Count - 1);
            for (int i = 0; i < emergencyRooms.Count; i++)
            {
                int pos = spot + i % emergencyRooms.Count;
                if (!emergencyRooms[pos].Overloaded)
                {
                    emergencyRooms[pos].checkinPatient(citizen);
                    break;
                }
            }

            startTime = Citizen.Town.Time;
            startDay = Citizen.Town.Day;
            endTime = startTime;
            endDay = startDay + endTime / 24 + random.Next(14, 21);
            if (citizen.RiskofDeath > 35)
            {
                endDay = startDay + endTime / 24 + random.Next(21, 42);
            }
            endTime %= 24;
        }
        #endregion
        
        #region Task Update Methods
        public void update(Random random, Citizen citizen, Town town) // This method isn't called when the citizen is hospitalized.
        {
            if (endTime >= town.Time && endDay >= town.Day)
            {
                Debug.Log("Citizen " + citizen.ID + " has completed their task");
                completed = true;
                taskLocation.exitBuilding(citizen);
            }
            else if (citizen.Hospitalized)
            {
                Debug.Log("Citizen collapsed and was taken to the hospital");
                admitToHospital(random, citizen);
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

        public int EndDay => endDay;

        #endregion
    }
}