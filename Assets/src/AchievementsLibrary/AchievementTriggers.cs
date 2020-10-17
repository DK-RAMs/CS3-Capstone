using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using src.UILibrary;

namespace src.AchievementsLibrary
{
	public class AchievementTriggers : MonoBehaviour
	{
		// triggres achievement 1
		public void ButtonPressed()
		{
			GlobalAchievements.ac1Count += 1;
		}

		// triggres achievement 2
		public void LevelReduced()
		{
			GlobalAchievements.ac2Count += 1;
		}

		// triggres achievement 3
		public void LevelRaised()
		{
			GlobalAchievements.ac3Count += 1;
		}
	}
}
