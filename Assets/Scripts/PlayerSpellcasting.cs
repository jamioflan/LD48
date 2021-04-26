using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpellcasting : MonoBehaviour
{
	public GameObject shieldObject;

	PlayerCapitalMovement movement = null;
	PlayerInventory inventory = null;
	Spell playerSpellA = null;
	Spell playerSpellB = null;

    // Start is called before the first frame update
    void Start()
    {
		movement = GetComponent<PlayerCapitalMovement>();
		inventory = GetComponent<PlayerInventory>();
	}

    // Update is called once per frame
    void Update()
    {
		if (Game.inst.state == Game.State.IN_LEVEL)
		{
			playerSpellA = inventory.GetSpellInSlot(0);
			playerSpellB = inventory.GetSpellInSlot(1);

			if (Input.GetKey(KeyCode.Q) && playerSpellA != null)
			{
				playerSpellA.CastSpell(movement.targetPosition);
				if (playerSpellA.sfx != null)
					playerSpellA.sfx.Play();
			}
			if (Input.GetKey(KeyCode.E) && playerSpellB != null)
			{
				playerSpellB.CastSpell(movement.targetPosition);
				if (playerSpellB.sfx != null)
					playerSpellB.sfx.Play();
			}

			if (playerSpellA is Shield shieldA)
			{
				shieldObject.SetActive(shieldA.GetShieldActive());
			}
			else if (playerSpellB is Shield shieldB)
			{
				shieldObject.SetActive(shieldB.GetShieldActive());
			}
			else shieldObject.SetActive(false);

			shieldObject.transform.localScale = Vector3.one * (0.2f * Mathf.Sin(Time.time) + 1.2f);
		}
	}
}
