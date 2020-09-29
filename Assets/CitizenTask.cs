using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using CitizenLibrary;
using UnityEngine;
using Random = System.Random;

namespace CitizenLibrary
{
    public class CitizenTask
    {
        string taskName;
        private Random random;
        private static double MAXBASEHAPPINESSGAIN;
        private int taskID;
        public static Dictionary<int, string> taskKeys = new Dictionary<int, string>();
        bool completed;
        private bool available;
        int startTime, endTime;

        public CitizenTask(Random random, Citizen.Occupation occupation)
        {
            switch (occupation)
            {
                case Citizen.Occupation.Employed:
                    break;
                case Citizen.Occupation.Retired:
                    break;
                case Citizen.Occupation.Student:
                    break;
                case Citizen.Occupation.Unemployed:
                    break;
                default:
                    throw new DataException();
            }
            this.random = random; // Passes citizen's random object to this one. This should reduce the amount of memory that the program uses
            int initialTask = random.Next(0, taskKeys.Count-1);
        }
        
        public double calculateTaskHappiness(double happinessModifier) // Happiness Modifier is applied in the case where a citizen completes their "favorite task"
        {
            return random.NextDouble()*MAXBASEHAPPINESSGAIN*happinessModifier;
        }
        public CitizenTask(int taskID, int startTime, int endTime, int completed)
        {
            string task;
            if (!taskKeys.TryGetValue(taskID, out task))
            {
                Console.WriteLine("ERROR! Task #" + taskID + " is not defined");
            }
            this.taskName = task;
            
        }

        public void update(Citizen citizen, Town town)
        {
            if (Town.Time == endTime)
            {
                completed = true;
            }
        }
        
        #region getters & setters
        
        public bool Completed
        {
            get => completed;
            set => completed = value;
        }

        public bool Available
        {
            get => available;
        }

        #endregion
    }
}