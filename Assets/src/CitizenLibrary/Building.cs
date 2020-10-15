using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace src.CitizenLibrary
{
    public class Building
    {
        protected HashSet<Citizen> occupants; // Usually, the number of occupants will be initialized to zero
        protected string id;
        protected int numWithNoMask;
        protected Collection<Upgrade> availableUpgrades;
        protected Collection<Upgrade> buildingUpgrades;
        protected double happinessContribution, exposureFactor;
        protected int maxOccupants, numOccupants;
        protected SemaphoreSlim entranceLock, occupantAccessLock;
        protected bool open, containsInfected;
        protected Random random;

        public enum BuildingType
        {
            Recreational = 0, // Each building UI object will have a building
            Supermarket = 1,
            Emergency = 2,
            Residential = 3
        };

        private BuildingType buildingType;

        #region Constructors
        public Building(string id, double happinessContribution, double exposureFactor, int maxOccupants, int numOccupants, int buildingType)
        {
            this.id = id;
            this.happinessContribution = happinessContribution;
            this.exposureFactor = exposureFactor;
            this.maxOccupants = maxOccupants;
            this.numOccupants = numOccupants;
            setBuildingType(buildingType);
            entranceLock = new SemaphoreSlim(1);
            occupantAccessLock = new SemaphoreSlim(1);
            occupants = new HashSet<Citizen>();
            random = new Random();
        }
        
        #endregion
        public void Update()
        {
            if (containsInfected)
            {
                occupantAccessLock.Wait();
                int spreadDisease = random.Next(0, 100);
                if (spreadDisease <= exposureFactor) // If spread disease
                {
                    int infect = random.Next(occupants.Count-1);
                    occupants.ElementAt(infect).rollHealthEvent(100);
                }
                occupantAccessLock.Release();
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

        public string ID => id;
        #endregion
        
        #region Citizen Management Methods
        public virtual bool enterBuilding(Citizen citizen)
        {
            if (citizen.homeLocation.Equals(this))
            {
                occupantAccessLock.Wait();
                occupants.Add(citizen);
                occupantAccessLock.Release();
            }
            if (open)
            {
                entranceLock.Wait(); // Lock acquired to 
                if (numOccupants < maxOccupants) // Checks if the citizen can actually enter the building
                {
                    occupantAccessLock.Wait();
                    occupants.Add(citizen);
                    occupantAccessLock.Release();
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
                    occupantAccessLock.Wait();
                    occupants.Add(citizen);
                    occupantAccessLock.Release();
                    exposureFactor += 1;
                    entranceLock.Release();
                    return true;
                }
            }

            return false;
        }

        public virtual bool exitBuilding(Citizen citizen)
        {
            entranceLock.Wait();
            occupantAccessLock.Wait();
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
            return true;
        }

        #endregion
        
        #region Property Methods

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Building))
            {
                return false;
            }
            Building b = (Building) obj;
            return b.id.Equals(id);
        }

        public override int GetHashCode()
        {
            int hash = 11;
            int hashID = hash * 17 + ID.GetHashCode();
            return hashID;
        }

        #endregion
    }
}