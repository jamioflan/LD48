using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthbar : MonoBehaviour
{
	public Image hp;
	public Creature creature;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{
		if (creature.maxHealth > 0f)
		{
			hp.fillAmount = Mathf.Clamp01((creature.health + creature.tempHealth) / creature.maxHealth);

			hp.color = creature.tempHealth > 0f ? Color.yellow : Color.red;
		}
	}
}
