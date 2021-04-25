using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCapitalMovement : MonoBehaviour
{
	public float moveSpeed = 1.0f;
	public float dashModifier = 10f;
	public float dashDuration = 0.2f;
	public Vector3 targetPosition = Vector3.zero;
	public Transform mouseCursor = null;

	/*
	private bool isSprinting = false;
	private float sprintCooldown = 0.0f;
	private float sprintCoolTime = 1.0f;
	*/
	private CreatureAnimations anims;

	// Start is called before the first frame update
	void Start()
    {
		anims = GetComponent<CreatureAnimations>();

	}

    // Update is called once per frame
    void Update()
    {
		CharacterController controller = GetComponent<CharacterController>();

		// Direction
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(mouseRay, out RaycastHit hit, float.MaxValue, 1 << LayerMask.NameToLayer("Floor")))
		{
			targetPosition = hit.point;
			mouseCursor.position = hit.point;
		}

		anims.SetAimDirection(targetDirection);

		// Movement
		Vector3 motionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		/*
		if (Input.GetKey(KeyCode.LeftShift) && sprintCooldown <= 0.0f && !isSprinting)
		{
			isSprinting = true;
			sprintCooldown = sprintCoolTime;
			motionVector = targetDirection;
			motionVector *= dashModifier;
		}
		*/

		anims.SetMoving(motionVector.magnitude > 0.01f);

		motionVector *= (moveSpeed * 10);

		motionVector.y = -1f;

		motionVector *= Time.deltaTime;
		controller.Move(motionVector);

		/*
		sprintCooldown = Mathf.Max(0, sprintCooldown - Time.deltaTime);
		*/
	}

	public Vector3 targetDirection
	{
		get
		{
			return (targetPosition - transform.position).normalized;
		}
	}
}
