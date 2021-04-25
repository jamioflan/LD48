using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : InventoryItem
{
	public float spellDamage = 1.0f;
	protected bool isPlayerCaster;
	protected bool isEnemyCaster;
	protected float cooldownTimer;
	protected float countDown;
	Player playerCaster;
	Enemy enemyCaster;

	public abstract void CastSpell(Vector3 target);

	protected virtual void Start()
	{
		playerCaster = owner.GetComponent<Player>();
		enemyCaster = owner.GetComponent<Enemy>();

		isPlayerCaster = playerCaster != null;
		isEnemyCaster = enemyCaster != null;
	}

	protected virtual void Update()
	{
		countDown = Mathf.Max(countDown - Time.deltaTime, 0.0f);
	}

	public virtual bool GetShieldActive()
	{
		return false;
	}
}
