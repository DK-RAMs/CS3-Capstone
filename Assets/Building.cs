using System.Collections.ObjectModel;

namespace CitizenLibrary
{
    public class Building
    {
        protected Collection<Citizen> occupants; // Usually, 
        protected string id;
        protected Collection<Upgrade> upgrades;
        protected double happinessContribution, exposureFactor;
        protected int x, y;
        protected int maxOccupants;
        
        
        public Building(string id)
        {
            
        }
    }
}