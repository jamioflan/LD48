using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : Weapon
{
	public override float cooldownModifier() { return 2.0f; }

	public override bool isCrushingWeapon() { return false; }

	public override float jabAttackRangeModifier() { return 2.0f; }

	public override float sweepAttackRangeModifier() { return 2.0f; }
}
