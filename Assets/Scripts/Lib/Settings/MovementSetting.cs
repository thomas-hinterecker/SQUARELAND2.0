using UnityEngine;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class MovementSetting : Setting {

		public string mode = "passive";

		public float movementSpeed = 1.39f;
		
		public float stopInfrontWaypoint = 0.0f;
		
		public MovementSetting (XmlNode playerNode) {
			XmlAttributeCollection attrColl = playerNode.Attributes;

			if (attrColl["mode"] != null) {
				mode = attrColl["mode"].Value;
			}
			if (attrColl["speed"] != null) {
				movementSpeed = float.Parse(attrColl["speed"].Value) * 50;
			}
			if (attrColl["stop_meters"] != null) {
				stopInfrontWaypoint = float.Parse(attrColl["stop_meters"].Value);
			}
		}
	}
}
