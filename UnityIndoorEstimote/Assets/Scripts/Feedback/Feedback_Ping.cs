using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback_Ping : FeedbackType {


	private User_AutoMove user; 

	public AudioSource source; 
	
	public AudioReverbZone reverb; 

	private bool startTimer; 

	private float timer; 

	private float play_delay = 1f; 

	private Instruction instruction; 



	void Start () 
	{
		timer = play_delay; 

		user = FindObjectOfType<User_AutoMove>(); 

		instruction = FindObjectOfType<Instruction>(); 
	}
	
	// Update is called once per frame

	float storedDistance; 
	float prevDistance; 
	
	void Update () {
		
		selected = toggle.isOn;

		if(prevDistance != storedDistance || (prevDistance == 0 && storedDistance == 0))
		{
			storedDistance = user.GetDistanceToTarget(); 
			prevDistance = storedDistance; 
		}


		if(selected == false)
		{
			Stop(); 
		} 

		if(startTimer)
		{
			timer+=Time.deltaTime; 
		}

		float delay = user.GetDistanceToTarget(); 
		
		delay = user.GetDistanceToTarget() / storedDistance;  

		delay = Mathf.Clamp(delay,.4f, delay); 

		if(timer > delay)
		{
			PlaySource(); 
			timer = 0; 
		}
	}

	public void Play()
	{

		timer = play_delay; 
		startTimer = true; 
	}

	private void PlaySource()
	{	
		if(user.TutorialOver)
		{
			Stop(); 
			return; 
		}

		if(instruction.isPlaying) return; 
		
		if(!selected)
		{
			Stop();  
		}
		
		source.Play(); 
		reverb.enabled = true; 
	}



	public void Stop()
	{
		reverb.enabled = false; 
		source.Stop(); 
		startTimer = false; 
		timer = 0;
	}
}
