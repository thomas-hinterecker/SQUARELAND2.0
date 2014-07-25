using UnityEngine;
using System.Collections;
using SquarelandSystem;

public class CrosslinesScene : ResponseListener {
	
	protected float time = 0.0f;
	
	protected Crosslines crosslines;
	
	void Start () {
		crosslines = (Crosslines) Controller.procedure.GetCurrentTrial().GetCurrentCommand();
		Screen.showCursor = false;
	}
	
	void Update () {
		Controller.timeSinceStart += Time.deltaTime;

		trial = Controller.procedure.GetCurrentTrial();
		trial.responses.NextEvent("Crosslines");

		responseListener();

		if (Time.timeSinceLevelLoad * 1000 >= crosslines.time) {
			Controller.procedure.GetCurrentTrial().LoadNextCommand();
		}
	}
}
