using UnityEngine;
using System.Collections;
using SquarelandSystem;

public class CameraLookAtPlayer : MonoBehaviour {
	
	protected Route route;
	
	protected GameObject player;

	public static Transform pathObjectToLookAt = null;

	// Use this for initialization
	void Start () {
		Squareland squareland = (Squareland) Controller.procedure.GetCurrentTrial().GetCurrentCommand();
		route = squareland.route;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void FixedUpdate () {
		//if (route.currentPathObject > 0) {
		transform.position = player.transform.position;
		if (pathObjectToLookAt != null) {
			if (SquarelandScene.movingForward == true) {
				transform.rotation = Quaternion.Slerp(
					transform.rotation,
					Quaternion.LookRotation(pathObjectToLookAt.position - transform.position), 
					6.0f * Time.deltaTime
				);
			} else {
				transform.rotation = Quaternion.Slerp(
					transform.rotation,
					Quaternion.LookRotation(pathObjectToLookAt.position - transform.position), 
					6.0f * Time.deltaTime
				);
			}
		}
		//}
	}
}
