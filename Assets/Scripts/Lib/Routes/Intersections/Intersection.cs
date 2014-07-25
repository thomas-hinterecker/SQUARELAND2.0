using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class Intersection {
		
		public List<Landmark> landmarks = new List<Landmark>(); 
		
		public string waypoint = "";
		
		public Overlay overlayTimeStop;
		
		public Overlay overlayDecisionStop;
		
		public Intersection () {

		}
		
		public Intersection (string given_waypoint) {
			waypoint = given_waypoint.Replace(";", "_");
		}
		
		public void AddLandmark (Landmark landmark) {
			landmarks.Add(landmark);
		}
	}
}
