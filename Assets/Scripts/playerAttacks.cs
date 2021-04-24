using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttacks : MonoBehaviour
{
	public float sweepAttackRange = 5.0f;
	public float attackCooldown = 50.0f;

	float attackCountDown = 0.0f;


	// Start is called before the first frame update
	void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
    {

		if (attackCountDown <= 0.1f) {
			if (Input.GetKey(KeyCode.Space))
			{
				SweepAttack();
				attackCountDown = attackCooldown;
			}
		}

		attackCountDown = Mathf.Max(attackCountDown - Time.deltaTime, 0.0f);
	}

	void SweepAttack()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, sweepAttackRange);

		foreach (Collider victim in hitColliders)
		{
			Debug.Log("You hit " + victim.name);
		}
	}
}
