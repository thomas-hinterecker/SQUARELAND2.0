using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SquarelandSystem;

public class ResponseListener : MonoBehaviour {
	
	protected Trial trial = null;

	/*public List<KeyCode> keyCode = new List<KeyCode>();

	public Trial trial = null;

	void Start () {
		trial = Main.procedure.GetCurrentTrial();
	}
	
	void OnGUI() {
		Event e = Event.current;
		Event e_down = Event.current;

		if (e.isKey) {
			if (e.type == EventType.KeyDown && e.keyCode.ToString() != "None") {
				if (false == keyCode.Contains(e.keyCode)) {
					Debug.Log("Key pressed: " + e.keyCode.ToString() + " (" + e.keyCode + ")");
					trial.responses_keyboard.Add(new Response(e.keyCode.ToString()));
					keyCode.Add(e.keyCode);
					e_down = e;
				}
			} else if (e.type == EventType.KeyUp && e.keyCode == e_down.keyCode) {
				keyCode.Remove(e.keyCode);
				//Debug.Log("Key up: " + e.keyCode.ToString() + " (" + e.keyCode + ")");
			}
		}
	}
	
	void Update () {
		Main.timeSinceStart += Time.deltaTime;
	}*/

	protected void responseListener () {

		bool horizontal_input = Input.GetButtonDown("Horizontal");
		bool vertical_input = Input.GetButtonDown("Vertical");

		float horizontal_axis = Input.GetAxisRaw("Horizontal");
		float vertical_axis = Input.GetAxisRaw("Vertical");

		if (true == horizontal_input && horizontal_axis > 0) {
			Debug.Log("Key pressed: Right ");
			trial.responses.Add(new Response("Right"));
		} else if (true == horizontal_input && horizontal_axis < 0) {
			Debug.Log("Key pressed: Left ");
			trial.responses.Add(new Response("Left"));
		} else if (true == vertical_input && vertical_axis > 0) {
			Debug.Log("Key pressed: Up ");
			trial.responses.Add(new Response("Up"));
		} else if (true == vertical_input && vertical_axis < 0) {
			Debug.Log("Key pressed: Down ");
			trial.responses.Add(new Response("Down"));
		} else if (true == Input.GetButtonDown("Continue")) {
			Debug.Log("Key pressed: Continue ");
			trial.responses.Add(new Response("Continue"));
		} else if (true == Input.anyKeyDown) {
			Debug.Log("Key pressed: AnyKey");
			trial.responses.Add(new Response("AnyKey"));
		}
	}
}
