using src.CitizenLibrary;
using UnityEngine;

namespace src.SaveLoadLibrary
{
    [System.Serializable]
    public class HospitalData : BuildingData
    {
        private string[] citizensInBedID;
        private int numBeds;
        private bool overloaded;
        public HospitalData(Building b) : base(b)
        {
            if (b is Hospital h)
            {
                citizensInBedID = new string[h.CitizensInBed.Count];
                for (int i = 0; i < h.CitizensInBed.Count; i++)
                {
                    citizensInBedID[i] = h.CitizensInBed[i].ID;
                }
                numBeds = h.NumBeds;
                overloaded = h.Overloaded;
                
            }
            else
            {
                Debug.LogError("ERROR! Incorrect type was passed into hospital data serializer");
            }
            
        }

        public string[] CitizensInBedId => citizensInBedID;

        public int NumBeds => numBeds;

        public bool Overloaded => overloaded;
    }
}