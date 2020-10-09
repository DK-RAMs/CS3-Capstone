using System;

namespace CitizenLibrary
{
    public class Upgrade
    {
        private string upgradeID;
        private string description;
        private double riskFactorAdjustment, deltaHappiness;
        private long cost;

        public Upgrade(string upgradeID, string description, double riskFactorAdjustment, double deltaHappiness, long cost)
        {
            this.upgradeID = upgradeID;
            this.description = description;
            this.riskFactorAdjustment = riskFactorAdjustment;
            this.deltaHappiness = deltaHappiness;
            this.cost = cost;
        }
        
        #region getters & setters

        public string UpgradeID => upgradeID;
        public string Description => description;

        public double RiskFactorAdjustment => riskFactorAdjustment;

        public double DeltaHappiness => deltaHappiness;

        public long Cost => cost;

        #endregion
    }
}