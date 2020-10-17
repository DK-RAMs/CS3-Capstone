using System.Linq;
using src.CitizenLibrary;
using UnityEngine;

namespace src.SaveLoadLibrary
{
    [System.Serializable]
    public class SupermarketData : BuildingData
    {
        private string[] citizensInQueue;
        private int maxQueueSize;
        public SupermarketData(Building b) : base(b)
        {
            if (b is Supermarket s)
            {
                citizensInQueue = new string[s.QueueOutside.Count];
                for (int i = 0; i < s.QueueOutside.Count; i++)
                {
                    citizensInQueue[i] = s.QueueOutside.ElementAt(i).ID;
                }

                maxQueueSize = s.MaxQueueSize;
            }
            else
            {
                Debug.LogError("ERROR! The wrong building type was sent for object generation");
            }
        }

        public string[] CitizensInQueue => citizensInQueue;

        public int MaxQueueSize => maxQueueSize;
    }
}