using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementTriggers : MonoBehaviour
{
	public void ButtonPressed()
	{
		GlobalAchievements.ac1Count += 1;
	}

	public void LevelReduced()
	{
		GlobalAchievements.ac2Count += 1;
	}

	public void LevelRaised()
	{
		GlobalAchievements.ac3Count += 1;
	}
}
