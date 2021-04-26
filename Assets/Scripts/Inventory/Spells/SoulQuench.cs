using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulQuench : Spell
{
	public float range = 1.0f;

	public float healAmount = 1.0f;
	public float healAmountPerLevel = 1.0f;

	public override void Scale(int level)
	{
		base.Scale(level);
		healAmount += level * healAmountPerLevel;
	}

	protected override void Start()
    {
		base.Start();
		cooldownTimer = 2.0f;
    }

	public override void CastSpell(Vector3 target)
	{
		if (countDown > 0.0f)
			return;

		countDown = cooldownTimer;

		bool hitSomething = false;
		foreach(Collider collider in Physics.OverlapSphere(target, range, 1 << LayerMask.NameToLayer("Enemy")))
		{
			Enemy enemy = collider.GetComponent<Enemy>();
			if(enemy != null)
			{
				enemy.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Spirit, owner.transform.position);
				hitSomething = true;

				for (int i = 0; i < 30; i++)
				{
					Vector3 origin = enemy.transform.position + Vector3.up * 0.5f + Random.insideUnitSphere * 2.0f;
					Particles.inst.Emit(origin, 2.0f * (owner.transform.position + Vector3.up * 0.5f - origin), 0.3f, Random.Range(0.3f, 1.0f), Color.white, Particles.Type.SPIRIT, 1);
				}
			}
		}

		if (hitSomething && owner is Player player)
		{
			player.Heal(healAmount);
		}

	}

}
