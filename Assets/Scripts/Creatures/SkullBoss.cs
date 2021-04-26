using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBoss : MonoBehaviour
{
	public SkullBossHand[] hands;
	public float attackInterval = 1.5f;

	public Enemy enemy;

	private float timeToNextAttack = 1.0f;
	private int nextAttackHand = 1;
	private float[] attackAnimAmount = new float[2];

    // Start is called before the first frame update
    void Start()
    {
		enemy = GetComponent<Enemy>();
		foreach (SkullBossHand hand in hands)
			hand.boss = this;
    }

    // Update is called once per frame
    void Update()
    {
		timeToNextAttack -= Time.deltaTime;

		if (timeToNextAttack <= 0.0f)
		{
			timeToNextAttack = attackInterval;
			attackAnimAmount[nextAttackHand] = 1.0f;
			hands[nextAttackHand].canAttackPlayer = true;

			// Next time, use the next hand
			nextAttackHand++;
			if (nextAttackHand >= hands.Length)
				nextAttackHand = 0;
		}

        for(int i = 0; i < hands.Length; i++)
		{
			SkullBossHand hand = hands[i];
			Vector3 restPos = transform.position + new Vector3(i % 2 == 0 ? -2f : 2f, 0f, 0f);

			if(enemy.target != null)
			{
				restPos = Vector3.Lerp(restPos, enemy.target.transform.position, attackAnimAmount[i]);
			}

			hand.transform.position = restPos;

			attackAnimAmount[i] *= 0.95f;
		}
    }
}
