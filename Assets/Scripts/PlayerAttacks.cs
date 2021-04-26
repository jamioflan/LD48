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
	float attackCountDownTotal = 1.0f;
	PlayerCapitalMovement movement = null;
	PlayerInventory inventory = null;
	CreatureAnimations animations = null;
	Weapon equippedWeapon = null;

	public float GetAttackCooldown() { return attackCountDown; }
	public float GetAttackCooldownParametric() { return attackCountDown / attackCountDownTotal; }


	// Start is called before the first frame update
	void Start()
    {
		movement = GetComponent<PlayerCapitalMovement>();
		inventory = GetComponent<PlayerInventory>();
		animations = GetComponent<CreatureAnimations>();
	}

	// Update is called once per frame
	void Update()
    {
		if (Game.inst.state == Game.State.IN_LEVEL)
		{
			equippedWeapon = inventory.GetWeaponInSlot(0);

			if (equippedWeapon != null)
			{
				if (attackCountDown <= 0.0f)
				{

					if (Input.GetMouseButton(0))
					{
						attackCountDownTotal = attackCountDown = baseAttackCooldown * equippedWeapon.cooldownModifier();
						JabAttack(equippedWeapon);

					}

					if (Input.GetMouseButton(1))
					{
						attackCountDownTotal = attackCountDown = baseAttackCooldown * equippedWeapon.cooldownModifier();
						SweepAttack(equippedWeapon);

					}
				}

				attackCountDown = Mathf.Max(attackCountDown - Time.deltaTime, 0.0f);
			}
		}
	}

	void JabAttack(Weapon weapon)
	{
		animations.PlayAnim(CreatureAnimations.WeaponAnim.STAB, movement.targetDirection, 0.5f);

		Collider[] hitColliders = Physics.OverlapCapsule(transform.position + (movement.targetDirection * 0.25f), transform.position + (movement.targetDirection * baseJabAttackRange * weapon.jabAttackRangeModifier()), jabAttackRadius, 1 << LayerMask.NameToLayer("Enemy"));

		foreach (Collider victim in hitColliders)
		{
			Enemy enemy = victim.GetComponent<Enemy>();

			if (enemy == null)
			{
				Debug.Log("You failed to stab " + victim.name);
			}
			else
			{
				Debug.Log("You stabbed " + victim.name + " with damage " + baseJabAttackDamage + " and weapon modifier " + weapon.damageModifier);
				if(weapon.isCrushingWeapon())
					enemy.SufferDamage(baseJabAttackDamage * weapon.damageModifier, DamageType.Crushing, weapon.dElement, transform.position);
				else
					enemy.SufferDamage(baseJabAttackDamage * weapon.damageModifier, DamageType.Piercing, weapon.dElement, transform.position);
			}
		}
	}

	void SweepAttack(Weapon weapon)
	{
		animations.PlayAnim(CreatureAnimations.WeaponAnim.SWING, movement.targetDirection, 0.5f);

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, baseSweepAttackRange * weapon.sweepAttackRangeModifier(), 1 << LayerMask.NameToLayer("Enemy"));

		foreach (Collider victim in hitColliders)
		{
			Enemy enemy = victim.GetComponent<Enemy>();

			if (enemy == null)
			{
				Debug.Log("You failed to slash " + victim.name);
			}
			else
			{
				Debug.Log("You slashed " + victim.name + " with damage " + baseJabAttackDamage + " and weapon modifier " + weapon.damageModifier);
				if (weapon.isCrushingWeapon())
					enemy.SufferDamage(baseSweepAttackDamage * weapon.damageModifier, DamageType.Crushing, weapon.dElement, transform.position);
				else
					enemy.SufferDamage(baseSweepAttackDamage * weapon.damageModifier, DamageType.Slashing, weapon.dElement, transform.position);
			}
		}
	}
}
