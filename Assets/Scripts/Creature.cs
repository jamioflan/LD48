using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creature : MonoBehaviour
{
	public float health = 10.0f;
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

	// Start is called before the first frame update
	void Start()
    {
		if (health <= 0.0f)
			Die();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public virtual void Die()
	{
		Destroy(gameObject);
	}

	public void SufferDamage(float damage, DamageType dType, DamageElement dElement)
	{
		float multiplier = TypeResistance(dType);
		multiplier *= ElementResistance(dElement);

		Color damageNumberColour = Color.red;
		if (multiplier > 1.0f)
			damageNumberColour = Color.magenta;
		if (multiplier < 1.0f)
			damageNumberColour = Color.yellow;

		UI.inst.SpawnDamageNumbers(Mathf.CeilToInt(damage), damageNumberColour, transform.position + new Vector3(0.5f, 1.5f, 0f));

		damage *= multiplier;

		if (damage >= health)
		{
			health = 0.0f;
			Die();
		}
		else
			health -= damage;
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
				return physicalF.DamageMultiplier();
			case DamageElement.Spirit:
				return spiritF.DamageMultiplier();
		}

		return 1.0f;
	}
}