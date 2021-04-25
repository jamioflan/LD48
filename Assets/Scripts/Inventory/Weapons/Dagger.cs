using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
	public override float cooldownModifier() { return 0.5f; }

	public override bool isCrushingWeapon() { return false; }

	public override float jabAttackRangeModifier() { return 0.5f; }

	public override float sweepAttackRangeModifier() { return 0.5f; }
}
