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
			InventoryItem instance = Instantiate(prefab);

			// Apply level
			int lootLevel = Random.Range(levelCompleted / 4, levelCompleted / 2 + 1);
			instance.Scale(lootLevel);
			instance.level = lootLevel;
			instance.cost = Random.Range(instance.cost * lootLevel, instance.cost * (lootLevel + 1));

			// Apply infusion
			if (instance is Weapon weapon && Random.Range(0f, 1f) < 0.5f)
			{
				weapon.Infuse((DamageElement)Random.Range(0, (int)DamageElement.Spirit + 1));
				instance.cost = Mathf.CeilToInt(instance.cost * 1.25f);
			}
			
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
}
