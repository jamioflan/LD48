using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : InventoryItem
{
	public float spellDamage = 1.0f;
	protected bool isPlayerCaster;
	protected bool isEnemyCaster;
	Player playerCaster = null;
	Enemy enemyCaster = null;

	public abstract void castSpell(Vector3 target);

	void Start()
	{
		Player playerCaster = caster.GetComponent<Player>();
		Enemy enemyCaster = caster.GetComponent<Enemy>();

		isPlayerCaster = playerCaster != null;
		isEnemyCaster = enemyCaster != null;
	}
}
