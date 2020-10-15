using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Unity.Collections;

namespace src.CitizenLibrary
{
    public class Supermarket : Building
    {
        private int maxQueueSize; 
        private HashSet<Citizen> queueOutside; // Use different dataStructure here
        private SemaphoreSlim queueSemaphore;
        public Supermarket(string id, double happinessContribution, double exposureFactor, int maxOccupants, int numOccupants, int maxQueueSize) : base(id, happinessContribution, exposureFactor, maxOccupants, numOccupants, 1)
        {
            queueOutside = new HashSet<Citizen>();
            this.maxQueueSize = maxQueueSize;
            queueSemaphore = new SemaphoreSlim(1);
        }

        public override bool enterBuilding(Citizen citizen)
        {
            entranceLock.Wait();
            if ( queueOutside.Count > 0 || (numOccupants >= maxOccupants && !citizen.Rebel))
            {
                return joinQueue(citizen);
            }

            occupantAccessLock.Wait();
            occupants.Add(citizen);
            occupantAccessLock.Release();
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

        public override bool exitBuilding(Citizen citizen)
        {
            entranceLock.Wait(); // We acquire the lock that is next to the entrance
            occupantAccessLock.Wait(); // We acquire the lock to access the HashSet
            if (occupants.Contains(citizen)) // If citizen is in the shop, do stuff
            {
                occupants.Remove(citizen);
                queueSemaphore.Wait(); // Acquire lock to access queue HashSet
                if (queueOutside.Count > 0 && numOccupants < maxOccupants)
                {
                    Citizen s = queueOutside.ElementAt(0);
                    occupants.Add(s);
                    queueOutside.Remove(s);
                }
                // All locks are released once all operations are completed(Better to release them in the order they were generated. That way some threads can start on operations while other locks are being released.
                entranceLock.Release();
                occupantAccessLock.Release();
                queueSemaphore.Release();  
                return true;
            }
            occupantAccessLock.Release();
            
            queueSemaphore.Wait();
            if (queueOutside.Contains(citizen)) // Checks if citizen is in the queue
            {
                queueOutside.Remove(citizen);
            }
            // All locks are released once all operations are completed.
            entranceLock.Release(); 
            queueSemaphore.Release();
            return false;
        }

        public int MaxQueueSize => maxQueueSize;

        public HashSet<Citizen> QueueOutside => queueOutside;
    }
}