using UnityEngine;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class Crosslines : Command {
		
		public float time;
		
		public Crosslines (int given_num, string given_name, XmlNode command_item_node) {
			num = given_num;
			name = given_name;
			
			XmlAttributeCollection attrColl = command_item_node.Attributes;
			
			if (attrColl["duration"] != null) {
				time = float.Parse(attrColl["duration"].Value);
			}
		}
	}
}

