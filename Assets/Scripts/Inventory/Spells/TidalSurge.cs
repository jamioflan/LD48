using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidalSurge : Spell
{
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
				Enemy[] hitEnemies = GetComponents<Enemy>();

				foreach (Enemy enemy in hitEnemies)
				{
					Debug.Log("You watered " + enemy.name + " with damage " + spellDamage);
					enemy.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Water, transform.position);
				}

				countDown = cooldownTimer;
			}

			if (isEnemyCaster)
			{
				Player[] players = GetComponents<Player>();

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