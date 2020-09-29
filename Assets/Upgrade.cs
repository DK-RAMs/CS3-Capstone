namespace CitizenLibrary
{
    public class Upgrade
    {
        private string description;
        private double riskFactorAdjustment, deltaHappiness;
        private long cost;

        public Upgrade(string description, double riskFactorAdjustment, double deltaHappiness, long cost)
        {
            this.description = description;
            this.riskFactorAdjustment = riskFactorAdjustment;
            this.deltaHappiness = deltaHappiness;
            this.cost = cost;
        }
        
        
    }
}