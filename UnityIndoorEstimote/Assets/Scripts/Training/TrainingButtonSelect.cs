using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingButtonSelect : MonoBehaviour {

	
	public Canvas trainingCanvas; 
	public Feedback_Voice voice; 
	public Feedback_Ping ping; 

	private Instruction instruction; 

	void Start()
	{
		instruction = FindObjectOfType<Instruction>(); 
	}


	public void OnVoiceSelect()
	{
		voice.toggle.isOn = true; 
		trainingCanvas.enabled = false; 
		instruction.PlayInstruction(InstructionType.Voice); 
	}

	public void OnPingSelect()
	{
		ping.toggle.isOn = true; 
		trainingCanvas.enabled = false; 
		instruction.PlayInstruction(InstructionType.Ping); 
	}
}
