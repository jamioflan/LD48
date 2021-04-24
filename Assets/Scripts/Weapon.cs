using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : InventoryItem
{
	public DamageElement dElement = DamageElement.Physical;

	public abstract bool isCrushingWeapon();
	public abstract float cooldownModifier();
	public abstract float sweepAttackRangeModifier();
	public abstract float jabAttackRangeModifier();

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
