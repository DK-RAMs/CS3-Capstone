using src.CitizenLibrary;
using UnityEngine;

namespace src.SaveLoadLibrary
{
    [System.Serializable]
    public class HospitalData : BuildingData
    {
        private int numBeds;
        private bool overloaded;
        public HospitalData(Building b) : base(b)
        {
            if (b is Hospital h)
            {
                numBeds = h.NumBeds;
                overloaded = h.Overloaded;
            }
            else
            {
                Debug.LogError("ERROR! Incorrect type was passed into hospital data serializer");
            }
        }
        
        public int NumBeds => numBeds;

        public bool Overloaded => overloaded;
    }
}