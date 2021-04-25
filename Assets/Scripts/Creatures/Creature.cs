using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
	public float health = 10.0f;
	public float maxHealth = 10.0f;
	public float tempHealth = 0.0f;
	public float tempHealthLoss = 0.05f;
	public Foible earthF = Foible.Regular;
	public Foible airF = Foible.Regular;
	public Foible fireF = Foible.Regular;
	public Foible waterF = Foible.Regular;
	public Foible physicalF = Foible.Regular;
	public Foible spiritF = Foible.Regular;
	public Foible slashingF = Foible.Regular;
	public Foible piercingF = Foible.Regular;
	public Foible crushingF = Foible.Regular;
	public Foible conjuringF = Foible.Regular;

	public Color bloodColour = Color.red;


	// Start is called before the first frame update
	protected override void Start()
    {
		if (health <= 0.0f)
			Die();
	}

	// Update is called once per frame
	protected override void Update()
    {
		health = Mathf.Max(Mathf.Min(health, maxHealth),0);
		tempHealth = Mathf.Max(Mathf.Min(maxHealth - health, tempHealth - (tempHealthLoss * Time.deltaTime)),0);
    }

	public virtual void Die()
	{
		Destroy(gameObject);
	}

	public void SufferDamage(float damage, DamageType dType, DamageElement dElement, Vector3 origin)
	{
		float multiplier = TypeResistance(dType);
		multiplier *= ElementResistance(dElement);

		Color damageNumberColour = Color.red;
		if (multiplier > 1.0f)
			damageNumberColour = Color.magenta;
		if (multiplier < 1.0f)
			damageNumberColour = Color.yellow;

		UI.inst.SpawnDamageNumbers(Mathf.CeilToInt(damage), damageNumberColour, transform.position + new Vector3(0.5f, 1.5f, 0f));
		UI.inst.SpawnHealthbar(this);

		for(int i = 0; i < 30; i++)
			Particles.inst.Emit(transform.position + Vector3.up, 3f * ((transform.position - origin).normalized + Random.insideUnitSphere), 1.0f, 3.0f, bloodColour, Particles.Type.BLOOD, 1);

		damage *= multiplier;

		if (damage <= 0)
		{
			return;
		}

		if (tempHealth >= 0)
		{
			if (damage >= tempHealth)
			{
				tempHealth = 0.0f;
				damage -= tempHealth;
			}
		}

		if (damage >= health)
		{
			health = 0.0f;
			Die();
		}
		else
		{
			health -= damage;
		}
	}

	float TypeResistance(DamageType dType)
	{
		switch (dType)
		{
			case DamageType.Slashing:
				return slashingF.DamageMultiplier();
			case DamageType.Piercing:
				return piercingF.DamageMultiplier();
			case DamageType.Crushing:
				return crushingF.DamageMultiplier();
			case DamageType.Conjuring:
				return conjuringF.DamageMultiplier();
		}

		return 1.0f;
	}

	float ElementResistance(DamageElement dType)
	{
		switch (dType)
		{
			case DamageElement.Earth:
				return earthF.DamageMultiplier();
			case DamageElement.Air:
				return airF.DamageMultiplier();
			case DamageElement.Fire:
				return fireF.DamageMultiplier();
			case DamageElement.Water:
				return waterF.DamageMultiplier();
			case DamageElement.Physical:
				Player player = GetComponent<Player>();
				if (player != null && (player.inventory.GetSpellInSlot(0).GetShieldActive() || player.inventory.GetSpellInSlot(1).GetShieldActive()) && physicalF != Foible.Immune)
					return Foible.Resistant.DamageMultiplier();
				else
					return physicalF.DamageMultiplier();
			case DamageElement.Spirit:
				return spiritF.DamageMultiplier();
		}

		return 1.0f;
	}
}
