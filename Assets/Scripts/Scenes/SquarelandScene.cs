using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using SquarelandSystem;

public class SquarelandScene : ResponseListener {
	
	Squareland squareland;
	
	/*spublic GameObject blockFullLandmarkSize;
	public GameObject blockSmallerLandmarkSize;*/
	public Material cubeMaterial;
	public Material landmarkMaterial;
	public GameObject waypointToCloneObject;
	public GameObject floorObject;
	
	public static bool movingForward = true;
	
	protected MovementSetting movementSetting;
	protected OverlaysSetting overlaysSetting;
	protected HazeSetting hazeSetting;

	public Maze maze = null;
	protected Route route = null;
	
	protected Vector3 movementVelocity = Vector3.zero;
	protected bool pathPause = false;
	protected float pathPauseTime = 0.0f;
	protected GUIText pauseText;
	protected GUITexture pauseTextBackground;
	protected bool clearResponseNextUpdate = false;
	protected GUIText timeText;

	protected float horizontalInput = 0;
	protected float verticalInput = 0;

	void Awake () {
		squareland = (Squareland) Controller.procedure.GetCurrentTrial().GetCurrentCommand();

		movementSetting = (MovementSetting) Controller.settings["movement"];
		overlaysSetting = (OverlaysSetting) Controller.settings["overlays"];
		hazeSetting = (HazeSetting) Controller.settings["haze"];

		if (squareland.movementMode == "") {
			squareland.movementMode = movementSetting.mode;
		}

		maze = new Maze(cubeMaterial, landmarkMaterial, waypointToCloneObject, floorObject);
		maze.CreateMaze();

		HazeSettings();

		trial = Controller.procedure.GetCurrentTrial();

		Screen.showCursor = false;
	}

	protected void HazeSettings () {
		if (hazeSetting.enable == true) {
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.Linear;
			RenderSettings.fogStartDistance = Maze.gapBetweenBlocks + movementSetting.stopInfrontWaypoint + hazeSetting.addMeters;
			RenderSettings.fogEndDistance = RenderSettings.fogStartDistance + 0.52f;
		}
	}

	void Start () {
		LoadRoute();
		InitPlayerPosition();
		CreateGUI();
	}
	
	void OnDrawGizmos() {
		if (route != null) {
			Gizmos.color = Color.red;
			for (int i = 0; i < route.pathObjects.Count - 1; ++i) {
				Gizmos.DrawLine(route.pathObjects[i].position, route.pathObjects[i + 1].position);
			}
		}
	}

	protected void LoadRoute () {
		route = squareland.route;
		if (route != null) {
			route.LoadRoute(squareland.startFromEnd, squareland.firstIntersection, squareland.numIntersections);
		}
	}
	
	protected void InitPlayerPosition () {
		transform.position = route.pathObjects[0].transform.position;
		transform.LookAt(route.pathObjects[route.currentPathObject + 1]);
		
		Camera.main.transform.position = transform.position;
		Camera.main.transform.LookAt(route.pathObjects[1].transform);
	}
	
	protected void CreateGUI () {
		Vector3 position = new Vector3(0.5f, 0.8f, 0.0f);
		
		GameObject pauseTextObject = new GameObject("GUIText");
		pauseTextObject.transform.position = position;
		pauseTextObject.AddComponent("LookAtCameraYonly");
		pauseText = (GUIText) pauseTextObject.AddComponent(typeof(GUIText));
		pauseText.anchor = TextAnchor.MiddleCenter;
		pauseText.alignment = TextAlignment.Center;
		pauseText.enabled = false;
		pauseText.pixelOffset = new Vector2(overlaysSetting.x, overlaysSetting.y);
		pauseText.fontSize = overlaysSetting.fontSize;
		
		string[] color_splitted = overlaysSetting.fontColor.Split('.');
		float r = float.Parse(color_splitted[0]);
		float g = float.Parse(color_splitted[1]);
		float b = float.Parse(color_splitted[2]);
		pauseText.material.color = new Color(r/255.0f, g/255.0f, b/255.0f);
		
		GameObject pauseTextBackgroundObject = new GameObject("GUITextBackground");
		pauseTextBackgroundObject.transform.position = position;
		pauseTextBackgroundObject.transform.localScale = new Vector3(0, 0, 1);
		pauseTextBackgroundObject.AddComponent("LookAtCameraYonly");
		pauseTextBackground = (GUITexture) pauseTextBackgroundObject.AddComponent(typeof(GUITexture));
		pauseTextBackground.texture = new Texture2D(0, 0);
		pauseTextBackground.enabled = false;
		
		color_splitted = overlaysSetting.backgroundColor.Split('.');
		r = float.Parse(color_splitted[0]);
		g = float.Parse(color_splitted[1]);
		b = float.Parse(color_splitted[2]);
		pauseTextBackground.color = new Color(r/255.0f, g/255.0f, b/255.0f);
		
		// Time debug text
		/*position = new Vector3(0.7f, 0.9f, 0.9f);
		GameObject timeTextObject = new GameObject("TimeText");
		timeTextObject.transform.position = position;
		timeTextObject.AddComponent("LookAtCameraYonly");
		timeText = (GUIText) timeTextObject.AddComponent(typeof(GUIText));
		timeText.anchor = TextAnchor.MiddleCenter;
		timeText.alignment = TextAlignment.Center;
		timeText.color = Color.black;
		timeText.enabled = false;*/
	}
	
