using System.Collections;
using System.Collections.Generic;
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