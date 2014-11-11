using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SquarelandSystem;

public class LogScene : MonoBehaviour {

	void Start () {
		Screen.showCursor = false;
		Trial trial = Controller.procedure.GetCurrentTrial();
		if (WriteReponsesFile(trial.responses, "", trial) == true) { // && WriteReponsesFile(trial.responses_all, "_keyboard", trial) == true
			Controller.procedure.LoadNextTrial();
		}
	}
	
	protected bool WriteReponsesFile (Responses responses, string name_add, Trial trial) {
		string text = "";
		float response_time, time_since_start;
		
		text += "Trial Item\tTrial Item Name\tEvent Name\tEvent Info\tTime Since Start\tResponse Number\tResponse Key\tResponse Time";
		
		for (int i = 0; i < responses.responses.Count; ++i) {
			text += "\n";
			
			response_time = responses.responses[i].time * 1000.0f;
			time_since_start = responses.responses[i].timeSinceStart * 1000.0f;
			
			text += (responses.responses[i].trialItem + 1).ToString() + "\t";
			text += trial.commands[responses.responses[i].trialItem].name + "\t";
			text += "\"" + responses.responses[i].eventName + "\"\t";
			text += "\"" + responses.responses[i].eventInfo + "\"\t";
			text += time_since_start.ToString() + "\t";
			text += (responses.responses[i].number - 1).ToString() + "\t";
			text += responses.responses[i].key + "\t";
			text += response_time.ToString() + "\n";
		} 
		
		File.WriteAllText(@"LogFiles/" + Controller.participant_id + "_t" + (trial.trialNum).ToString() + "_Responses" + name_add + ".txt", text);

		return true;
	}
}
