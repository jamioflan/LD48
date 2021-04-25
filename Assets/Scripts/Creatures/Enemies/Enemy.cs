using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
	public int treasure = 1;
	public float detectionRange = 10.0f;
	public float moveSpeed = 2.0f;
	protected Player target = null;

	// Start is called before the first frame update
	protected override void Start()
    {
		base.Update();
	}

	// Update is called once per frame
	protected override void Update()
    {
		base.Update();

		// Detect the Player

		if (target = null)
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
					if (weapon.isCrushingWeapon())
						enemy.SufferDamage(baseJabAttackDamage * weapon.damageModifier, DamageType.Crushing, weapon.dElement, transform.position);
					else
						enemy.SufferDamage(baseJabAttackDamage * weapon.damageModifier, DamageType.Piercing, weapon.dElement, transform.position);
				}
			}
		}
	}

	public override void Die()
	{
		Destroy(gameObject);
	}
}
