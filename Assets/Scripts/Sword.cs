using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
	public override float cooldownModifier() { return 1.0f; }

	public override bool isCrushingWeapon() { return false; }

	public override float jabAttackRangeModifier() { return 1.0f; }

	public override float sweepAttackRangeModifier() { return 1.0f; }
}
