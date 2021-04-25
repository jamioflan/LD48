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

	public enum Character
	{
		ARCHER,
		WIZARD,
		KNIGHT,
		DRUID,
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

	protected override void Update()
    {
		base.Update();
    }
}
