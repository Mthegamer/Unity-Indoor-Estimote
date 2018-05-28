using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum InstructionType
{
	None, 
	Ping, 
	Voice,
	Complete,
}
public class Instruction : MonoBehaviour {



	public AudioClip[] clips; 

	private AudioSource source; 

	public bool isPlaying;

	void Start () 
	{
		source = GetComponent<AudioSource>();
	}

	public void PlayInstruction(InstructionType type)
	{
		source.Stop(); 

		switch(type)
		{
			case InstructionType.Ping:
			{
				source.clip = clips[0]; 
				source.Play(); 
				break; 
			}

			case InstructionType.Voice:
			{
				source.clip = clips[1]; 
				source.Play(); 
				break; 
			}
			case InstructionType.Complete: 
			{
				source.clip = clips[2]; 
				source.Play(); 
				break; 
			}
		}

		StopCoroutine("IIsPlaying"); 
		StartCoroutine("IIsPlaying"); 
	}

	IEnumerator IIsPlaying()
	{
		while(source.isPlaying)
		{
			isPlaying = true; 
			yield return null; 
		}

		isPlaying = false; 

	}
}