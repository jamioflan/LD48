using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{

	PlayerMovement movement = null;
	PlayerInventory inventory = null;
	Spell playerSpellA = null;
	Spell playerSpellB = null;

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
		playerSpellA = inventory.GetSpellInSlot(0);
		playerSpellB = inventory.GetSpellInSlot(1);

        if (Input.GetKey(KeyCode.Q) && playerSpellA != null)
		{
			playerSpellA.castSpell(movement.targetPosition);
		}
		if (Input.GetKey(KeyCode.E) && playerSpellB != null)
		{
			playerSpellB.castSpell(movement.targetPosition);
		}
	}
}
