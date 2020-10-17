using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace src.UILibrary
{
	public class LayerUI : MonoBehaviour
	{
		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			BuildingUI.isCovered = true;
		}

		public void isAbove()
		{
			BuildingUI.isCovered = true;

		}

		public void isBelow()
		{
			BuildingUI.isCovered = false;
		}
	}
}