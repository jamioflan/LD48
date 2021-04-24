using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelAssets", order = 1)]
public class LevelAssets : ScriptableObject
{
	public Transform wallPrefab;
	public Transform floorPrefab;
	public float caveness = 0.5f;
}
