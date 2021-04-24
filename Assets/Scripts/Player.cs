using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player inst;

	public PlayerMovement movement;
	public PlayerAttacks attacks;
	public PlayerInventory inventory;

    void Awake()
    {
		inst = this;
		movement = GetComponent<PlayerMovement>();
		attacks = GetComponent<PlayerAttacks>();
		inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        
    }
}
