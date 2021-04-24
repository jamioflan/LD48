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