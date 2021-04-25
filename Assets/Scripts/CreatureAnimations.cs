using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAnimations : MonoBehaviour
{
	public Transform body;

	public float walkBobLR = 0.1f;
	public float walkBobUD = 0.1f;
	public float walkAnimSpeed = 6.0f;
	public float walkWobble = 30.0f;

	public float idleBobLR = 0.07f;
	public float idleBobUD = 0.03f;
	public float idleAnimSpeed = 2.5f;
	public float idleWobble = 5.0f;

	private bool moving = false;
	private float walkAnim = 0.0f;
	private float idleAnim = 0.0f;

	public void SetMoving(bool b)
	{
		if(moving != b)
		{
			walkAnim = 0.0f;
			idleAnim = 0.0f;
		}
		moving = b;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
		{
			walkAnim += Time.deltaTime * walkAnimSpeed;
			body.localPosition = new Vector3(
				walkBobLR * -Mathf.Cos(walkAnim),
				Mathf.Abs(walkBobUD * Mathf.Sin(walkAnim)),
				0f);
			body.localEulerAngles = new Vector3(0f, 0f,
				Mathf.Cos(walkAnim) * walkWobble);
		}
		else
		{
			idleAnim += Time.deltaTime * idleAnimSpeed;
			body.localPosition = new Vector3(
				idleBobLR * -Mathf.Cos(idleAnim),
				Mathf.Abs(idleBobUD * Mathf.Sin(idleAnim)),
				0f);
			body.localEulerAngles = new Vector3(0f, 0f,
				Mathf.Cos(idleAnim) * idleWobble);
		}
    }
}
