using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eruption : Spell
{
	float cooldownTimer = 5.0f;
	float countDown = 0.0f;
	float eruptionRadius = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		countDown = Mathf.Max(countDown - Time.deltaTime, 0.0f);
	}

	public override void castSpell(Vector3 target)
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
						enemy.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Earth);
					}
				}
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
						player.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Earth);
					}
				}
			}

			caster.transform.position.Set(target.x, caster.transform.position.y, target.z);
		}
	}
}
