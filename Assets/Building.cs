using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace CitizenLibrary
{
    public class Building
    {
        protected HashSet<Citizen> occupants; // Usually, the number of occupants will be initialized to zero
        protected string id;
        protected int numWithNoMask;
        protected Collection<Upgrade> availableUpgrades;
        protected Collection<Upgrade> buildingUpgrades;
        protected double happinessContribution, exposureFactor;
        protected int x, y;
        protected int maxOccupants, numOccupants;
        protected SemaphoreSlim entranceLock;
        protected bool open, containsInfected;
        protected Random random;

        public enum BuildingType
        {
            Recreational = 0,
            Supermarket = 1,
            Emergency = 2,
            Residential = 3
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
            random = new Random();
        }

        public void Update()
        {
            if (containsInfected)
            {
                int spreadDisease = random.Next(0, 100);
                if (spreadDisease <= exposureFactor)
                {
                    
                }
            }
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
                case 3:
                    this.buildingType = BuildingType.Residential;
                    break;
            }
        }

        public BuildingType getBuildingType
        {
            get => buildingType;
        }
        #endregion
        
        #region Citizen Management Methods
        public virtual bool enterBuilding(Citizen citizen)
        {
            if (open)
            {
                entranceLock.Wait(); // Lock acquired to 
                if (numOccupants < maxOccupants) // Checks if the citizen can actually enter the building
                {
                    occupants.Add(citizen);
                    if (!citizen.WearingMask)
                    {
                        numWithNoMask++;
                        exposureFactor += 1;
                    }
                    
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
            }

            return false;
        }

        public virtual void exitBuilding(Citizen citizen)
        {
            entranceLock.Wait();
            occupants.Remove(citizen);
            if (numOccupants > maxOccupants)
            {
                if (!citizen.WearingMask)
                {
                    exposureFactor -= 1;
                    numWithNoMask--;
                }
            }
            numOccupants--;
            entranceLock.Release();
        }

        #endregion
        
        #region Property Methods

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if(typeof(Building).IsInstanceOfType(obj))
            {
                Building b = (Building) obj;
                return b.id.Equals(this.id);
            }
            return false;
        }

        #endregion
    }
}