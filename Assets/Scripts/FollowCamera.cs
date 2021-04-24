using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public List<Transform> players = new List<Transform>();
	public bool autoFindPlayers = false;
	public float lerpSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Transform player1 = players[0];
		if(player1 != null)
		{
			transform.position = Vector3.Lerp(transform.position, player1.position, lerpSpeed * Time.deltaTime);
		}

	}
}
