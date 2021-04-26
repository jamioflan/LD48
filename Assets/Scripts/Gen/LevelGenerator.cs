using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
	public static LevelGenerator inst;

	public List<LevelAssets> levels = new List<LevelAssets>();
	public int levelSize = 64;

	// Currently generated level
	public LevelData currentLevel = null;
	public Transform[,] walls = null;
	public Transform floor = null;

	private const int WALL_TYPE_EMPTY = 0;
	private const int WALL_TYPE_ROCK = 1;
	private const int WALL_TYPE_BEDROCK = 2;
	private const int WALL_TYPE_WALL = 3;

	private void Awake()
	{
		inst = this;
	}

	void Start()
    {
		//GenerateLevel(0);
    }

    void Update()
    {
        
    }

	public void GenerateLevel(int level)
	{
		int assetsToUse = level % levels.Count;
		LevelAssets assets = levels[assetsToUse];

		currentLevel = new LevelData()
		{
			levelNumber = level,
			levelName = assets.displayName,
			assets = assets,
			bossName = "Fred",
			enemies = new List<Transform>(),
		};


		int[,] wallTypes = new int[levelSize, levelSize];
		List<NavMeshBuildSource> src = new List<NavMeshBuildSource>();

		// Start with cave carving
		for (int i = 0; i < levelSize; i++)
		{
			for (int k = 0; k < levelSize; k++)
			{
				int dX = i - levelSize / 2;
				int dZ = k - levelSize / 2;

				float caveRadius = Mathf.Sqrt(dX * dX + dZ * dZ) / levelSize + Mathf.PerlinNoise(i * 0.0913f, k * 0.07498f);
				if(caveRadius > assets.caveness)
				{
					wallTypes[i, k] = WALL_TYPE_ROCK;
				}
			}
		}

		// Then build the route
		int x = levelSize / 2;
		int z = levelSize / 2;
		BuildRoom(x, z, wallTypes, assets.roomBudget, assets.hallwayLength, assets.minRoomSize, assets.maxRoomSize);

		// Place boundaries
		for (int i = 0; i < levelSize; i++)
		{
			wallTypes[i, 0] = WALL_TYPE_BEDROCK;
			wallTypes[i, levelSize - 1] = WALL_TYPE_BEDROCK;
			wallTypes[0, i] = WALL_TYPE_BEDROCK;
			wallTypes[levelSize - 1, i] = WALL_TYPE_BEDROCK;

			if(Random.Range(0f,1f) > 0.5f)
			{
				wallTypes[i, 1] = WALL_TYPE_BEDROCK;
				wallTypes[levelSize - 1 - i, levelSize - 2] = WALL_TYPE_BEDROCK;
				wallTypes[1, levelSize - 1 - i] = WALL_TYPE_BEDROCK;
				wallTypes[levelSize - 2, i] = WALL_TYPE_BEDROCK;
			}
		}

		walls = new Transform[levelSize, levelSize];
		for (int i = 0; i < levelSize; i++)
		{
			for (int k = 0; k < levelSize; k++)
			{
				switch(wallTypes[i,k])
				{
					case WALL_TYPE_ROCK:
					{
						walls[i, k] = Instantiate(assets.rockPrefab, new Vector3(i, -1f, k), Quaternion.identity, transform);
						break;
					}
					case WALL_TYPE_WALL:
					{
						walls[i, k] = Instantiate(assets.wallPrefab, new Vector3(i, -1f, k), Quaternion.identity, transform);
						break;
					}
					case WALL_TYPE_BEDROCK:
					{
						walls[i, k] = Instantiate(assets.bedrockPrefab, new Vector3(i, -1f, k), Quaternion.identity, transform);
						
						break;
					}
				}

				if(walls[i, k] != null)
				{
					MeshFilter mesh = walls[i, k].GetComponentInChildren<MeshFilter>();
					src.Add(new NavMeshBuildSource()
					{
						area = 0,
						shape = NavMeshBuildSourceShape.Mesh,
						sourceObject = mesh.sharedMesh,
						transform = mesh.transform.localToWorldMatrix,
					});
				}
			}
		}

		floor = Instantiate(assets.floorPrefab, transform);
		foreach (MeshFilter mf in floor.GetComponentsInChildren<MeshFilter>())
		{
			src.Add(new NavMeshBuildSource()
			{
				area = 0,
				shape = NavMeshBuildSourceShape.Mesh,
				sourceObject = mf.sharedMesh,
				transform = mf.transform.localToWorldMatrix,
			});
		}

		// Place player spawn pos - start at outer edge and work inwards
		Vector2Int spawnPoint = Vector2Int.zero;
		float radius = levelSize / 2f;
		while(spawnPoint == Vector2Int.zero && radius >= 0.0f)
		{
			float angle = Random.Range(0, Mathf.PI * 2f);
			Vector2 pos = new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
			Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
			if(gridPos.x > 0 && gridPos.x < levelSize - 1 && gridPos.y > 0 && gridPos.y < levelSize - 1)
			{
				if (wallTypes[gridPos.x, gridPos.y] == WALL_TYPE_EMPTY)
					spawnPoint = gridPos;
			}

			radius -= 1.0f;
		}

		if (spawnPoint == Vector2Int.zero)
		{
			Debug.LogError("Failed to find spawn point");
			spawnPoint = new Vector2Int(levelSize / 2, levelSize / 2);
		}

		Player.inst.GetComponent<CharacterController>().enabled = false;
		Player.inst.transform.position = new Vector3(spawnPoint.x, 0.0f, spawnPoint.y);
		Player.inst.GetComponent<CharacterController>().enabled = true;
		Camera.main.transform.parent.position = Player.inst.transform.position;


		// Place enemies
		float totalWeight = 0.0f;
		foreach(LevelAssets.SpawnData spawn in assets.regularSpawns)
		{
			totalWeight += spawn.spawnWeight;
		}
		for(int i = 0; i < assets.numToSpawn; i++)
		{
			float pick = Random.Range(0.0f, totalWeight);
			int index = 0;

			do
			{
				if (pick <= assets.regularSpawns[index].spawnWeight)
				{
					Spawn(assets.regularSpawns[index], wallTypes, spawnPoint);
				}

				pick -= assets.regularSpawns[index].spawnWeight;
				index++;
			}
			while (pick >= 0.0f && index < assets.regularSpawns.Count);
		}

		// Place an exit hole / boss spawn
		Dictionary<Vector2Int, int> connected = new Dictionary<Vector2Int, int>();
		connected.Add(spawnPoint, 0);
		int furthestExplored = Explore(connected, spawnPoint, wallTypes, 0);

		// Go a good distance from the player
		int target = furthestExplored * 3 / 4;
		Vector2Int targetPos = Vector2Int.zero;
		foreach(var kvp in connected)
		{
			if(kvp.Value >= target)
			{
				targetPos = kvp.Key;
				break;
			}
		}

		Transform hole = Instantiate(assets.holePrefab, new Vector3(targetPos.x + 0.5f, 0, targetPos.y + 0.5f), Quaternion.identity, transform);
		currentLevel.enemies.Add(hole);
		UI.inst.Announce($"Level {level + 1} - {assets.displayName}");

		if (assets.spawnBoss)
		{
			Spawn(assets.bossData, wallTypes, spawnPoint, forceSpawn:true);
			currentLevel.bossName = LootGenerator.inst.GetBossName(assets.bossData.name, level / 3);
			UI.inst.Announce($@"Level {level + 1} - {assets.displayName}");
			currentLevel.boss = currentLevel.enemies[currentLevel.enemies.Count - 1];

			hole.gameObject.SetActive(false);
			currentLevel.boss.GetComponent<Creature>().setActiveOnDeath = hole.gameObject;
		}
		else
		{
			
		}

		UI.inst.SetLevelNameText($"Level {level + 1} - {assets.displayName}");

		RenderSettings.fogDensity = 0.2f + 0.01f * level;

		NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(0);
		NavMeshData data = NavMeshBuilder.BuildNavMeshData(settings, src, new Bounds(Vector3.one * (levelSize / 2f) + Vector3.down, Vector3.one * levelSize), Vector3.zero, Quaternion.identity);
		NavMesh.RemoveAllNavMeshData();
		NavMesh.AddNavMeshData(data);
	}

	private int Explore(Dictionary<Vector2Int, int> connected, Vector2Int pos, int[,] map, int distance)
	{
		int furthestExplored = distance;

		// +x
		if(map[pos.x + 1,pos.y] == WALL_TYPE_EMPTY)
		{
			Vector2Int newPos = new Vector2Int(pos.x + 1, pos.y);
			if (!connected.ContainsKey(newPos))
			{
				connected.Add(newPos, distance + 1);
				furthestExplored = Mathf.Max(furthestExplored, Explore(connected, newPos, map, distance + 1));
			}
		}

		// -x
		if (map[pos.x - 1, pos.y] == WALL_TYPE_EMPTY)
		{
			Vector2Int newPos = new Vector2Int(pos.x - 1, pos.y);
			if (!connected.ContainsKey(newPos))
			{
				connected.Add(newPos, distance + 1);
				furthestExplored = Mathf.Max(furthestExplored, Explore(connected, newPos, map, distance + 1));
			}
		}

		// +y
		if (map[pos.x, pos.y + 1] == WALL_TYPE_EMPTY)
		{
			Vector2Int newPos = new Vector2Int(pos.x, pos.y + 1);
			if (!connected.ContainsKey(newPos))
			{
				connected.Add(newPos, distance + 1);
				furthestExplored = Mathf.Max(furthestExplored, Explore(connected, newPos, map, distance + 1));
			}
		}

		// -y
		if (map[pos.x, pos.y - 1] == WALL_TYPE_EMPTY)
		{
			Vector2Int newPos = new Vector2Int(pos.x, pos.y - 1);
			if (!connected.ContainsKey(newPos))
			{
				connected.Add(newPos, distance + 1);
				furthestExplored = Mathf.Max(furthestExplored, Explore(connected, newPos, map, distance + 1));
			}
		}

		return furthestExplored;
	}

	private void Spawn(LevelAssets.SpawnData spawn, int[,] map, Vector2Int playerSpawnPos, bool forceSpawn = false)
	{
		int attempts = 300;
		bool found = false;
		while(!found && attempts > 0)
		{
			attempts--;

			int x = Random.Range(0, levelSize);
			int y = Random.Range(0, levelSize);

			if (map[x, y] != WALL_TYPE_EMPTY)
				continue;

			if (Vector2Int.Distance(playerSpawnPos, new Vector2Int(x, y)) < 6f)
				continue;

			found = true;

			int count = Random.Range(spawn.minNumPerGroup, spawn.maxNumPerGroup + 1);
			for (int i = 0; i < count; i++)
			{
				Transform enemy = Instantiate(spawn.prefab, new Vector3(x + 0.5f, 0.0f, y + 0.5f), Quaternion.identity, transform);
				currentLevel.enemies.Add(enemy);
			}
		}

		Debug.LogWarning("Failed to spawn enemy");
		if(!found && forceSpawn)
		{
			Transform enemy = Instantiate(spawn.prefab, new Vector3(32f, 0.0f, 32f), Quaternion.identity, transform);
			currentLevel.enemies.Add(enemy);
		}
	}

	private void PlaceRoom(int x, int z, int width, int height, int[,] map)
	{
		for (int i = x - width / 2; i < x + width / 2; i++)
		{
			for (int k = z - height / 2; k < z + height / 2; k++)
			{
				if (i >= 0 && i < levelSize && k >= 0 && k < levelSize)
				{
					// Wall
					if (i == x - width / 2 || i == x + width / 2 - 1
					|| k == z - height / 2 || k == z + height / 2 - 1)
					{
						if (map[i, k] == WALL_TYPE_ROCK)
							map[i, k] = WALL_TYPE_WALL;
					}
					else
					{
						// Carved
						map[i, k] = WALL_TYPE_EMPTY;
					}
				}

			}
		}
	}

	private void BuildRoom(int x, int z, int[,] map, int budgetLeft, float hallwayLength, int minSize, int maxSize)
	{
		int width = Random.Range(minSize, maxSize);
		int height = Random.Range(minSize, maxSize);

		PlaceRoom(x, z, width, height, map);

		budgetLeft--;

		if (budgetLeft > 0)
		{
			int numBranches = Random.Range(1, Mathf.Min(2, budgetLeft) + 1);
			int budgetEach = budgetLeft / numBranches;

			for (int i = 0; i < numBranches; i++)
			{
				int newX = x;
				int newZ = z;
				bool solutionFound = false;
				while (!solutionFound)
				{
					newX = x;
					newZ = z;
					int dir = Random.Range(0, 4);
					switch (dir)
					{
						case 0:
							newX += (int)(width * hallwayLength);
							break;
						case 1:
							newZ += (int)(height * hallwayLength);
							break;
						case 2:
							newX -= (int)(width * hallwayLength);
							break;
						case 3:
							newZ -= (int)(height * hallwayLength);
							break;
					}
					if (newX > 0 && newX < levelSize && newZ > 0 && newZ < levelSize)
						solutionFound = true;
				}

				PlaceRoom((newX + x) / 2, (newZ + z) / 2, Mathf.Abs(newX - x) + 4, Mathf.Abs(newZ - z) + 4, map);
				BuildRoom(newX, newZ, map, budgetEach, hallwayLength, minSize, maxSize);
			}
		}
	}

	public void DeleteLevel()
	{
		foreach(Transform t in walls)
		{
			if(t != null)
				Destroy(t.gameObject);
		}
		walls = null;

		if (floor != null)
			Destroy(floor.gameObject);
		floor = null;

		foreach (Transform t in currentLevel.enemies)
			if(t != null)
				Destroy(t.gameObject);
		currentLevel.enemies.Clear();

		currentLevel = null;
	}
}
