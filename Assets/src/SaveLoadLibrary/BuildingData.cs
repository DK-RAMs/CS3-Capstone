using System.Linq;
using src.CitizenLibrary;

namespace src.SaveLoadLibrary
{
    [System.Serializable]
    public class BuildingData
    {
        protected string[] availableUpgradeIDs;
        protected string[] buildingUpgradeIDs;
        protected bool open;
        protected long elapsedTime;

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
            elapsedTime = b.ElapsedTime;
        }
        
        #region getters & setters

        public string[] AvailableUpgradeIDs => availableUpgradeIDs;

        public string[] BuildingUpgradeIDs => buildingUpgradeIDs;

        public bool Open => open;

        public long ElapsedTime => elapsedTime;

        #endregion
    }
}