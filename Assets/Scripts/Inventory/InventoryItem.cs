using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
	public Creature owner;
	public Texture2D texture;

	public Texture2D GetTexture()
	{
		return texture;
	}

	public string displayName;
	public string GetDescription()
	{
		return "Shiny";
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
