using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Spell
{
	public float radius = 1.0f;

	public override void CastSpell(Vector3 target)
	{
		if (countDown <= 0.0f && (isPlayerCaster || isEnemyCaster))
		{
			Collider[] hitColliders = Physics.OverlapSphere(target, radius);

			foreach (Collider victim in hitColliders)
			{
				Player player = victim.GetComponent<Player>();
				Enemy enemy = victim.GetComponent<Enemy>();

				if (isPlayerCaster && enemy != null)
				{
					Debug.Log("You immolated " + enemy.name + " with damage " + spellDamage);
					enemy.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Fire, target);
					countDown = cooldownTimer;
				}
				else if (isEnemyCaster && player != null)
				{
					Debug.Log("The Enemy immolated " + player.name + " with damage " + spellDamage);
					player.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Fire, target);
					countDown = cooldownTimer;
				}
			}

		}
	}
}
