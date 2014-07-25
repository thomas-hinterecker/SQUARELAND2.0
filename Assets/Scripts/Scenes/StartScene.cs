using UnityEngine;
using System.Collections;
using SquarelandSystem;
/**
 * Start Scene of SQUARELAND 2.0.
 * Shows the departement logo and loads the first trail item afterwards.
 */
public class StartScene : MonoBehaviour {
	
	protected int startAfter = 1500;
	
	void Start () {
		Screen.showCursor = false;

		// Load the prerequired stuff (like the XML file).
		Controller.StartSquareland();
	}
	
	void Update () {
		if (Time.timeSinceLevelLoad * 1000 > startAfter) {
			Controller.procedure.GetCurrentTrial().LoadNextCommand();
		}
	}
}
