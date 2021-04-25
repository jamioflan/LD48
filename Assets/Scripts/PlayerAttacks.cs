using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{

	// Inspector variables
	public Transform weaponOrigin;


	// Private variables
	float baseAttackCooldown = 2.0f;
	float baseSweepAttackRange = 2.5f;
	float baseSweepAttackDamage = 0.75f;
	float baseJabAttackRange = 6.0f;
	float baseJabAttackDamage = 1.0f;
	float jabAttackRadius = 0.75f;
	float attackCountDown = 0.0f;
	PlayerMovement movement = null;
	PlayerInventory inventory = null;
	Weapon equippedWeapon = null;



	// Start is called before the first frame update
	void Start()
    {
		movement = GetComponent<PlayerMovement>();
		inventory = GetComponent<PlayerInventory>();
	}

	// Update is called once per frame
	void Update()
    {
		equippedWeapon = inventory.GetWeaponInSlot(0);

		if (equippedWeapon != null)
		{
			if (attackCountDown <= 0.0f)
			{

				if (Input.GetMouseButton(0))
				{
					JabAttack(equippedWeapon);
					attackCountDown = baseAttackCooldown * equippedWeapon.cooldownModifier();
				}

				if (Input.GetMouseButton(1))
				{
					SweepAttack(equippedWeapon);
					attackCountDown = baseAttackCooldown * equippedWeapon.cooldownModifier();
				}
			}

			attackCountDown = Mathf.Max(attackCountDown - Time.deltaTime, 0.0f);
		}
	}

	void JabAttack(Weapon weapon)
	{
		Collider[] hitColliders = Physics.OverlapCapsule(transform.position + (movement.targetDirection * 0.25f), transform.position + (movement.targetDirection * baseJabAttackRange * weapon.jabAttackRangeModifier()), jabAttackRadius, 1 << LayerMask.NameToLayer("Enemy"));

		foreach (Collider victim in hitColliders)
		{
			Enemy enemy = victim.GetComponent<Enemy>();

			if (enemy is null)
			{
				Debug.Log("You failed to stab " + victim.name);
			}
			else
			{
				Debug.Log("You stabbed " + victim.name + " with damage " + baseJabAttackDamage + " and weapon modifier " + weapon.damageModifier);
				if(weapon.isCrushingWeapon())
					enemy.SufferDamage(baseJabAttackDamage * weapon.damageModifier, DamageType.Crushing, weapon.dElement);
				else
					enemy.SufferDamage(baseJabAttackDamage * weapon.damageModifier, DamageType.Piercing, weapon.dElement);
			}
		}
	}

	void SweepAttack(Weapon weapon)
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, baseSweepAttackRange * weapon.sweepAttackRangeModifier(), 1 << LayerMask.NameToLayer("Enemy"));

		foreach (Collider victim in hitColliders)
		{
			Enemy enemy = victim.GetComponent<Enemy>();

			if (enemy is null)
			{
				Debug.Log("You failed to slash " + victim.name);
			}
			else
			{
				Debug.Log("You slashed " + victim.name + " with damage " + baseJabAttackDamage + " and weapon modifier " + weapon.damageModifier);
				if (weapon.isCrushingWeapon())
					enemy.SufferDamage(baseSweepAttackDamage * weapon.damageModifier, DamageType.Crushing, weapon.dElement);
				else
					enemy.SufferDamage(baseSweepAttackDamage * weapon.damageModifier, DamageType.Slashing, weapon.dElement);
			}
		}
	}
}