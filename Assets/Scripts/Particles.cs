using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
	public static Particles inst;

	public ParticleSystem blood,
		fire,
		water,
		rock,
		spirit
		;



	public enum Type
	{
		BLOOD,
		FIRE,
		WATER,
		ROCK,
		SPIRIT,
	}

	public void Awake()
	{
		inst = this;
	}

	public ParticleSystem Get(Type type)
	{
		switch(type)
		{
			case Type.BLOOD: return blood;
			case Type.FIRE: return fire;
			case Type.WATER: return water;
			case Type.ROCK: return rock;
			case Type.SPIRIT: return spirit;
		}
		return null;
	}

	public void Emit(Vector3 pos, Vector3 motion, float size, float duration, Color colour, Type type, int count)
	{
		ParticleSystem particles = Get(type);
		ParticleSystem.EmitParams par = new ParticleSystem.EmitParams()
		{
			position = pos,
			velocity = motion,
			startColor = colour,
			startLifetime = duration,
			startSize = size,
		};
		particles.Emit(par, count);
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
