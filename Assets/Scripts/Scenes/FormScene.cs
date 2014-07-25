using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SquarelandSystem;

public class FormScene : MonoBehaviour {
	
	protected string participant_id = "";
	
	protected int inputFieldNum = 0;

	protected List<FormColumn> columns = new List<FormColumn>();

	protected List<string> inputFieldTypes = new List<string>();
	
	protected string[] inputFieldTextContents;
	
	protected bool[] inputFieldBoolContents;
	
	protected string[] inputFieldNames;
	
	protected Form form;
	
	protected bool rectOnly = false;

	protected Rect guiRect;

	protected int currColumn = 0;

	protected float columnX = 0;

	protected int inputFieldsCount = 0;

	void Start () {
		FormColumn column;

		Screen.showCursor = true;
		form = (Form)  Controller.procedure.GetCurrentTrial().GetCurrentCommand();
		
		if (form.type != "setup") {
			FormInputField field;
			for (int i = 0; i < form.components.Count; ++i) {
				column = (FormColumn) form.components[i];
				columns.Add(column);
				for (int z = 0; z < column.components.Count; ++z) {
					if (column.components[z] is FormInputField) {
						++inputFieldNum;
						field = (FormInputField) column.components[z];
						inputFieldTypes.Add(field.type);
					}
				}
			}
			
			inputFieldTextContents = new string[inputFieldNum];
			inputFieldBoolContents = new bool[inputFieldNum];
			
			inputFieldNames = new string[inputFieldNum];
			
			for (int i = 0; i < inputFieldNum; ++i) {
				inputFieldNames[i] = inputFieldTextContents[i] = "";
				inputFieldBoolContents[i] = false;
			}
		}
	}
	
	protected void OnGUI() {
		if (form.type == "setup") {
			Setup();
		} else {
			FormCreator();
		}
	}
	
	protected void FormCreator () {
		rectOnly = true;
		MakeGUI(0);
		rectOnly = false;

		GUI.Window(0, guiRect, MakeGUI, form.form_name);
	}

	protected void MakeGUI (int id) {
		float y = 0;
		float x = 20;
		float columnHeight = 0;
		float formHeight = 0;
		float columnWidth = 0;
		float formWidth = columnHeight;
		int height_add = 0, width_add = 0;
		List<FormComponent> components;

		inputFieldsCount = 0;

		for (int i = 0; i < columns.Count; ++i) {
			y = 30;
			columnHeight = y;
			columnWidth = 500.0f + 20*2;

			components = columns[i].components;
			for (int z = 0; z < components.Count; ++z) {
				if (components[z] is FormText) {
					FormText text = (FormText) components[z];
					if (text.height == -1) {
						text.height = 40;
					}
					if (text.width == -1) {
						text.width = 500;
					}
					if (rectOnly == false) {
						GUI.Label(new Rect(x, y, text.width, text.height), text.text);
					}
					y += text.height + 15;
					columnHeight += text.height + 15;
					if (text.width > columnWidth) {
						columnWidth = text.width + 20*2;
					}
				} else if (components[z] is FormSubmit) {
					FormSubmit submit = (FormSubmit) components[z];
					if (submit.height == -1) {
						submit.height = 30;
					}
					if (submit.width == -1) {
						submit.width = 50;
					}
					if (rectOnly == false) {
						if (GUI.Button(new Rect(x, y, submit.width, submit.height), submit.label)) {
							WriteFormInputFile();
							Controller.procedure.GetCurrentTrial().LoadNextCommand();
						}
					}
					y += submit.height + 15;
					columnHeight += submit.height + 15;
					if (submit.width > columnWidth) {
						columnWidth = submit.width + 20*2;
					}
				} else {
					FormInputField input_field = (FormInputField) components[z];

					if (input_field.width == -1) {
						input_field.width = 500;
					}
					height_add = 0;
					width_add = 0;
					switch (input_field.type) {
					case "text":
						if (input_field.label != "") {
							if (rectOnly == false) {
								InputLabel(x, y, input_field.label);
							}
							width_add = 150;
						}
						if (input_field.height == -1) {
							input_field.height = 20;
						}
						height_add = 2;
						if (rectOnly == false) {
							inputFieldTextContents[inputFieldsCount] = GUI.TextField(new Rect(x + width_add, y, input_field.width, input_field.height), inputFieldTextContents[inputFieldsCount]);
						}
						break;
					case "textarea":
						if (input_field.label != "") {
							if (rectOnly == false) {
								InputLabel(x, y, input_field.label);
							}
							width_add = 150;
						}
						if (input_field.height == -1) {
							input_field.height = 60;
						}
						if (rectOnly == false) {
							inputFieldTextContents[inputFieldsCount] = GUI.TextField(new Rect(x + width_add, y, input_field.width, input_field.height), inputFieldTextContents[inputFieldsCount]);
						}
						break;
					case "checkbox":
						if (input_field.height == -1) {
							input_field.height = 20;
						}
						height_add = 5;
						if (rectOnly == false) {
							inputFieldBoolContents[inputFieldsCount] = GUI.Toggle(new Rect(x, y, input_field.width, input_field.height), inputFieldBoolContents[inputFieldsCount], input_field.label);
						}
						break;
					}
					inputFieldNames[inputFieldsCount] = input_field.name;

					++inputFieldsCount;
					
					y += input_field.height + height_add + 20;
					columnHeight += input_field.height + height_add + 20;
					if ((input_field.width + width_add) > columnWidth) {
						columnWidth = input_field.width + width_add + 20*2;
					}
				}
			}
			x += columnWidth;
			formWidth += columnWidth;
			if (columnHeight > formHeight) {
				formHeight = columnHeight;
			}
		}

		if (rectOnly == true) {
			guiRect = new Rect(Screen.width/2-formWidth/2, Screen.height/2-formHeight/2, formWidth, formHeight);
		}
	}
	
	protected void InputLabel (float x, float y, string label)
	{
		GUI.Label(new Rect(x, y, 150, 40), label);
	}
	
	protected void WriteFormInputFile () {
		string text = "";
		Trial trial = Controller.procedure.GetCurrentTrial();
		
		text += "Time needed for completion:\t" + Time.timeSinceLevelLoad.ToString() + "\n\n";
		text += "Field Name\tValue";
		
		for (int i = 0; i < inputFieldNum; ++i) {
			text += "\n";
			text += inputFieldNames[i] + "\t";
			if (inputFieldTypes[i] == "checkbox") {
				text += (inputFieldBoolContents[i]).ToString() + "";
			} else {
				text += inputFieldTextContents[i] + "";
			}
		}
		
		File.WriteAllText(@"LogFiles/" + Controller.participant_id + "_t" + (trial.trialNum).ToString() + "_Form_" + form.form_name + ".txt", text);
	}
	
	protected void Setup () {
		GUI.Window(0, new Rect(Screen.width/2-200, Screen.height/2-150, 400, 130), SetupWindowContent, "Setup");
	}
	
	protected void SetupWindowContent (int id) {
		GUI.Label(new Rect(20, 50, 100, 20), "Participant Id");
		
		participant_id = GUI.TextField(new Rect(120, 50, 150, 20), participant_id);
		Controller.participant_id = participant_id;

		if (GUI.Button(new Rect(20, 90, 100, 20), "Start")) {
			if (participant_id != "") {
				Controller.procedure.GetCurrentTrial().LoadNextCommand();
			}
		}
	}
}
