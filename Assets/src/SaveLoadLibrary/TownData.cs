using src.CitizenLibrary;

namespace src.SaveLoadLibrary // Work on this
{
    [System.Serializable]
    public class TownData
    {
        private string id, mayor;
        private int day, time, totalInfected;
        private double money, baseDeltaHappiness, favoriteMaidifier;
        private long elapsedTime;
        private bool[] policyImplementations;
        private string[] buildingIDs;
        public TownData(Town t)
        {
            id = t.ID;
            mayor = t.Mayor;
            day = t.Day;
            time = t.Time;
            totalInfected = t.TotalInfected;
            money = t.Money;
            favoriteMaidifier = t.FavoriteModifier;
            baseDeltaHappiness = t.BaseDetalHappiness;
            elapsedTime = t.Timer.ElapsedMilliseconds;
            policyImplementations = new bool[t.PolicyImplementation.Count];
            for (int i = 0; i < policyImplementations.Length; i++)
            {
                policyImplementations[i] = t.PolicyImplementation[i];
            }

            buildingIDs = new string[t.Buildings.Count];
            int counter = 0;
            foreach (Building b in t.Buildings)
            {
                buildingIDs[counter] = b.ID;
                counter++;
            }
        }
        
        #region getters & setters

        public string ID => id;

        public string Mayor => mayor;

        public int Day => day;

        public int Time => time;

        public int TotalInfected => totalInfected;

        public double Money => money;

        public double BaseDeltaHappiness => baseDeltaHappiness;

        public double FavoriteMaidifier => favoriteMaidifier;

        public long ElapsedTime => elapsedTime;

        public bool[] PolicyImplementations => policyImplementations;

        public string[] BuildingIDs => buildingIDs;
        

        #endregion
    }
}