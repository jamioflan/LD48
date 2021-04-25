using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 1.0f;
	public float sprintModifier = 1.5f;
	public Vector3 targetPosition = Vector3.zero;
	public Transform mouseCursor = null;

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

		anims.SetMoving(motionVector.magnitude > 0.01f);

		motionVector *= (moveSpeed * 10);

		if (Input.GetKey(KeyCode.LeftShift))
			motionVector *= sprintModifier;

		motionVector.y = -1f;

		motionVector *= Time.deltaTime;
		controller.Move(motionVector);
	}

	public Vector3 targetDirection
	{
		get
		{
			return (targetPosition - transform.position).normalized;
		}
	}
}
