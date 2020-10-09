using System.Collections.ObjectModel;

namespace CitizenLibrary
{
    public class Hospital : Building
    {
        private int numBeds;
        private bool overload;
        private Collection<Citizen> citizensInBed;
        public Hospital(string id, double happinessContribution, double exposureFactor, int maxOccupants, int numOccupants, int numBeds, bool overloaded) : base(id, happinessContribution, exposureFactor, maxOccupants, numOccupants, 1)
        {
            citizensInBed = new Collection<Citizen>();
        }

        public void checkinPatient(Citizen citizen)
        {
            citizensInBed.Add(citizen);
        }

        public int NumBeds => numBeds;

        public bool Overload => overload;

        public Collection<Citizen> CitizensInBed => citizensInBed;
    }
}