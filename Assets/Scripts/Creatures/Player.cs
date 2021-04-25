using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
	public static Player inst;

	public PlayerCapitalMovement movement;
	public PlayerAttacks attacks;
	public PlayerInventory inventory;

    void Awake()
    {
		inst = this;
		movement = GetComponent<PlayerCapitalMovement>();
		attacks = GetComponent<PlayerAttacks>();
		inventory = GetComponent<PlayerInventory>();
    }

	protected override void Update()
    {
		base.Update();
    }
}
