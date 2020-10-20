using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using src.SaveLoadLibrary;
using Unity.Collections;

namespace src.CitizenLibrary
{
    public class Supermarket : Building
    {
        private int maxQueueSize; 
        private HashSet<Citizen> queueOutside; // Use different dataStructure here
        private SemaphoreSlim queueSemaphore;
        public Supermarket(string id, double exposureFactor, int maxOccupants, int numOccupants, int maxQueueSize) : base(id, exposureFactor, maxOccupants, numOccupants, 1)
        {
            queueOutside = new HashSet<Citizen>();
            this.maxQueueSize = maxQueueSize;
            queueSemaphore = new SemaphoreSlim(1);
        }

        public Supermarket(BuildingData b) : base(b)
        {
            if (b is SupermarketData s)
            {
                maxQueueSize = s.MaxQueueSize;
                for (int i = 0; i < s.CitizensInQueue.Length; i++)
                {
                    for (int j = 0; j < CitizenWorkerThread.citizens.Count; j++)
                    {
                        if (CitizenWorkerThread.citizens[j].ID.Equals(id)) // We do it like this because we want to get the actual reference to the citizen and not a value that equates to the citizen
                        {
                            enterBuilding(CitizenWorkerThread.citizens[j]); // We have the citizens that are waiting outside do this. That way we both add and initialize presence
                            break;
                        }
                    }
                }
                // Need to figure out how we're gonna convert 
            }
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
            numOccupants++;
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
            queueOutside.Remove(citizen);// Removes citizen from the queue
            // All locks are released once all operations are completed.
            entranceLock.Release(); 
            queueSemaphore.Release();
            numOccupants--;
            return false;
        }

        public int MaxQueueSize => maxQueueSize;

        public HashSet<Citizen> QueueOutside => queueOutside;
    }
}