using System.Linq;
using src.CitizenLibrary;

namespace src.SaveLoadLibrary
{
    [System.Serializable]
    public class BuildingData
    {
        protected string id;
        protected string[] availableUpgradeIDs, buildingUpgradeIDs;
        protected int maxOccupants;
        protected double exposureFactor;
        protected bool open;
        protected static long elapsedTime;
        protected int buildingType;

        public BuildingData(Building b)
        {
            availableUpgradeIDs = new string[b.AvailableUpgrades.Count];
            for (int i = 0; i < b.AvailableUpgrades.Count; i++)
            {
                availableUpgradeIDs[i] = b.AvailableUpgrades[i].upgradeID;
            }
            buildingUpgradeIDs = new string[b.BuildingUpgrades.Count];
            for (int i = 0; i < b.BuildingUpgrades.Count; i++)
            {
                buildingUpgradeIDs[i] = b.BuildingUpgrades[i].upgradeID;
            }
            open = b.Open;
            elapsedTime = Building.buildingTimer.ElapsedMilliseconds;
        }
        
        #region getters & setters

        public string ID => id;

        public int MaxOccupants => maxOccupants;

        public int BuildingType => buildingType;

        public double ExposureFactor => exposureFactor;

        public string[] AvailableUpgradeIDs => availableUpgradeIDs;

        public string[] BuildingUpgradeIDs => buildingUpgradeIDs;

        public bool Open => open;

        public long ElapsedTime => elapsedTime;

        #endregion
    }
}