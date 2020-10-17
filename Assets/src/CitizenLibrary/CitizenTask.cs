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
        public int TaskID { get; private set; }
        public static Dictionary<int, (string, bool)> taskKeys = new Dictionary<int, (string, bool)>(); // 2nd Key in dictionary is the availability of the task
        bool completed;
        private bool firstsuccess;
        int startTime, endTime, startDay, endDay;
        public Building taskLocation;
        
        #region Constructors
        public CitizenTask(int taskID, int startTime, int endTime, int startDay, int endDay, int completed, Building taskLocation)
        {
            (string, bool) task;
            if (!taskKeys.TryGetValue(taskID, out task))
            {
                Console.WriteLine("ERROR! Task #" + taskID + " is not defined");
            }
            this.startDay = startDay;
            this.endDay = endDay;
            taskName = taskKeys[taskID].Item1;
            this.startTime = startTime;
            this.endTime = endTime;
            if (completed == 0)
            {
                this.completed = false;
            }
            else{
                this.completed = true;
            }

            this.taskLocation = taskLocation;
        }

        public CitizenTask(Random random, Citizen citizen, bool favorite)
        {
            if (favorite)
            {
                generateFavoriteTask(random, citizen);
            }
            else
            {
                generateNewTask(random, citizen);
            }
        }
        #endregion
        
        #region Task Selection methods
        public double calculateTaskHappiness(Random random, bool rebel, double modifier) // Happiness Modifier is applied in the case where a citizen completes their "favorite task"
        {
            if (firstsuccess)
            {
                if (TaskID == 4)
                {
                    return 1;
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
            TaskID = 4;
            taskName = taskKeys[TaskID].Item1;
            endTime = startTime + random.Next(1, 3);
            endDay = startDay + (endTime / 24);
            taskLocation = citizen.homeLocation;
        }

        private void generateNewTask(Random random, Citizen citizen)
        {
            startTime = Game.town.Time;
            startDay = Game.town.Day;
            if (citizen.Infected && !citizen.Rebel) // Checks if an infected citizen is a rebel. If not, they will self quarantine
            {
                if (random.Next(1, 100) <= 50 || Game.town.PolicyImplementation[0]) // PolicyImplementation[1] - All citizens that are infected must self-quarantine
                {
                    Debug.Log("Citizen contracted COVID and they must self quarantine");
                    TaskID = 5;
                    taskName = taskKeys[TaskID].Item1;
                    endTime = startTime + 2;
                    endDay = startDay + random.Next(12, 14);
                    taskLocation = citizen.homeLocation;
                    return;
                }
            }
            TaskID = random.Next(0, taskKeys.Count-3);
            if (!taskKeys[TaskID].Item2) // Checks if task with specified key is available
            {
                generateUnhappyTask(random, citizen);
                return;
            }
            firstsuccess = true;
            taskName = taskKeys[TaskID].Item1;
            switch (TaskID)
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
                    break;
                case 4:
                    generateHomeTask(random, citizen);
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
            taskLocation = citizen.homeLocation;
        }
        
        private void generateRecreationalTask(Random random, Citizen citizen)
        {
            Collection<Building> spots = Game.town.Recreational;
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
            if (citizen.workLocation.Equals(citizen.homeLocation))
            {
                citizen.homeLocation.enterBuilding(citizen);
                taskLocation = citizen.homeLocation;
                return;
            }
            if (citizen.workLocation.enterBuilding(citizen))
            {
                taskLocation = citizen.workLocation;
                return;
            }
            generateUnhappyTask(random, citizen);
        }

        private void generateShoppingTask(Random random, Citizen citizen)
        {
            Collection<Supermarket> spots = Game.town.Essentials;
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
            taskLocation = citizen.homeLocation;
        }

        public void generateFavoriteTask(Random random, Citizen citizen)
        {
            TaskID = random.Next(0, taskKeys.Count - 3);
            switch (TaskID)
            {
                case 0:
                    taskLocation = Game.town.Recreational[random.Next(Game.town.Recreational.Count - 1)]; // It's necessary to directly access this value (using a getter will return a value and not the reference to the value)
                    break;
                case 1:
                    taskLocation = citizen.homeLocation;
                    break;
                case 2:
                    taskLocation = citizen.workLocation;
                    break;
                case 3:
                    taskLocation = Game.town.Essentials[random.Next(Game.town.Essentials.Count - 1)];
                    break;
                case 4:
                    taskLocation = Game.town.Residential[random.Next(Game.town.Residential.Count - 1)];
                    break;
                case 5:
                    taskLocation = citizen.homeLocation;
                    break;
                default:
                    TaskID = 1;
                    taskLocation = citizen.homeLocation;
                    break;
            }
        }
        
        private void admitToHospital(Random random, Citizen citizen)
        {
            Collection<Hospital> emergencyRooms = Game.town.Emergency;
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

            startTime = Game.town.Time;
            startDay = Game.town.Day;
            endTime = startTime;
            endDay = startDay + (endTime / 24) + random.Next(14, 21);
            if (citizen.RiskofDeath > 35)
            {
                endDay = startDay + endTime / 24 + random.Next(14, 42);
            }
            endTime %= 24;
        }
        #endregion
        
        #region Task Update Methods
        public void Update(Random random, Citizen citizen, Town town) // This method isn't called when the citizen is hospitalized.
        {
            if (town.Time >= endTime && town.Day >= endDay)
            {
                completed = true;
                if (TaskID == 6)
                {
                    ((Hospital)taskLocation).checkOutPatient(citizen);
                }
                else
                {
                    taskLocation.exitBuilding(citizen);
                }
            }
            else if (citizen.Hospitalized)
            {
                admitToHospital(random, citizen);
            }
        }
        
        #endregion
        
        #region getters & setters
        
        public bool Completed
        {
            get => completed;
        }

        public string TaskName => taskName;

        public int StartTime => startTime;

        public int StartDay => startDay;

        public int EndTime => endTime;

        public int EndDay => endDay;

        public bool FirstSuccess => firstsuccess;

        #endregion
        
        #region Property Methods

        public override bool Equals(object obj)
        {
            if (obj == null || !typeof(CitizenTask).IsInstanceOfType(obj))
            {
                return false;
            }

            CitizenTask t = (CitizenTask) obj;
            return TaskID.Equals(t.TaskID) && taskLocation.Equals(t.taskLocation);
        }

        #endregion
    }
}