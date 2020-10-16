using System;
using src.CitizenLibrary;

namespace src.SaveLoadLibrary
{
    [Serializable]
    public class CitizenData
    {
        private string id, name, currentTaskLoc, favoriteTaskLoc;
        private int age;
        private double happiness, riskofDeath, deltaHappiness;
        private bool rebel, hospitalized, dead, infected, wearingMask;
        private TaskData currentTaskData, favoriteTaskData;

        public CitizenData(Citizen c)
        {
            
        }
    }
}