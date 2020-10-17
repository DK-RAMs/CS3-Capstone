using System;
using src.CitizenLibrary;

namespace src.SaveLoadLibrary
{
    [Serializable]
    public class CitizenData
    {
        private string id, name, currentTaskLoc, favoriteTaskLoc, homeLocation, workLocation;
        private int age, endTime, endDay, currentTaskID;
        private double happiness, riskofDeath;
        private bool rebel, hospitalized, dead, infected, wearingMask, taskCompleted, firstsuccess;

        public CitizenData(Citizen c)
        {
            id = c.ID;
            name = c.Name;
            currentTaskLoc = c.CurrentTask.taskLocation.ID;
            favoriteTaskLoc = c.FavoriteTask.taskLocation.ID;
            homeLocation = c.homeLocation.ID;
            workLocation = c.workLocation.ID;
            age = c.Age;
            endTime = c.CurrentTask.EndTime;
            endDay = c.CurrentTask.EndDay;
            currentTaskID = c.CurrentTask.TaskID;
            happiness = c.Happiness;
            riskofDeath = c.RiskofDeath;
            rebel = c.Rebel;
            hospitalized = c.Hospitalized;
            dead = c.Dead;
            infected = c.Infected;
            wearingMask = c.WearingMask;
            taskCompleted = c.CurrentTask.Completed;
            firstsuccess = c.CurrentTask.FirstSuccess;
        }

        public string ID => id;
        
        public string Name => name;

        public string CurrentTaskLoc => currentTaskLoc;

        public string FavoriteTaskLoc => favoriteTaskLoc;

        public string HomeLocation => homeLocation;

        public string WorkLocation => workLocation;

        public int Age => age;

        public int EndTime => endTime;

        public int EndDay => endDay;


        public double Happiness => happiness;

        public double RiskofDeath => riskofDeath;


        public bool Rebel => rebel;

        public bool Hospitalized => hospitalized;

        public int CurrentTaskId => currentTaskID;

        public bool Dead => dead;

        public bool Infected => infected;

        public bool WearingMask => wearingMask;

        public bool TaskCompleted => taskCompleted;

        public bool Firstsuccess => firstsuccess;
    }
}