	void Update () {
		Controller.timeSinceStart += Time.deltaTime;

		if (false == pathPause) {
			WalkPath();
		} else {
			PathPause();
		}
	}
	
	protected void WalkPath () {
		Vector3 moveDirection = transform.TransformDirection(Vector3.forward);

		if (squareland.movementMode == "active") {
			if (Input.GetAxisRaw("Vertical") > 0) {
				if (movingForward == false) {
					movingForward = true;
					if (route.currentPathObject < route.pathObjects.Count - 1) {
						++route.currentPathObject;
					}
				}
				movementVelocity = moveDirection.normalized * movementSetting.movementSpeed * Time.deltaTime;
			} /*else if (Input.GetAxisRaw("Vertical") < 0) {
				if (movingForward == true) {
					if (route.currentPathObject > 0) {
						--route.currentPathObject;
					}
					movingForward = false;
				}
				movementVelocity = moveDirection.normalized * movementSetting.movementSpeed * Time.deltaTime;
			}*/ else {
				movementVelocity = Vector3.zero;
			}
		} else {
			movementVelocity = moveDirection.normalized * movementSetting.movementSpeed * Time.deltaTime;
		}

		Transform target = route.pathObjects[route.currentPathObject];
		Vector3 targetPosition = target.position;
		Vector3 delta = targetPosition - transform.position;
		
		float magnitude = delta.magnitude * 2;
		magnitude = (float) Math.Round(magnitude, MidpointRounding.AwayFromZero) / 2;
		if (magnitude == 0.0) {
			if (target.CompareTag("PathPause")) {

				trial.responses.SetTimeRecorder();
				trial.responses.NextEvent("Route: " + squareland.route_num + "; Intersection " + (route.currentWaypoint).ToString(), false, route.GetCurrentWaypointDescription());

				pathPause = true;
				movementVelocity = Vector3.zero; 
			} else {
				ContinueWalk();
			}
		} else {
			trial.responses.NextEvent("Route: " + squareland.route_num + "; Between Intersection " + (route.currentWaypoint-1).ToString() + " and " + (route.currentWaypoint).ToString(), true);
		}

		if (movingForward == false) {
			CameraLookAtPlayer.pathObjectToLookAt = route.pathObjects[route.currentPathObject];
		} else {
			CameraLookAtPlayer.pathObjectToLookAt = route.pathObjects[route.currentPathObject];
		}
		transform.LookAt(route.pathObjects[route.currentPathObject]);

		rigidbody.velocity = movementVelocity;
		
		//transform.position += transform.forward * Time.deltaTime * 10.0f;

		responseListener();
	}

	protected void PathPause () {
		switch (squareland.performStop) {
		case "timeStop":
			TimeStop();
			break;
		case "decisionStop":
			WaitForDecision();
			break;
		}
	}
	
	protected void TimeStop () {;
		Overlay overlay = null;
		
		if (route.timeStopOverlays.ContainsKey(route.currentWaypoint)) {
			overlay = (Overlay) route.timeStopOverlays[route.currentWaypoint];
		}
		
		if (overlay != null) {
			SetOverlay(overlay);
		}
		
		//timeText.text = Math.Round(pathPauseTime * 1000).ToString();
		//timeText.enabled = true;

		responseListener();

		if (pathPauseTime * 1000 >= squareland.timeStopDuration) {
			pathPause = false;
			pathPauseTime = 0;
			
			pauseText.enabled = false;
			pauseTextBackground.enabled = false;
			
			//timeText.enabled = false;

			++route.currentWaypoint;
			ContinueWalk();
		}
		pathPauseTime += Time.deltaTime;
	}
	
