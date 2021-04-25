using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAnimations : MonoBehaviour
{
	public Transform body;

	public enum WeaponAnim
	{
		NONE,
		SWING,
		STAB,
	}

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

	// Weapon anim
	public Transform weaponTransform;

	private WeaponAnim currentAnim = WeaponAnim.NONE;
	private Vector3 directionOfAnim = Vector3.zero;
	private float weaponAnimTime = 0.0f;
	private float weaponAnimLength = 1.0f;
	private Vector3 aimDirection = Vector3.zero;

	public void PlayAnim(WeaponAnim type, Vector3 direction, float duration)
	{
		currentAnim = type;
		weaponAnimTime = 0.0f;
		weaponAnimLength = duration;
		directionOfAnim = direction;
	}

	public void SetAimDirection(Vector3 direction)
	{
		aimDirection = direction;
	}

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

		if(currentAnim != WeaponAnim.NONE)
		{
			weaponAnimTime += Time.deltaTime;

			if (weaponAnimTime >= weaponAnimLength)
				currentAnim = WeaponAnim.NONE;
		}

		switch(currentAnim)
		{
			case WeaponAnim.NONE:
			{
				weaponTransform.localPosition = aimDirection;
				break;
			}
			case WeaponAnim.STAB:
			{
				float t = weaponAnimTime / weaponAnimLength;

				weaponTransform.localPosition = directionOfAnim * (1.0f + Mathf.Sin(t * Mathf.PI) * 1.0f);


				break;
			}
		}
	}
}
