using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Creature
{
	public int treasure = 1;
	public float detectionRange = 10.0f;
	public float moveSpeed = 2.0f;
	public float attackRange = 1.0f;
	public float attackCooldownTime = 2.0f;
	public float attackDamage = 1.0f;
	public DamageType dType = DamageType.Crushing;
	public DamageElement dElement = DamageElement.Physical;

	protected Player target = null;

	float detectionCountDown = 0.0f;
	float attackCountDown = 0.0f;
	NavMeshAgent agent = null;

	// Start is called before the first frame update
	protected override void Start()
    {
		base.Update();
		agent = GetComponent<NavMeshAgent>();
		agent.speed = moveSpeed;
	}

	// Update is called once per frame
	protected override void Update()
    {
		base.Update();

		if (target == null && detectionCountDown <= 0.0f)
		{
			// Detect the Player

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

			detectionCountDown = Random.Range(0.0f, 1.0f);
		}
		else if (hitTimeCountdown > 0.0f)
		{
			agent.SetDestination(transform.position);
		}
		else if (target != null)
		{
			// Move towards the Player
			agent.SetDestination(target.transform.position);

			// Attack the Player

			if (attackCountDown <= 0.0f)
			{
				Vector3 lineToTarget = target.transform.position - transform.position;

				if (lineToTarget.magnitude <= attackRange)
				{
					attackCountDown = attackCooldownTime;
					Attack(target);
				}
			}
		}

		detectionCountDown = Mathf.Max(detectionCountDown - Time.deltaTime, 0.0f);
		attackCountDown = Mathf.Max(attackCountDown - Time.deltaTime, 0.0f);
	}

	public override void Die()
	{
		Destroy(gameObject);
	}

	public virtual void Attack (Player player)
	{
		player.SufferDamage(attackDamage,dType,dElement,transform.position);
	}
}
