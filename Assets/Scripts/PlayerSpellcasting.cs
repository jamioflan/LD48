using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{

	PlayerMovement movement = null;
	PlayerInventory inventory = null;

	bool shieldActive = false;
	float shieldActiveTimer = 0.0f;
	float shieldCountDown = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
		movement = GetComponent<PlayerMovement>();
		inventory = GetComponent<PlayerInventory>();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
		{
			//Spell spell = inventory;
		}
		if (Input.GetKey(KeyCode.E))
		{

		}
	}
}
