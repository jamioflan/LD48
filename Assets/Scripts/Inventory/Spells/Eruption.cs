using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eruption : Spell
{
	float eruptionRadius = 1.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
		base.Start();
		cooldownTimer = 5.0f;
		countDown = 0.0f;
	}

	public override void CastSpell(Vector3 target)
	{
		if (countDown <= 0.0f && (isPlayerCaster || isEnemyCaster))
		{
			if (isPlayerCaster)
			{
				Collider[] hitColliders = Physics.OverlapCapsule(transform.position, target, eruptionRadius, 1 << LayerMask.NameToLayer("Enemy"));

				foreach (Collider victim in hitColliders)
				{
					Enemy enemy = victim.GetComponent<Enemy>();

					if (enemy is null)
					{
						Debug.Log("You failed to disturb " + victim.name);
					}
					else
					{
						Debug.Log("You disturbed " + victim.name + " with damage " + spellDamage);
						enemy.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Earth, transform.position);
					}
				}

				countDown = cooldownTimer;
			}

			if (isEnemyCaster)
			{
				Collider[] hitColliders = Physics.OverlapCapsule(transform.position, target, eruptionRadius, 1 << LayerMask.NameToLayer("Player"));

				foreach (Collider victim in hitColliders)
				{
					Player player = victim.GetComponent<Player>();

					if (player is null)
					{
						Debug.Log("The failed to disturb " + victim.name);
					}
					else
					{
						Debug.Log("The Enemy disturbed " + victim.name + " with damage " + spellDamage);
						player.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Earth, transform.position);
					}
				}

				countDown = cooldownTimer;
			}

			owner.transform.position.Set(target.x, owner.transform.position.y, target.z);
		}
	}
}
