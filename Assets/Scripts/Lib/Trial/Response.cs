using UnityEngine;
using System.Collections;

namespace SquarelandSystem {

	public class Response {
		
		public int trialItem = 0;
		
		public int number = 0;
		
		public float time = 0.0f;

		public float timeSinceStart = 0.0f;
		
		public string key = "";
		
		public string eventName = "";
		
		public string eventInfo = "";
		
		public Response (string response_key) {
			key = response_key;
		}
	}
}
