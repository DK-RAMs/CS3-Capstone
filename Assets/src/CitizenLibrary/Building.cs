using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using src.SaveLoadLibrary;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace src.CitizenLibrary
{
    public class Building // Need to polish up
    {
        protected HashSet<Citizen> occupants; // Usually, the number of occupants will be initialized to zero
        protected string id;
        protected int numWithNoMask, numInfected, maxOccupants, numOccupants;
        protected Collection<Upgrade> availableUpgrades;
        protected Collection<Upgrade> buildingUpgrades;
        public static Collection<Upgrade> upgrades = new Collection<Upgrade>();
        protected double exposureFactor;
        protected SemaphoreSlim entranceLock, occupantAccessLock;
        protected bool open, containsInfected;
        protected static Random random = new Random();
        public static Stopwatch buildingTimer = new Stopwatch();
        public static long buildingUpdateTimer = Game.UPDATETICKRATE / 4;

        public enum BuildingType
        {
            Recreational = 0, // Each building UI object will have a building
            Essential = 1,
            Emergency = 2,
            Residential = 3
        };

        private BuildingType buildingType;

        #region Constructors
        public Building(string id, double exposureFactor, int maxOccupants, int numOccupants, int buildingType)
        {
            this.id = id;
            this.exposureFactor = exposureFactor;
            this.maxOccupants = maxOccupants;
            this.numOccupants = numOccupants;
            setBuildingType(buildingType);
            entranceLock = new SemaphoreSlim(1);
            occupantAccessLock = new SemaphoreSlim(1);
            occupants = new HashSet<Citizen>();
            numInfected = 0;
        }

        public Building(BuildingData b)
        {
            id = b.ID;
            maxOccupants = b.MaxOccupants;
            exposureFactor = b.ExposureFactor;
            open = b.Open;
            entranceLock = new SemaphoreSlim(1);
            occupantAccessLock = new SemaphoreSlim(1);
            occupants = new HashSet<Citizen>();
            buildingTimer.Start();
            while (buildingTimer.ElapsedMilliseconds < b.ElapsedTime)  continue;
            buildingTimer.Stop();
            switch (b.BuildingType)
            {
                case 0:
                    buildingType = BuildingType.Recreational;
                    break;
                case 1:
                    buildingType = BuildingType.Essential;
                    break;
                case 2:
                    buildingType = BuildingType.Emergency;
                    break;
                case 3:
                    buildingType = BuildingType.Residential;
                    break;
            }
            
            // Loads building upgrades from some place
            for (int i = 0; i < b.AvailableUpgradeIDs.Length; i++)
            {
                foreach (Upgrade u in upgrades)
                {
                    if (u.upgradeID.Equals(b.AvailableUpgradeIDs[i]))
                    {
                        availableUpgrades.Add(u);
                    }
                }
            }
            for (int i = 0; i < b.BuildingUpgradeIDs.Length; i++)
            {
                foreach (Upgrade u in upgrades)
                {
                    if (u.upgradeID.Equals(b.BuildingUpgradeIDs[i]))
                    {
                        buildingUpgrades.Add(u);
                    }
                }
            }
        }
        
        #endregion
        
        #region Building Update Methods
        public void Update()
        {
            Debug.Log("Updating " +ID);
            if (containsInfected)
            {
                occupantAccessLock.Wait();
                int spreadDisease = random.Next(0, 100);
                if (spreadDisease <= exposureFactor) // If spread check succeeds, a new citizen is infected
                {
                    int infect = random.Next(occupants.Count-1);
                    if (!occupants.ElementAt(infect).Infected)
                    {
                        numInfected++;
                        occupants.ElementAt(infect).rollHealthEvent(100);
                    }
                }
                occupantAccessLock.Release();
            }
        }
        
        #endregion
        
        #region getters & setters
        private void setBuildingType(int buildingType)
        {
            switch (buildingType)
            {
                case 0:
                    this.buildingType = BuildingType.Recreational;
                    break;
                case 1:
                    this.buildingType = BuildingType.Essential;
                    break;
                case 2:
                    this.buildingType = BuildingType.Emergency;
                    break;
                case 3:
                    this.buildingType = BuildingType.Residential;
                    break;
            }
        }

        public BuildingType getBuildingType => buildingType;

        public HashSet<Citizen> Occupants => occupants;

        public Collection<Upgrade> AvailableUpgrades => availableUpgrades;

        public Collection<Upgrade> BuildingUpgrades => buildingUpgrades;

        public bool Open => open;

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
                numOccupants++;
                if (citizen.Infected)
                {
                    Debug.Log("Building contains infected individual");
                    numInfected++;
                    containsInfected = true;
                }
            }
            else if (buildingType == BuildingType.Residential)
            {
                occupantAccessLock.Wait();
                occupants.Add(citizen);
                occupantAccessLock.Release();
                numOccupants++;
                if (citizen.Infected)
                {
                    Debug.Log("Building contains infected individual");
                    numInfected++;
                    containsInfected = true;
                    exposureFactor += 5;
                }
            }
            else if (open) // Building being entered isn't a residence
            {
                entranceLock.Wait(); // Lock acquired to 
                if (numOccupants < maxOccupants) // Checks if the citizen can actually enter the building
                {
                    occupantAccessLock.Wait();
                    occupants.Add(citizen);
                    occupantAccessLock.Release();
                    numOccupants++;
                    if (!citizen.WearingMask)
                    {
                        numWithNoMask++;
                        exposureFactor += 1;
                    }
                    
                    entranceLock.Release();
                    return true;
                }
                if (citizen.Infected)
                {
                    Debug.Log("Building contains infected individual");
                    numInfected++;
                    containsInfected = true;
                }

                if (citizen.Rebel && numOccupants >= maxOccupants)
                {
                    occupantAccessLock.Wait();
                    occupants.Add(citizen);
                    occupantAccessLock.Release();
                    numOccupants++;
                    exposureFactor += 1;
                    if (!citizen.WearingMask)
                    {
                        numWithNoMask++;
                        exposureFactor += 1;
                    }
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
            if (numOccupants >= maxOccupants)
            {
                exposureFactor--;
            }
            if (!citizen.WearingMask)
            {
                exposureFactor -= 1;
                numWithNoMask--;
            }
            if (citizen.Infected)
            {
                numInfected--;
                Debug.Log("Infected citizen left the building");
                if (numInfected <= 0)
                {
                    containsInfected = false;
                    numInfected = 0;
                }
            }
            numOccupants--;
            occupantAccessLock.Release();
            entranceLock.Release();
            return true;
        }

        public void applyUpgrade(string upgradeID)
        {
            int pos = -1;
            for (int i = 0; i < availableUpgrades.Count; i++)
            {
                if (availableUpgrades[i].upgradeID.Equals(upgradeID))
                {
                    pos = i;
                    buildingUpgrades.Add(availableUpgrades[i]);
                }
            }

            if (pos >= 0)
            {
                availableUpgrades.RemoveAt(pos);
            }
            else
            {
                Debug.Log("The upgrade selected can't be applied to this building");
            }
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