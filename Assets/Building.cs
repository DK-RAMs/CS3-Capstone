using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace CitizenLibrary
{
    public class Building
    {
        protected HashSet<Citizen> occupants; // Usually, the number of occupants will be initialized to zero
        protected string id;
        protected Collection<Upgrade> availableUpgrades;
        protected Collection<Upgrade> buildingUpgrades;
        protected double happinessContribution, exposureFactor;
        protected int x, y;
        protected int maxOccupants, numOccupants;
        protected static SemaphoreSlim entranceLock;
        protected static SemaphoreSlim exitLock;
        protected bool open;

        public enum BuildingType
        {
            Recreational = 0,
            Supermarket = 1,
            Emergency = 2
        };

        private BuildingType buildingType;


        public Building(string id, double happinessContribution, double exposureFactor, int maxOccupants, int numOccupants, int buildingType)
        {
            this.id = id;
            this.happinessContribution = happinessContribution;
            this.exposureFactor = exposureFactor;
            this.maxOccupants = maxOccupants;
            this.numOccupants = numOccupants;
            setBuildingType(buildingType);
            entranceLock = new SemaphoreSlim(1);
            occupants = new HashSet<Citizen>();
        }

        public void Update()
        {
            
        }
        
        #region getters & setters
        private void setBuildingType(int buildingType)
        {
            switch (buildingType)
            {
                case 0:
                    this.buildingType = BuildingType.Recreational;
                    break;
                case 1:
                    this.buildingType = BuildingType.Supermarket;
                    break;
                case 2:
                    this.buildingType = BuildingType.Emergency;
                    break;
            }
        }

        public BuildingType getBuildingType
        {
            get => buildingType;
        }

        public virtual bool enterBuilding(Citizen citizen)
        {
            entranceLock.Wait(); // Lock acquired to 
            if (numOccupants < maxOccupants) // Checks if the citizen can actually enter the building
            {
                occupants.Add(citizen);
                entranceLock.Release();
                return true;
            }
            if (citizen.Rebel && numOccupants >= maxOccupants)
            {
                occupants.Add(citizen);
                exposureFactor += 1;
                entranceLock.Release();
                return true;
            }
            return false;
        }

        public virtual void exitBuilding(Citizen citizen)
        {
            entranceLock.Wait();
            occupants.Remove(citizen);
            if (numOccupants > maxOccupants)
            {
                exposureFactor-=1;
            }
            numOccupants--;
            entranceLock.Release();
        }

        #endregion
    }
}