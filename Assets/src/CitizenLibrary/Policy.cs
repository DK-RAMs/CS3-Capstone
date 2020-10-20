using UnityEngine;

namespace src.CitizenLibrary
{
    public class Policy : ScriptableObject // Still need to do (making these scriptable makes it easier to make more of them
    {
        public int policyID;
        public string policyDescription;
        public double deltaHappiness, exposureRiskAdjustment;
        public enum PolicyType {Individual, Community}

        public PolicyType policyType;

    }
}