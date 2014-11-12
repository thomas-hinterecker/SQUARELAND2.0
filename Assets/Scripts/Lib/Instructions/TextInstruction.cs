using UnityEngine;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class TextInstruction : Instruction {
		
		public string text = "";
		
		public TextInstruction (XmlNode instructionNode) {
			XmlAttributeCollection attrColl = instructionNode.Attributes;

			text = instructionNode.InnerText;
			Debug.Log (text);

			if (attrColl["min_duration"] != null) {
				minDuration = float.Parse(attrColl["min_duration"].Value);
			}
		}
	}
}
