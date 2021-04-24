using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelAssets", order = 1)]
public class LevelAssets : ScriptableObject
{
	public Transform wallPrefab, bedrockPrefab, rockPrefab;
	public Transform floorPrefab;
	public float caveness = 0.5f;
	public int roomBudget = 10;
	public float hallwayLength = 1.5f;
}
