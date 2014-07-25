using UnityEngine;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class InstructionsSetting : Setting {
		
		public float minDuration = 0.0f;
		
		public InstructionsSetting (XmlNode instructionsNode) {
			XmlAttributeCollection attrColl = instructionsNode.Attributes;
			
			if (attrColl["min_duration"] != null) {
				minDuration = float.Parse(attrColl["min_duration"].Value);
			}
		}
	}
}
