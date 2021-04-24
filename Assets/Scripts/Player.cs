using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player inst;

	public PlayerMovement movement;
	public PlayerAttacks attacks;

    void Awake()
    {
		inst = this;
		movement = GetComponent<PlayerMovement>();
		attacks = GetComponent<PlayerAttacks>();
    }

    void Update()
    {
        
    }
}
