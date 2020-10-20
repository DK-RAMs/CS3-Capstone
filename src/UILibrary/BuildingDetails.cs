using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Description", menuName = "Building Details")]
public class BuildingDetails : ScriptableObject
{
	public new string name;
	public string status;
	public Sprite artwork;
	public int occupancy;
}
