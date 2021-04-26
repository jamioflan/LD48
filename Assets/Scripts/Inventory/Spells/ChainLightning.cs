using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Spell
{
	public LineRenderer lr;

	public float distanceToFirstTarget = 1.0f;
	public float distanceBetweenTargets = 1.0f;

	private float lineRenderTime = 0.0f;

	// Start is called before the first frame update
	protected override void Start()
    {
		base.Start();
		lr = GetComponent<LineRenderer>();
		lr.enabled = false;
		cooldownTimer = 3.0f;

	}

	protected override void Update()
	{
		base.Update();
		if (lineRenderTime > 0.0f)
			lineRenderTime -= Time.deltaTime;

		lr.enabled = lineRenderTime > 0.0f;
	}

	public override void CastSpell(Vector3 target)
	{
		if (countDown <= 0.0f && isPlayerCaster)
		{
			List<Enemy> alreadyZapped = new List<Enemy>();
			foreach(Collider collider in Physics.OverlapSphere(owner.transform.position, distanceToFirstTarget))
			{
				Enemy enemy = collider.GetComponent<Enemy>();
				if (enemy != null)
				{
					alreadyZapped.Add(enemy);
					Zap(enemy, alreadyZapped);
					break;
				}
			}

			lr.positionCount = alreadyZapped.Count * 2 + 1;
			Vector3[] positions = new Vector3[alreadyZapped.Count * 2 + 1];
			positions[0] = owner.transform.position + Vector3.up;
			for(int i = 0; i < alreadyZapped.Count; i++)
			{
				positions[i * 2 + 1] = 0.5f * (positions[i * 2] + alreadyZapped[i].transform.position + Vector3.up) + Random.insideUnitSphere * 0.5f;
				positions[i * 2 + 2] = alreadyZapped[i].transform.position + Vector3.up;
			}

			lr.SetPositions(positions);
			lineRenderTime = 0.2f;

			countDown = cooldownTimer;
		}
	}

	private void Zap(Enemy enemy, List<Enemy> alreadyZapped)
	{
		enemy.SufferDamage(spellDamage, DamageType.Conjuring, DamageElement.Air, owner.transform.position);

		foreach (Collider collider in Physics.OverlapSphere(enemy.transform.position, distanceBetweenTargets))
		{
			Enemy nextEnemy = collider.GetComponent<Enemy>();
			if(nextEnemy != null && !alreadyZapped.Contains(nextEnemy))
			{
				alreadyZapped.Add(nextEnemy);
				Zap(nextEnemy, alreadyZapped);
				break;
			}
		}
	}

}
