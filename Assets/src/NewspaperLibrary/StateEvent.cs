using UnityEngine;

namespace src.NewspaperLibrary
{
    [CreateAssetMenu(fileName = "new State Event", menuName = "Newspaper/Events/State Event")]
    public class StateEvent : NewspaperEvent
    {
        public StateEvent(string mainText) : base(mainText)
        {
        }
    }
}