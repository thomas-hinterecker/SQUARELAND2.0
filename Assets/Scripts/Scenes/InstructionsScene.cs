using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SquarelandSystem;

public class InstructionsScene : ResponseListener {
	
	public GameObject InstructionsGuiTexture;
	
	public GameObject InstructionsGuiText;

	protected Instructions instructions;
	
	protected int currentInstruction = 0;
	
	protected float playerResponseTime = 0.0f;
	
	protected Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();
	
	protected Dictionary<int, string> texts = new Dictionary<int, string>();
	
	protected InstructionsSetting instructionsSetting;

	protected bool setEvent = true;

	void Start () {
		ImageInstruction image_instruction;
		TextInstruction text_instruction;

		instructionsSetting = (InstructionsSetting) Controller.settings["instructions"];

		trial = Controller.procedure.GetCurrentTrial();

		instructions = (Instructions) trial.GetCurrentCommand();

		for (int i = 0; i < instructions.instructions.Count; ++i) {
			if (instructions.instructions[i] is ImageInstruction) {
				image_instruction = (ImageInstruction) instructions.instructions[i];
				if (image_instruction.file != "") {
					byte[] textureData = File.ReadAllBytes(image_instruction.file);
					Texture2D texture = new Texture2D(0,0);
					texture.LoadImage(textureData);
					textures.Add(i, texture);
				}
			} else {
				text_instruction = (TextInstruction) instructions.instructions[i];
				texts.Add(i, text_instruction.text);
			}
		}
		Screen.showCursor = false;
	}
	
	void Update () {
		Controller.timeSinceStart += Time.deltaTime;

		int instructions_count = instructions.instructions.Count;
		float min_duration = 0.0f;

		if (instructions_count > 0) {

			if (instructions.instructions[currentInstruction] is ImageInstruction) {
				InstructionsGuiTexture.guiTexture.texture = textures[currentInstruction];
				InstructionsGuiTexture.guiTexture.enabled = true;
			} else {
				InstructionsGuiText.guiText.text = texts[currentInstruction];
				InstructionsGuiText.guiText.enabled = true;
			}

			if (true == setEvent) {
				setEvent = false;
				trial.responses.SetTimeRecorder();
				trial.responses.NextEvent("Instruction " + (currentInstruction + 1).ToString());
			}

			if (instructions.instructions[currentInstruction].minDuration >= 0) {
				min_duration = instructions.instructions[currentInstruction].minDuration;
			} else {
				min_duration = instructionsSetting.minDuration;
			}

			if (true == Input.GetButtonDown("Continue") && playerResponseTime * 1000 > min_duration) {
				setEvent = true;

				Debug.Log("Key pressed: Continue");
				trial.responses.Add(new Response("Continue"));
				trial.responses.ClearEventData();

				InstructionsGuiTexture.guiTexture.enabled = false;
				InstructionsGuiText.guiText.enabled = false;
				++currentInstruction;
				playerResponseTime = 0.0f;
				
				if (currentInstruction == instructions_count) {
					Controller.procedure.GetCurrentTrial().LoadNextCommand();
				}
			} else {
				responseListener();
			}
			playerResponseTime += Time.deltaTime;
		}
	}
}
