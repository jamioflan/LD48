using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
	public int treasure = 1;
	public float detectionRange = 10.0f;
	public float moveSpeed = 2.0f;
	protected Player target = null;

	// Start is called before the first frame update
	protected override void Start()
    {
		base.Update();
	}

	// Update is called once per frame
	protected override void Update()
    {
		base.Update();

		// Detect the Player

		if (target = null)
		{
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, 1 << LayerMask.NameToLayer("Player"));

			foreach (Collider victim in hitColliders)
			{
				Player player = victim.GetComponent<Player>();

				if (player != null)
				{
					target = player;
					return;
				}
			}
		}
	}

	public override void Die()
	{
		Destroy(gameObject);
	}
}
