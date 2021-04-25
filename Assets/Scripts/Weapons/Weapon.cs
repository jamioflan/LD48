using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : InventoryItem
{
	public DamageElement dElement = DamageElement.Physical;
	public float damageModifier = 1.0f;
	public float damageModifierPerLevel = 0.5f;

	public abstract bool isCrushingWeapon();
	public abstract float cooldownModifier();
	public abstract float sweepAttackRangeModifier();
	public abstract float jabAttackRangeModifier();

	public override void Scale(int level)
	{
		damageModifier += damageModifierPerLevel * level;
	}

	public override string GetDescription()
	{
		return $"{damageModifier} {dElement.ToString()} damage";
	}
}
