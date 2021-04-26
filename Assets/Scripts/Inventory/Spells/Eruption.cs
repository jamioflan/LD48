using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eruption : Spell
{
	float eruptionRadius = 1.0f;

	protected override void Start()
	{
		base.Start();
		cooldownTimer = 2.5f;

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

					if (enemy == null)
					{
						Debug.Log("You failed to disturb " + victim.name);
					}
					else
					{
						Debug.Log("You disturbed " + victim.name + " with damage " + spellDamage);
						enemy.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Earth, transform.position);
					}
				}

				for (int i = 0; i < 100; i++)
				{
					Particles.inst.Emit((owner.transform.position + (target - owner.transform.position) * i / 100f) + Vector3.up * 0.5f, Vector3.up * 2.0f + Random.insideUnitSphere * 4.0f, 0.3f, Random.Range(0.3f, 0.5f), Color.white, Particles.Type.ROCK, 1);
				}

				countDown = cooldownTimer;
			}

			if (isEnemyCaster)
			{
				Collider[] hitColliders = Physics.OverlapCapsule(transform.position, target, eruptionRadius, 1 << LayerMask.NameToLayer("Player"));

				foreach (Collider victim in hitColliders)
				{
					Player player = victim.GetComponent<Player>();

					if (player == null)
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
