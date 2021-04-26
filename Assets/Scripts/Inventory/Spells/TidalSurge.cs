using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidalSurge : Spell
{
	protected override void Start()
	{
		base.Start();
		cooldownTimer = 1.5f;
	}

	public override void CastSpell(Vector3 target)
	{
		if (countDown <= 0.0f && (isPlayerCaster || isEnemyCaster))
		{
			if (isPlayerCaster)
			{
				for (int i = 0; i < 100; i++)
				{
					Particles.inst.Emit(owner.transform.position + Vector3.up * 0.5f, Random.insideUnitSphere * 4.0f, 0.3f, Random.Range(0.3f, 0.5f), Color.white, Particles.Type.WATER, 1);
				}
				Enemy[] hitEnemies = FindObjectsOfType<Enemy>();

				foreach (Enemy enemy in hitEnemies)
				{
					if (enemy.hasTarget())
					{
						Debug.Log("You watered " + enemy.name + " with damage " + spellDamage);
						enemy.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Water, transform.position);
					}
				}

				countDown = cooldownTimer;
			}

			if (isEnemyCaster)
			{
				Player[] players = FindObjectsOfType<Player>();

				foreach (Player player in players)
				{
					Debug.Log("The Enemy watered " + player.name + " with damage " + spellDamage);
					player.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Water, transform.position);
				}

				countDown = cooldownTimer;
			}
		}
	}
}
