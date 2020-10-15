using UnityEngine;

namespace src.UILibrary
{
    public class StartCountdown : MonoBehaviour
    {
        public void triggerCountdown()
        {
            Countdown.start = 1;
        }
    }
}