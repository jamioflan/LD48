using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
	public float moveSpeed = 1.0f;
	public float sprintModifier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		CharacterController controller = GetComponent<CharacterController>();

		Vector3 motionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		motionVector = motionVector * moveSpeed * 10;

		if (Input.GetKey(KeyCode.LeftShift))
			motionVector = motionVector * sprintModifier;

		motionVector = motionVector * Time.deltaTime;

		controller.Move(motionVector);
    }
}
