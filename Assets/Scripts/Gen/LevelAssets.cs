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

	public List<SpawnData> regularSpawns = new List<SpawnData>();
	public int numToSpawn = 8;
	public bool spawnBoss = false;
	public SpawnData bossData = null;
	
	[System.Serializable]
	public class SpawnData
	{
		public Transform prefab;
		public float spawnWeight = 1.0f;
		public int minNumPerGroup = 1;
		public int maxNumPerGroup = 3;
	}
}
