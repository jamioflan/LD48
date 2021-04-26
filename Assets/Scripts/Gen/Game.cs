using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public enum State
	{
		CHOOSE_CHARACTER,
		IN_LEVEL,
		IN_SHOP,
	}

	public static Game inst;

	public State state = State.CHOOSE_CHARACTER;
	public int level = -1;
	public int currency;
	public Pickup coinPrefab, healthPrefab;

	private void Awake()
	{
		inst = this;
	}

	// Start is called before the first frame update
	void Start()
    {
		//UI.ShowCharacterSelection();
    }

	public void DropPickup(Pickup.Type type, Vector3 pos, int count)
	{
		Pickup prefab = type == Pickup.Type.COIN ? coinPrefab : healthPrefab;

		Pickup pickup = Instantiate(prefab);
		pickup.transform.position = pos;

		pickup.count = count;
	}

	public void AwardCurrency(int x)
	{
		currency += x;
	}

	public void SpendCurrency(int x)
	{
		currency -= x;
	}

	private void NextLevel()
	{
		level++;
		LevelGenerator.inst.GenerateLevel(level);
	}

	public void Begin()
	{
		if(state == State.CHOOSE_CHARACTER)
		{
			state = State.IN_LEVEL;
			NextLevel();
		}
	}

	public void FinishedShopping()
	{
		if(state == State.IN_SHOP)
		{
			state = State.IN_LEVEL;
			NextLevel();
		}
	}

    // Update is called once per frame
    void Update()
    {
		switch (state)
		{
			case State.CHOOSE_CHARACTER:
			{
				UI.inst.ShowCharacterSelection();
				break;
			}
			case State.IN_LEVEL:
			{
				bool anyEnemiesLeft = false;
				foreach(Transform t in LevelGenerator.inst.currentLevel.enemies)
				{
					if(t != null)
					{
						anyEnemiesLeft = true;
					}
				}
				if(!anyEnemiesLeft)
				{
					state = State.IN_SHOP;
					UI.inst.OpenShop(LevelGenerator.inst.currentLevel);
				}
				break;
			}
			case State.IN_SHOP:
			{

				break;
			}
		}
    }
}
