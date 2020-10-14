namespace src.CitizenLibrary
{
    public class Policy
    {
        private int policyID;
        private string policyDescription;
        private double deltaHappiness, deltaHealth, exposureRiskAdjustment;

        public Policy(int policyID, string policyDescription, double deltaHappiness, double deltaHealth,
            double exposureRiskAdjustment)
        {
            this.policyID = policyID;
            this.policyDescription = policyDescription;
            this.deltaHappiness = deltaHappiness;
            this.deltaHealth = deltaHealth;
            this.exposureRiskAdjustment = exposureRiskAdjustment;
        }

        public int PolicyId
        {
            get => policyID;
        }

        public string PolicyDescription
        {
            get => policyDescription;
        }

        public double DeltaHappiness
        {
            get => deltaHappiness;
        }

        public double DeltaHealth
        {
            get => deltaHealth;
        }

        public double ExposureRiskAdjustment
        {
            get => exposureRiskAdjustment;
        }
    }
}