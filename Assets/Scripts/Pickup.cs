using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	public enum Type
	{
		HEALTH,
		COIN,
	}

	public Type type;
	public int count;
	public Transform bobber;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		bobber.localPosition = new Vector3(0f, Mathf.Sin(Time.time) * 0.25f + 0.25f, 0f);

	}
}
