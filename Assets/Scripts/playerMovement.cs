using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed = 1.0f;
	public float sprintModifier = 1.5f;
	public Vector3 targetPosition = Vector3.zero;
	public Transform mouseCursor = null;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		CharacterController controller = GetComponent<CharacterController>();

		// Direction

		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(mouseRay,out RaycastHit hit, float.MaxValue, 1 << LayerMask.NameToLayer("Floor")))
		{
			mouseCursor.position = hit.point;
		}

		// Movement
		Vector3 motionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		motionVector *= (moveSpeed * 10);

		if (Input.GetKey(KeyCode.LeftShift))
			motionVector *= sprintModifier;

		motionVector *= Time.deltaTime;

		controller.Move(motionVector);
    }

	public Vector3 targetDirection
	{
		get
		{
			return targetPosition.normalized;
		}
	}
}
