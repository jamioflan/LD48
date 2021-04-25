using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageElement
{
	Earth,
	Air,
	Fire,
	Water,
	Physical,
	Spirit
}

public enum DamageType
{
	Slashing,
	Piercing,
	Crushing,
	Conjuring
}

public enum Foible
{
	Vulnerable,
	Regular,
	Resistant,
	Immune
}

public static class FoibleExt
{
	public static string ItemPrefix(this DamageElement e)
	{
		switch(e)
		{
			case DamageElement.Air: return "Lofty ";
			case DamageElement.Earth: return "Rock-Hewn ";
			case DamageElement.Fire: return "Ignited ";
			case DamageElement.Physical: return "";
			case DamageElement.Spirit: return "Haunted ";
			case DamageElement.Water: return "Soaked ";
		}
		return "";		
	}

	public static float DamageMultiplier(this Foible f)
	{
		switch (f)
		{
			case Foible.Vulnerable:
				return 2.0f;
			case Foible.Regular:
				return 1.0f;
			case Foible.Resistant:
				return 0.5f;
			case Foible.Immune:
				return 0.0f;
		}

		return 1.0f;
	}
}