	protected void WaitForDecision () {
		Overlay overlay = null;
		pathPause = true;
		
		if (route.decisionStopOverlays.ContainsKey(route.currentWaypoint)) {
			overlay = (Overlay) route.decisionStopOverlays[route.currentWaypoint];
		}
		
		if (overlay != null) {
			SetOverlay(overlay);
		}
		
		//timeText.text = Math.Round(pathPauseTime * 1000).ToString();
		//timeText.enabled = true;

		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");

		if (squareland.decisionStopAxis == 1 && horizontalInput != 0) {
			WaitForDecisionAfterInput();
		} else if (squareland.decisionStopAxis == 2 && (horizontalInput != 0 || verticalInput > 0)) {
			WaitForDecisionAfterInput();
		} else {
			responseListener();
		}

		pathPauseTime += Time.deltaTime;
	}

	protected void WaitForDecisionAfterInput () {
		if (horizontalInput > 0) {
			Debug.Log("Key pressed: Right ");
			trial.responses.Add(new Response("Right"));
		} else if (horizontalInput < 0) {
			Debug.Log("Key pressed: Left ");
			trial.responses.Add(new Response("Left"));
		} else if (verticalInput > 0) {
			Debug.Log("Key pressed: Up ");
			trial.responses.Add(new Response("Up"));
		}
		trial.responses.ClearEventData();

		pathPause = false;
		pathPauseTime = 0;

		pauseText.enabled = false;
		pauseTextBackground.enabled = false;
		
		//timeText.enabled = false;

		++route.currentWaypoint;
		ContinueWalk();
	}

	protected void ContinueWalk () {
		if (movingForward == true && route.currentPathObject < route.pathObjects.Count - 1) {
			route.currentPathObject++;
		} else if (movingForward == false && route.currentPathObject > 0) {
			route.currentPathObject--;
		} else {
			movementVelocity = Vector3.zero;
			if (route.currentPathObject == route.pathObjects.Count - 1) {
				Controller.procedure.GetCurrentTrial().LoadNextCommand();
			}
		}
	}
	
	protected void SetOverlay (Overlay overlay) {
		float overlay_file_width = 0;
		float overlay_file_height = 0;
		
		float position_x = overlay.x;
		float position_y = overlay.y;
		
		if (overlay.file != null) {
			pauseTextBackground.texture = overlay.file;
			overlay_file_width = overlay.file.width;
			overlay_file_height = overlay.file.height;
			if (overlay.fileWidth > -1) {
				overlay_file_width = overlay.fileWidth;
			}
			if (overlay.fileHeight > -1) {
				overlay_file_height = overlay.fileHeight;
			}
			pauseTextBackground.color = Color.white;
			pauseTextBackground.pixelInset = new Rect((overlay_file_width / -2) + position_x, (overlay_file_height / -2) + position_y, overlay_file_width, overlay_file_height);
			pauseTextBackground.enabled = true;
		} else if (overlay.text != "") {
			pauseText.text = overlay.text;
			Rect rect = FormatGuiTextArea(pauseText, overlay.textMaximumWidth);
			float text_width = rect.width + 10;
			float text_height = rect.height + 15;
			pauseTextBackground.pixelInset = new Rect((text_width / -2) + position_x, (text_height / -2) + position_y, text_width, text_height);
			pauseTextBackground.enabled = true;
			pauseText.enabled = true;
		}
	}

	public static Rect FormatGuiTextArea(GUIText guiText, float maxAreaWidth) {
		string[] words = guiText.text.Split(' ');
		string result = "";
		Rect textArea = new Rect();
		
		for (int i = 0; i < words.Length; i++) {
			// set the gui text to the current string including new word
			guiText.text = (result + words[i] + " ");
			// measure it
			textArea = guiText.GetScreenRect();
			// if it didn't fit, put word onto next line, otherwise keep it
			if (textArea.width > maxAreaWidth) {
				result += ("\n" + words[i] + " ");
			} else {
				result = guiText.text;
			}
		}
		return textArea;
	}
}
