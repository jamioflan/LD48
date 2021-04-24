using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public List<LevelAssets> levels = new List<LevelAssets>();
	public int levelSize = 64;

	// Currently generated level
	public int currentLevel = 0;
	public Transform[,] walls = null;

	private const int WALL_TYPE_EMPTY = 0;
	private const int WALL_TYPE_WALL = 1;
	private const int WALL_TYPE_BEDROCK = 2;



	void Start()
    {
		GenerateLevel(0);
    }

    void Update()
    {
        
    }

	public void GenerateLevel(int level)
	{
		int assetsToUse = level % levels.Count;
		LevelAssets assets = levels[assetsToUse];

		currentLevel = level;
		int[,] wallTypes = new int[levelSize, levelSize];

		for (int i = 0; i < levelSize; i++)
		{
			for (int k = 0; k < levelSize; k++)
			{
				int dX = i - levelSize / 2;
				int dZ = k - levelSize / 2;

				float radius = Mathf.Sqrt(dX * dX + dZ * dZ) / levelSize + Mathf.PerlinNoise(i * 0.0913f, k * 0.07498f);
				if(radius > 0.75f)
				{
					wallTypes[i, k] = WALL_TYPE_WALL;
				}
			}
		}

		// Place boundaries
		for (int i = 0; i < levelSize; i++)
		{
			wallTypes[i, 0] = WALL_TYPE_BEDROCK;
			wallTypes[i, levelSize - 1] = WALL_TYPE_BEDROCK;
			wallTypes[0, i] = WALL_TYPE_BEDROCK;
			wallTypes[levelSize - 1, i] = WALL_TYPE_BEDROCK;
		}

		walls = new Transform[levelSize, levelSize];
		for (int i = 0; i < levelSize; i++)
		{
			for (int k = 0; k < levelSize; k++)
			{
				switch(wallTypes[i,k])
				{
					case WALL_TYPE_WALL:
					case WALL_TYPE_BEDROCK:
					{
						walls[i, k] = Instantiate(assets.wallPrefab, new Vector3(i, 0, k), Quaternion.identity, transform);
						break;
					}
				}
			}
		}
	}

	public void DeleteLevel()
	{
		currentLevel = -1;
		foreach(Transform t in walls)
		{
			if(t != null)
				Destroy(t.gameObject);
		}
		walls = null;
	}
}
