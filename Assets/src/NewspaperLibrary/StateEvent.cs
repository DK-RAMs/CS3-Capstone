using UnityEngine;

namespace src.NewspaperLibrary
{
    [CreateAssetMenu(fileName = "new State Event", menuName = "Newspaper/Events/State Event")]
    public class StateEvent : NewspaperEvent
    {
        public string stateChange;
        public int deltaHappiness;
        public int deltaHealth;
        public int deltaMoney;
        public StateEvent(string mainText) : base(mainText)
        {
            
        }
    }
}