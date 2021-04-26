using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
	public static Player inst;

	public Sprite[] icons;
	public Material[] skins;
	public Character character;
	public string[] names;
	public string[] descs;
	public PlayerCapitalMovement movement;
	public PlayerAttacks attacks;
	public PlayerInventory inventory;
	public AudioSource pickupSFX, levelupSFX;

	public enum Character
	{
		ARCHER,
		WIZARD,
		KNIGHT,
		DRUID,
	}

	public void Heal(float count)
	{
		// Increase HP
		if (count + health > maxHealth)
			count = maxHealth - health;

		UI.inst.SpawnDamageNumbers(Mathf.CeilToInt(count), Color.green, transform.position);

		health += count;
	}

	void Awake()
    {
		inst = this;
		movement = GetComponent<PlayerCapitalMovement>();
		attacks = GetComponent<PlayerAttacks>();
		inventory = GetComponent<PlayerInventory>();
    }

	public void ConfirmCharacter(Character c)
	{
		character = c;
		GetComponentInChildren<MeshRenderer>().sharedMaterial = skins[(int)c];
	}

	public override void Die()
	{
		base.Die();

		UI.inst.GameOver(LevelGenerator.inst.currentLevel);
	}

	protected override void Update()
    {
		base.Update();

		foreach(Collider coll in Physics.OverlapBox(transform.position, Vector3.one * 0.25f))
		{
			Pickup pickup = coll.GetComponent<Pickup>();
			if (pickup != null)
			{
				switch (pickup.type)
				{
					case Pickup.Type.COIN:
						Game.inst.AwardCurrency(pickup.count);
						pickupSFX.Play();
						break;
					case Pickup.Type.HEALTH:
						Heal(pickup.count);
						break;
				}
				Destroy(coll.gameObject);
			}

			if(coll.GetComponent<Hole>())
			{
				levelupSFX.Play();
				Game.inst.EnteredHole();
			}

			SkullBossHand hand = coll.GetComponent<SkullBossHand>();
			if (hand != null)
			{
				if (hand.canAttackPlayer)
				{
					SufferDamage(hand.boss.enemy.attackDamage, hand.boss.enemy.dType, hand.boss.enemy.dElement, hand.transform.position);
					hand.canAttackPlayer = false;
				}
			}
		}
	}

}
