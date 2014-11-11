using UnityEngine;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {
	public class Squareland : Command {
		
		/**
		 * 
		 */		
		public Route route;
		
		/**
		 * 
		 */		
		public int route_num = 0;
		
		/**
		 * 
		 */		
		public string performStop = "timeStop";
		
		/**
		 * 
		 */		
		public int decisionStopAxis = 1;
		
		/**
		 * 
		 */		
		public float timeStopDuration = 0.0f;
		
		/**
		 * 
		 */		
		public bool startFromEnd = false;
		
		/**
		 * 
		 */		
		public int firstIntersection = 0;
		
		/**
		 * 
		 */		
		public int numIntersections = 0;
		
		/**
		 * 
		 */	
		public string movementMode = "";
		
		/**
		 * 
		 */
		public bool record = false;

		/**
		 * 
		 */
		public string recordFolderName = "";

		/**
		 * 
		 */
		public bool pointing = false;

		/**
		 * 
		 */		
		public int pointingStartLocation = 0;
		
		/**
		 * 
		 */		
		public int pointingToLocation = 0;

		/**
		 * 
		 */
		public Squareland (int given_num, string given_name, XmlNode command_item_node) {
			num = given_num;
			name = given_name;

			ReadAttributes(command_item_node);
			ReadChildNodes(command_item_node);
		}

		/**
		 * 
		 */
		protected void ReadAttributes (XmlNode command_item_node) {
			XmlAttributeCollection attrColl = command_item_node.Attributes;
			
			if (attrColl["route"] != null) {
				Routes routes = Controller.routes;
				route = routes.routes[attrColl["route"].Value];
			}
			
			if (attrColl["perform_stop"] != null) {
				performStop = attrColl["perform_stop"].Value;
			}
			
			if (attrColl["time_stop_duration"] != null) {
				timeStopDuration = float.Parse(attrColl["time_stop_duration"].Value);
			}
			
			if (attrColl["decision_stop_axis"] != null) {
				decisionStopAxis = int.Parse(attrColl["decision_stop_axis"].Value);
			}
			
			if (attrColl["from_end"] != null) {
				if (attrColl["from_end"].Value == "1") {
					startFromEnd = true;
				}
			}
			
			if (attrColl["movement_mode"] != null) {
				movementMode = attrColl["movement_mode"].Value;
			}
			
			if (attrColl["first_intersection"] != null) {
				firstIntersection = int.Parse(attrColl["first_intersection"].Value) - 1;
			}
			
			if (attrColl["num_intersections"] != null) {
				numIntersections = int.Parse(attrColl["num_intersections"].Value) + 1;
			} else {
				numIntersections = -1;
			}
			
			if (attrColl["record"] != null) {
				if (int.Parse(attrColl["record"].Value) == 1) {
					record = true;
				}
			}
			
			if (attrColl["record_folder_name"] != null) {
				recordFolderName = attrColl["record_folder_name"].Value;
			}
		}

		/**
		 * 
		 */
		protected void ReadChildNodes (XmlNode command_item_node) {
			XmlNode commandChildNode;
			XmlAttributeCollection attrColl;

			if (command_item_node.HasChildNodes) {
				for (int i = 0; i < command_item_node.ChildNodes.Count; ++i) {
					commandChildNode = command_item_node.ChildNodes[i];
					
					switch (commandChildNode.Name) {
					case "pointing":
						pointing = true;

						attrColl = commandChildNode.Attributes;

						if (attrColl["location"] != null) {
							pointingStartLocation = int.Parse(attrColl["location"].Value) - 1;
						}
						
						if (attrColl["point_to"] != null) {
							pointingToLocation = int.Parse(attrColl["point_to"].Value) - 1;
						}
						break;
					}
				}
			}
		}
	}
}

