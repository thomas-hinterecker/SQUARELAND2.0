using UnityEngine;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class HazeSetting : Setting {
		
		public bool enable = false;
		
		public float addMeters = 0.0f;
		
		public HazeSetting (XmlNode fogNode) {
			XmlAttributeCollection attrColl = fogNode.Attributes;
			
			if (attrColl["enable"] != null) {
				if (attrColl["enable"].Value == "1") {
					enable = true;
				}
			}
			
			if (attrColl["add_meters"] != null) {
				addMeters = float.Parse(attrColl["add_meters"].Value);
			}
		}
	}
}
