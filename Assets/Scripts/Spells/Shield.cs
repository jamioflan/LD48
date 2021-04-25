using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Spell
{
	bool shieldActive = false;
	float shieldDuration = 2.5f;
	float shieldCountDown = 0.0f;

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		cooldownTimer = 5.0f;
		countDown = 0.0f;
	}

	// Update is called once per frame
	protected override void Update()
    {
		base.Update();

		shieldCountDown = Mathf.Max(shieldCountDown - Time.deltaTime, 0.0f);

		if (shieldActive && shieldCountDown <= 0)
			shieldActive = false;
	}

	public override void CastSpell(Vector3 target)
	{
		if (countDown <= 0.0f)
		{
			shieldActive = true;
			shieldCountDown = shieldDuration;
			countDown = cooldownTimer;
		}
	}

	public override bool GetShieldActive()
	{
		return shieldActive;
	}
}