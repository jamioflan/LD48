using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : InventoryItem
{
	public float spellDamage = 1.0f;
	public float spellDamagePerLevel = 0.5f;
	protected bool isPlayerCaster;
	protected bool isEnemyCaster;
	protected float cooldownTimer;
	protected float countDown;
	Player playerCaster = null;
	Enemy enemyCaster = null;
	public string description = "Deals {0} damage to a target";

	public abstract void CastSpell(Vector3 target);

	public override void Scale(int level)
	{
		spellDamage += spellDamagePerLevel * level;
	}

	void Start()
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
		
	public override string GetDescription()
	{
		return string.Format(description, spellDamage);
	}
}
