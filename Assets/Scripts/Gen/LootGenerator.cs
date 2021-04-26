using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
	public static LootGenerator inst;

	public List<InventoryItem> scaleableLootPrefabs = new List<InventoryItem>();

	private void Awake()
	{
		inst = this;
	}

	public InventoryItem[] GenerateLoot(int count, int levelCompleted)
	{
		InventoryItem[] result = new InventoryItem[count];

		for(int i = 0; i < count; i++)
		{
			InventoryItem prefab = scaleableLootPrefabs[Random.Range(0, scaleableLootPrefabs.Count)];

			// Reroll twice if we pick an expensive item to weight the early game loot
			if (prefab.cost > Game.inst.currency + 2)
				prefab = scaleableLootPrefabs[Random.Range(0, scaleableLootPrefabs.Count)];
			if (prefab.cost > Game.inst.currency + 2)
				prefab = scaleableLootPrefabs[Random.Range(0, scaleableLootPrefabs.Count)];

			InventoryItem instance = Instantiate(prefab);

			// Apply level
			int lootLevel = Random.Range(levelCompleted / 4, levelCompleted / 2 + 1);
			instance.Scale(lootLevel);
			instance.level = lootLevel;
			instance.cost = Random.Range(instance.cost * (lootLevel + 1), instance.cost * (lootLevel + 2));

			// Apply infusion
			if (instance is Weapon weapon && Random.Range(0f, 1f) < 0.5f)
			{
				weapon.Infuse((DamageElement)Random.Range(0, (int)DamageElement.Spirit + 1));
				instance.cost = Mathf.CeilToInt(instance.cost * 1.25f);
			}

			if (instance.cost <= 0)
				instance.cost = 1;
			
			instance.gameObject.SetActive(false);
			result[i] = instance;
		}

		return result;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public string[] bossNameTemplates = new string[0];

	public string GetBossName(string name, int bossCount)
	{
		int pick = Random.Range(0, bossNameTemplates.Length);
		return string.Format(bossNameTemplates[pick], name, GetDepthName(bossCount));
	}

	public string GetDepthName(int bossCount)
	{
		switch (bossCount)
		{
			case 0: return "Deep";
			case 1: return "Deeper";
			case 2: return "Deepest";
			case 3: return "Deeperer";
			case 4: return "Deeperest";
			case 5: return "Deepestest";
			default:
			{
				int tens = bossCount / 6;
				int ones = bossCount % 6;
				switch(tens)
				{
					case 1: return $"{GetDepthName(ones)} Depths";
					case 2: return $"{GetDepthName(ones)} Darkness";
					case 3: return $"{GetDepthName(ones)} Defiled";
					case 4: return $"{GetDepthName(ones)} Desecrated";
				}
				return "THE DEEPEST OF ALL DEPTHS";
			}
		}
	}
}
