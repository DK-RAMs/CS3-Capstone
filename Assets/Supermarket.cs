using System.Collections.ObjectModel;
using System.Threading;

namespace CitizenLibrary
{
    public class Supermarket : Building
    {
        private int maxQueueSize;
        private Collection<Citizen> queueOutside; // Use different dataStructure here
        public Supermarket(string id, double happinessContribution, double exposureFactor, int maxOccupants, int numOccupants, int maxQueueSize) : base(id, happinessContribution, exposureFactor, maxOccupants, numOccupants, 1)
        {
            queueOutside = new Collection<Citizen>();
            this.maxQueueSize = maxQueueSize;
        }

        public override bool enterBuilding(Citizen citizen)
        {
            entranceLock.Wait();
            if ( queueOutside.Count > 0 || numOccupants >= maxOccupants)
            {
                return joinQueue(citizen);
            }
            occupants.Add(citizen);
            entranceLock.Release();
            return true;
        }
        
        public bool joinQueue(Citizen citizen)
        {
            if (queueOutside.Count >= maxQueueSize)
            {
                entranceLock.Release();
                return false;
            }
            queueOutside.Add(citizen);
            entranceLock.Release();
            return true;
        }

        public override void exitBuilding(Citizen citizen)
        {
            entranceLock.Wait();
            occupants.Remove(citizen);
            if (queueOutside.Count > 0 && numOccupants < maxOccupants)
            {
                occupants.Add(queueOutside[0]);
                queueOutside.RemoveAt(0);
            }

            entranceLock.Release();
        }

        public int MaxQueueSize => maxQueueSize;

        public Collection<Citizen> QueueOutside => queueOutside;
    }
}