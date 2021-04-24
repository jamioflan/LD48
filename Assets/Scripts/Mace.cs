using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : Weapon
{
	public override float cooldownModifier() { return 1.25f; }

	public override bool isCrushingWeapon() { return true; }

	public override float jabAttackRangeModifier() { return 0.75f; }

	public override float sweepAttackRangeModifier() { return 1.25f; }
}
