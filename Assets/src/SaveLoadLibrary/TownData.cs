using src.CitizenLibrary;

namespace src.SaveLoadLibrary
{
    [System.Serializable]
    public class TownData
    {
        private string id, mayor;
        private int day, time, totalInfected;
        private double money, deltaMoney, baseDeltaHappiness, favoriteMaidifier;
        private long elapsedTime;
        private bool[] policyImplementations;
        public TownData(Town t)
        {
            id = t.ID;
            mayor = t.Mayor;
            day = t.Day;
            time = t.Time;
            totalInfected = t.TotalInfected;
            money = t.Money;
            deltaMoney = t.DeltaMoney;
            favoriteMaidifier = t.FavoriteModifier;
            baseDeltaHappiness = t.BaseDetalHappiness;
            elapsedTime = t.Timer.ElapsedMilliseconds;
            policyImplementations = new bool[t.PolicyImplementation.Count];
            for (int i = 0; i < policyImplementations.Length; i++)
            {
                policyImplementations[i] = t.PolicyImplementation[i];
            }
        }
    }
}