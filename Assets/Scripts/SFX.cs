using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFX : MonoBehaviour
{
	public AudioMixerSnapshot boss, game, shop;
	public AudioMixer mixer;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(Game.inst.state)
		{
			case Game.State.CHOOSE_CHARACTER:
			case Game.State.IN_SHOP:
			{
				mixer.TransitionToSnapshots(new AudioMixerSnapshot[] { shop }, new float[] { 1f }, 1f);
				break;
			}
			case Game.State.IN_LEVEL:
			{
				if(LevelGenerator.inst.currentLevel.boss != null)
				{
					mixer.TransitionToSnapshots(new AudioMixerSnapshot[] { boss }, new float[] { 1f }, 1f);
				}
				else
				{
					mixer.TransitionToSnapshots(new AudioMixerSnapshot[] { game }, new float[] { 1f }, 1f);
				}
				break;
			}
		}

    }
}
