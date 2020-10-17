using System;
using UnityEngine;

namespace src.CitizenLibrary
{
    public class Upgrade : ScriptableObject
    {
        public string upgradeID;
        public string description;
        public double riskFactorAdjustment, deltaHappiness;
        public long cost;

        public Upgrade(string upgradeID, string description, double riskFactorAdjustment, double deltaHappiness, long cost)
        {
            this.upgradeID = upgradeID;
            this.description = description;
            this.riskFactorAdjustment = riskFactorAdjustment;
            this.deltaHappiness = deltaHappiness;
            this.cost = cost;
        }
        
    }
}