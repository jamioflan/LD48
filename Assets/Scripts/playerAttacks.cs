using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttacks : MonoBehaviour
{
	public float attackCooldown = 50.0f;
	public float sweepAttackRange = 5.0f;
	public float jabAttackRange = 10.0f;
	public float jabAttackRadius = 1.0f;

	float attackCountDown = 0.0f;
	playerMovement movement = null;

	// Start is called before the first frame update
	void Start()
    {
		movement = GetComponent<playerMovement>();
    }

	// Update is called once per frame
	void Update()
    {

		if (attackCountDown <= 0.1f) {

			if (Input.GetMouseButton(0))
			{
				JabAttack();
				attackCountDown = attackCooldown;
			}

			if (Input.GetMouseButton(1))
			{
				SweepAttack();
				attackCountDown = attackCooldown;
			}
		}

		attackCountDown = Mathf.Max(attackCountDown - Time.deltaTime, 0.0f);
	}

	void SweepAttack()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, sweepAttackRange, 1 << LayerMask.NameToLayer("Enemy"));

		foreach (Collider victim in hitColliders)
		{
			Debug.Log("You slashed " + victim.name);
		}
	}

	void JabAttack()
	{
		Collider[] hitColliders = Physics.OverlapCapsule(transform.position, transform.position + (movement.targetDirection * jabAttackRange),jabAttackRadius, 1 << LayerMask.NameToLayer("Enemy"));

		foreach (Collider victim in hitColliders)
		{
			Debug.Log("You stabbed " + victim.name);
		}
	}
}
