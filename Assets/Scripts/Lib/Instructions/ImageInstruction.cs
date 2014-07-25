using UnityEngine;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class ImageInstruction : Instruction {
		
		public string file = "";
		
		public ImageInstruction (XmlNode instructionNode) {
			XmlAttributeCollection attrColl = instructionNode.Attributes;
			
			if (attrColl["file"] != null) {
				file = attrColl["file"].Value;
			}

			if (attrColl["min_duration"] != null) {
				minDuration = float.Parse(attrColl["min_duration"].Value);
			}
		}
	}
}
