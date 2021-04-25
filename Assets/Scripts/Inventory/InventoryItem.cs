using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : MonoBehaviour
{
	public Creature owner;
	public Texture2D texture;
	public int level;
	public int cost;

	public Texture2D GetTexture()
	{
		return texture;
	}

	public string displayName;
	public virtual string GetDisplayName()
	{
		return $"{displayName} {ToRoman(level + 1)}";
	}
	public virtual string GetDescription()
	{
		return "Shiny";
	}

	public abstract void Scale(int level);

	protected virtual void Start()
	{
		MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
		mr.sharedMaterial = new Material(mr.sharedMaterial);
		mr.sharedMaterial.mainTexture = texture;
	}

	// Update is called once per frame
	protected virtual void Update()
    {
        
    }

	public static string ToRoman(int number)
	{
		if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
		if (number < 1) return string.Empty;
		if (number >= 1000) return "M" + ToRoman(number - 1000);
		if (number >= 900) return "CM" + ToRoman(number - 900);
		if (number >= 500) return "D" + ToRoman(number - 500);
		if (number >= 400) return "CD" + ToRoman(number - 400);
		if (number >= 100) return "C" + ToRoman(number - 100);
		if (number >= 90) return "XC" + ToRoman(number - 90);
		if (number >= 50) return "L" + ToRoman(number - 50);
		if (number >= 40) return "XL" + ToRoman(number - 40);
		if (number >= 10) return "X" + ToRoman(number - 10);
		if (number >= 9) return "IX" + ToRoman(number - 9);
		if (number >= 5) return "V" + ToRoman(number - 5);
		if (number >= 4) return "IV" + ToRoman(number - 4);
		if (number >= 1) return "I" + ToRoman(number - 1);
		throw new ArgumentOutOfRangeException("something bad happened");
	}
}
