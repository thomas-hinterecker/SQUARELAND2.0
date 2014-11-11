using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class Route {

		public string name = "";

		public List<Intersection> intersections = new List<Intersection>();
		
		public int currentWaypoint = 1;
		
		public int currentPathObject = 0;
		
		public List<Transform> waypoints = new List<Transform>();
		
		public List<string> waypointDescriptions = new List<string>();
		
		public Dictionary<int, Overlay> timeStopOverlays = new Dictionary<int, Overlay>();
		
		public Dictionary<int, Overlay> decisionStopOverlays = new Dictionary<int, Overlay>();
		
		public List<Transform> pathObjects = new List<Transform>();
		
		public int pathObjectsCount = 0;
		
		protected GameObject pathObjectsParent;
		
		public Route (XmlNode routeNode) {
			XmlNode intersectionNode, node;
			Intersection intersection;
			Landmark landmark;
			Overlay overlay;
			XmlAttributeCollection attrColl, routeAttrColl;
			
			routeAttrColl = routeNode.Attributes;

			if (routeAttrColl["name"] != null) {
				name = routeAttrColl["name"].Value;
			}

			if (routeAttrColl["start"] != null) {
				intersections.Add(new Intersection(routeAttrColl["start"].Value));
			}

			if (routeNode.HasChildNodes) {
				
				for (int i = 0; i < routeNode.ChildNodes.Count; i++) {
					
					intersectionNode = routeNode.ChildNodes[i];
					if (intersectionNode.Name == "intersection") {
						attrColl = intersectionNode.Attributes;
						intersection = new Intersection(attrColl["waypoint"].Value);
						
						for (int z = 0; z < intersectionNode.ChildNodes.Count; z++) {
							node = intersectionNode.ChildNodes[z];
							switch (node.Name) {
							case "landmark":
								landmark = new Landmark(node);
								intersection.AddLandmark(landmark);
								break;
							case "overlay":
								overlay = new Overlay(node);
								if (overlay.appearsAt == "timeStop") {
									intersection.overlayTimeStop = overlay;
								} else if (overlay.appearsAt == "decisionStop") {
									intersection.overlayDecisionStop = overlay;
								}
								break;
							}
						}
						intersections.Add(intersection);
					}
				}
			}
			if (routeAttrColl["end"] != null) {
				intersections.Add(new Intersection(routeAttrColl["end"].Value));
			}
		}
		
		public string GetCurrentWaypointDescription () {
			//Debug.Log(waypointDescriptions[currentWaypoint]);
			return waypointDescriptions[currentWaypoint];
		}
		
		public void LoadRoute (bool start_from_end, int first_intersection, int num_intersections) {
			GameObject waypoint, cube_fassade_1, cube_fassade_2;
			string cube_name = "", fassade_1_name = "", fassade_2_name = "", waypoint_description = "";
			Intersection intersection;
			Landmark landmark;
			
			currentWaypoint = 1;
			currentPathObject = 0;
			waypoints = new List<Transform>();
			timeStopOverlays = new Dictionary<int, Overlay>();
			decisionStopOverlays = new Dictionary<int, Overlay>();
			pathObjects = new List<Transform>();
			pathObjectsCount = 0;
			
			pathObjectsParent = new GameObject("PathObjectsParent");
			
			if (num_intersections == -1) {
				num_intersections = (intersections.Count - first_intersection) - 1;
			} else {
				num_intersections = first_intersection + num_intersections;
			}

			for (int i = first_intersection; i <= num_intersections; ++i) {
				intersection = intersections[i];
				
				waypoint = GameObject.Find("Waypoint_" + intersection.waypoint);
				waypoint_description = "Waypoint " + intersection.waypoint.Replace("_", ";") + " (";
				waypoints.Add(waypoint.transform);
				
				if (intersection.overlayTimeStop != null) {
					timeStopOverlays.Add(i, intersection.overlayTimeStop);
				}
				if (intersection.overlayDecisionStop != null) {
					decisionStopOverlays.Add(i, intersection.overlayDecisionStop);
				}
				
				if (intersection.landmarks != null && i != first_intersection && i < num_intersections) {
					for (int j = 0; j < intersection.landmarks.Count; j++) {
						landmark = intersection.landmarks[j];
						GetCubeAndFassadeNames(landmark.cube, intersection.waypoint, ref cube_name, ref fassade_1_name, ref fassade_2_name);
						waypoint_description += landmark.cube + ": ";
						
						if (landmark.type == "color") {
							cube_fassade_1 = GameObject.Find(cube_name + "/" + fassade_1_name);
							cube_fassade_1.renderer.material.color = landmark.color;
							
							cube_fassade_2 = GameObject.Find(cube_name + "/" + fassade_2_name);
							cube_fassade_2.renderer.material.color = landmark.color;
							
							waypoint_description += landmark.color.ToString();
						} else {
							cube_fassade_1 = GameObject.Find(cube_name + "/" + fassade_1_name);
							cube_fassade_1.renderer.material.mainTexture = landmark.file;
							
							cube_fassade_2 = GameObject.Find(cube_name + "/" + fassade_2_name);
							cube_fassade_2.renderer.material.mainTexture = landmark.file;
							
							waypoint_description += landmark.fileName;
						}
						cube_fassade_1.renderer.enabled = true;
						cube_fassade_2.renderer.enabled = true;
						waypoint_description += "; ";
					}
				}
				
				waypoint_description += ")";
				waypointDescriptions.Add(waypoint_description);
			}
			
			if (start_from_end == true) {
				FlipWaypoints();
			}
			
			CreatePath();
		}
		
		protected void GetCubeAndFassadeNames (string cube_value, string waypoint_value, ref string cube_name, ref string fassade_1_name, ref string fassade_2_name) {
			int cube_number_1, cube_number_2;
			string[] waypoint_value_splitted = waypoint_value.Split('_');
			
			switch (cube_value) {
			case "1":
				cube_name = "Block_" + waypoint_value_splitted[0] + "_" + waypoint_value_splitted[1];
				fassade_1_name = "LM_2";
				fassade_2_name = "LM_3";
				break;
			case "2":
				cube_number_1 = Int16.Parse(waypoint_value_splitted[1]) + 1;
				cube_name = "Block_" + waypoint_value_splitted[0] + "_" + cube_number_1.ToString();
				fassade_1_name = "LM_8";
				fassade_2_name = "LM_1";
				break;
			case "3":
				cube_number_1 = Int16.Parse(waypoint_value_splitted[0]) + 1;
				cube_name = "Block_" + cube_number_1.ToString() + "_" + waypoint_value_splitted[1];
				fassade_1_name = "LM_5";
				fassade_2_name = "LM_4";
				break;
			case "4":
				cube_number_1 = Int16.Parse(waypoint_value_splitted[0]) + 1;
				cube_number_2 = Int16.Parse(waypoint_value_splitted[1]) + 1;
				cube_name = "Block_" + cube_number_1.ToString() + "_" + cube_number_2.ToString();
				fassade_1_name = "LM_7";
				fassade_2_name = "LM_6";
				break;
			}
		}
		
		protected void FlipWaypoints () {
			List<Transform> waypoints_ = new List<Transform>();
			List<string> waypoint_descriptions_ = new List<string>();
			
			for (int i = waypoints.Count - 1; i >= 0; --i) {
				waypoints_.Add(waypoints[i]);
			}
			waypoints = waypoints_;
			
			for (int i = waypointDescriptions.Count - 1; i >= 0; --i) {
				waypoint_descriptions_.Add(waypointDescriptions[i]);
			}
			waypointDescriptions = waypoint_descriptions_;
		}
		
		protected void CreatePath () {
			Vector3 start_waypoint_position, new_start_waypoint_position, last_waypoint_position, new_last_waypoint_position;
			
			new_start_waypoint_position = start_waypoint_position = waypoints[1].transform.position;
			switch (GetApproachingDirection(1)) {
			case "north":
				new_start_waypoint_position = new Vector3(start_waypoint_position.x, start_waypoint_position.y, start_waypoint_position.z + Maze.blockWidth);
				break;
			case "south":
				new_start_waypoint_position = new Vector3(start_waypoint_position.x, start_waypoint_position.y, start_waypoint_position.z - Maze.blockWidth);
				break;
			case "west":
				new_start_waypoint_position = new Vector3(start_waypoint_position.x - Maze.blockWidth, start_waypoint_position.y, start_waypoint_position.z);
				break;
			case "east":
				new_start_waypoint_position = new Vector3(start_waypoint_position.x + Maze.blockWidth, start_waypoint_position.y, start_waypoint_position.z);
				break;
			}
			CreatePathObject(new_start_waypoint_position, "PathStart");
			
			for (int i = 1; i < waypoints.Count - 1; ++i) {
				CreatePathIntersectionObjects(i);
			}
			
			new_last_waypoint_position = last_waypoint_position = waypoints[waypoints.Count - 1].transform.position;
			switch (GetApproachingDirection(waypoints.Count - 1)) {
			case "north":
				new_last_waypoint_position = new Vector3(last_waypoint_position.x, last_waypoint_position.y, last_waypoint_position.z + Maze.blockWidth);
				break;
			case "south":
				new_last_waypoint_position = new Vector3(last_waypoint_position.x, last_waypoint_position.y, last_waypoint_position.z - Maze.blockWidth);
				break;
			case "west":
				new_last_waypoint_position = new Vector3(last_waypoint_position.x - Maze.blockWidth, last_waypoint_position.y, last_waypoint_position.z);
				break;
			case "east":
				new_last_waypoint_position = new Vector3(last_waypoint_position.x + Maze.blockWidth, last_waypoint_position.y, last_waypoint_position.z);
				break;
			}
			CreatePathObject(new_last_waypoint_position, "PathEnd");
		}
		
		protected void CreatePathIntersectionObjects (int waypount_count) {
			Transform refObj = waypoints[waypount_count];
			float radius, degrees, metres_pause;
			string approaching_direction, turn_direction;
			float degrees_incr = 2.0f;
	
			approaching_direction = GetApproachingDirection(waypount_count);
	
			radius = Maze.gapBetweenBlocks / 2;
			
			MovementSetting movementSetting = (MovementSetting) Controller.settings["movement"];
			metres_pause = (Maze.gapBetweenBlocks / 2) + movementSetting.stopInfrontWaypoint; // + Maze.blockLandmarkWidth
			
			switch (approaching_direction) {
			case "west":
				CreatePathObject(new Vector3(refObj.position.x - metres_pause, refObj.position.y, refObj.position.z), "PathPause");
				
				turn_direction = GetTurnDirection(waypount_count, approaching_direction);
				if (turn_direction == "left") {
					for (degrees = -90.0f; degrees <= 0.0f; degrees += degrees_incr) {
						CreatePathObject(PointOnCircle(radius, degrees, new Vector3(refObj.position.x - radius, refObj.position.y, refObj.position.z + radius)), null);
					}
				} else if (turn_direction == "right") {
					for (degrees = 90.0f; degrees >= 0.0f; degrees -= degrees_incr) {
						CreatePathObject(PointOnCircle(radius, degrees, new Vector3(refObj.position.x - radius, refObj.position.y, refObj.position.z - radius)), null);
					}
				}
				break;
			case "east":
				CreatePathObject(new Vector3(refObj.position.x + metres_pause, refObj.position.y, refObj.position.z), "PathPause");
				
				turn_direction = GetTurnDirection(waypount_count, approaching_direction);
				if (turn_direction == "left") {
					for (degrees = 90.0f; degrees <= 180.0f; degrees += degrees_incr) {
						CreatePathObject(PointOnCircle(radius, degrees, new Vector3(refObj.position.x + radius, refObj.position.y, refObj.position.z - radius)), null);
					}
				} else if (turn_direction == "right") {
					for (degrees = 270.0f; degrees >= 180.0f; degrees -= degrees_incr) {
						CreatePathObject(PointOnCircle(radius, degrees, new Vector3(refObj.position.x + radius, refObj.position.y, refObj.position.z + radius)), null);
					}
				}
				break;
			case "south":
				CreatePathObject(new Vector3(refObj.position.x, refObj.position.y, refObj.position.z - metres_pause), "PathPause");
				
				turn_direction = GetTurnDirection(waypount_count, approaching_direction);
				if (turn_direction == "left") {
					for (degrees = 0.0f; degrees <= 90.0f; degrees += degrees_incr) {
						CreatePathObject(PointOnCircle(radius, degrees, new Vector3(refObj.position.x - radius, refObj.position.y, refObj.position.z - radius)), null);
					}
				} else if (turn_direction == "right") {
					for (degrees = 180.0f; degrees >= 90.0f; degrees -= degrees_incr) {
						CreatePathObject(PointOnCircle(radius, degrees, new Vector3(refObj.position.x + radius, refObj.position.y, refObj.position.z - radius)), null);
					}
				}
				break;
			case "north":
				CreatePathObject(new Vector3(refObj.position.x, refObj.position.y, refObj.position.z + metres_pause), "PathPause");
				
				turn_direction = GetTurnDirection(waypount_count, approaching_direction);
				if (turn_direction == "left") {
					for (degrees = 180.0f; degrees <= 270.0f; degrees += degrees_incr) {
						CreatePathObject(PointOnCircle(radius, degrees, new Vector3(refObj.position.x + radius, refObj.position.y, refObj.position.z + radius)), null);
					}
				} else if (turn_direction == "right") {
					for (degrees = 0.0f; degrees >= -90.0f; degrees -= degrees_incr) {
						CreatePathObject(PointOnCircle(radius, degrees, new Vector3(refObj.position.x - radius, refObj.position.y, refObj.position.z + radius)), null);
					}
				}
				break;
			}
		}
		
		protected string GetApproachingDirection (int waypount_count) {
			string approaching_direction = null;
			Vector3 delta = waypoints[waypount_count].transform.position - waypoints[waypount_count - 1].transform.position;
			if (delta.x != 0) {
				if (delta.x > 0) {
					approaching_direction = "west";
				} else {
					approaching_direction = "east";
				}			
			} else {
				if (delta.z > 0) {
					approaching_direction = "south";
				} else {
					approaching_direction = "north";
				}
			}
			return approaching_direction;
		}
		
		protected string GetTurnDirection (int waypount_count, string approaching_direction) {
			Vector3 delta = waypoints[waypount_count].transform.position - waypoints[waypount_count + 1].transform.position;
			string turn_direction = null;	
			if (delta.x != 0) {
				if (delta.x > 0) {
					if (approaching_direction == "north") {
						turn_direction = "right";
					} else if (approaching_direction == "south") {
						turn_direction = "left";
					}
				} else {
					if (approaching_direction == "north") {
						turn_direction = "left";
					} else if (approaching_direction == "south") {
						turn_direction = "right";
					}
				}			
			} else {
				if (delta.z > 0) {
					if (approaching_direction == "west") {
						turn_direction = "right";
					} else if (approaching_direction == "east") {
						turn_direction = "left";
					}
				} else {
					if (approaching_direction == "west") {
						turn_direction = "left";
					} else if (approaching_direction == "east") {
						turn_direction = "right";
					}
				}
			}
				
			return turn_direction;
		}
		
		protected void CreatePathObject (Vector3 position, string tag) {
			//GameObject pathObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			//BoxCollider collider = pathObject.GetComponent("BoxCollider") as BoxCollider;
			//collider.enabled = false;
			GameObject pathObject = new GameObject();
			pathObject.name = "pathObject" + pathObjectsCount.ToString();
			++pathObjectsCount;
			pathObject.transform.parent = pathObjectsParent.transform;
			pathObject.transform.position = position;
			if (tag != null) {
				pathObject.tag = tag;
			}
			
			pathObjects.Add(pathObject.transform);
		}
		
		public static Vector3 PointOnCircle (float radius, float angleInDegrees, Vector3 origin) {
			float x = (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180f)) + origin.x;
			float z = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180f)) + origin.z;
			
			return new Vector3(x, origin.y, z);
		}
	}
}
