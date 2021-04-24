using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
	public float attackCooldown = 50.0f;
	public float sweepAttackRange = 5.0f;
	public float sweepAttackDamage = 1.0f;
	public float jabAttackRange = 10.0f;
	public float jabAttackRadius = 1.0f;
	public float jabAttackDamage = 1.0f;
	public Weapon playerWeapon = null;

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
				attackCountDown = attackCooldown;
			}

			if (Input.GetMouseButton(1))
			{
				SweepAttack();
				attackCountDown = attackCooldown;
			}
		}

		attackCountDown = Mathf.Max(attackCountDown - Time.deltaTime, 0.0f);
	}

	void JabAttack()
	{
		Collider[] hitColliders = Physics.OverlapCapsule(transform.position, transform.position + (movement.targetDirection * jabAttackRange), jabAttackRadius, 1 << LayerMask.NameToLayer("Enemy"));

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
				if(playerWeapon.isCrushingWeapon)
					enemy.SufferDamage(jabAttackDamage * playerWeapon.damageModifier, DamageType.Crushing, playerWeapon.dElement);
				else
					enemy.SufferDamage(jabAttackDamage * playerWeapon.damageModifier, DamageType.Piercing, playerWeapon.dElement);
			}
		}
	}

	void SweepAttack()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, sweepAttackRange, 1 << LayerMask.NameToLayer("Enemy"));

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
				if (playerWeapon.isCrushingWeapon)
					enemy.SufferDamage(sweepAttackDamage * playerWeapon.damageModifier, DamageType.Crushing, playerWeapon.dElement);
				else
					enemy.SufferDamage(sweepAttackDamage * playerWeapon.damageModifier, DamageType.Slashing, playerWeapon.dElement);
			}
		}
	}
}
