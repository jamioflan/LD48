using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
	
	public Weapon playerWeapon = null;

	float baseAttackCooldown = 2.0f;
	float baseSweepAttackRange = 2.5f;
	float baseSweepAttackDamage = 1.0f;
	float baseJabAttackRange = 10.0f;
	float baseJabAttackDamage = 0.75f;
	float jabAttackRadius = 1.5f;
	float attackCountDown = 0.0f;
	PlayerMovement movement = null;

	// Start is called before the first frame update
	void Start()
    {
		movement = GetComponent<PlayerMovement>();
    }

	// Update is called once per frame
	void Update()
    {

		if (attackCountDown <= 0.1f) {

			if (Input.GetMouseButton(0))
			{
				JabAttack();
				attackCountDown = baseAttackCooldown;
			}

			if (Input.GetMouseButton(1))
			{
				SweepAttack();
				attackCountDown = baseAttackCooldown;
			}
		}

		attackCountDown = Mathf.Max(attackCountDown - Time.deltaTime, 0.0f);
	}

	void JabAttack()
	{
		Collider[] hitColliders = Physics.OverlapCapsule(transform.position, transform.position + (movement.targetDirection * baseJabAttackRange), jabAttackRadius, 1 << LayerMask.NameToLayer("Enemy"));

		foreach (Collider victim in hitColliders)
		{
			Enemy enemy = victim.GetComponent<Enemy>();

			if (enemy is null)
			{
				Debug.Log("You failed to stab " + victim.name);
			}
			else
			{
				Debug.Log("You stabbed " + victim.name);
				if(playerWeapon.isCrushingWeapon())
					enemy.SufferDamage(baseJabAttackDamage * playerWeapon.damageModifier, DamageType.Crushing, playerWeapon.dElement);
				else
					enemy.SufferDamage(baseJabAttackDamage * playerWeapon.damageModifier, DamageType.Piercing, playerWeapon.dElement);
			}
		}
	}

	void SweepAttack()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, baseSweepAttackRange, 1 << LayerMask.NameToLayer("Enemy"));

		foreach (Collider victim in hitColliders)
		{
			Enemy enemy = victim.GetComponent<Enemy>();

			if (enemy is null)
			{
				Debug.Log("You failed to slash " + victim.name);
			}
			else
			{
				Debug.Log("You slashed " + victim.name);
				if (playerWeapon.isCrushingWeapon())
					enemy.SufferDamage(baseSweepAttackDamage * playerWeapon.damageModifier, DamageType.Crushing, playerWeapon.dElement);
				else
					enemy.SufferDamage(baseSweepAttackDamage * playerWeapon.damageModifier, DamageType.Slashing, playerWeapon.dElement);
			}
		}
	}
}
