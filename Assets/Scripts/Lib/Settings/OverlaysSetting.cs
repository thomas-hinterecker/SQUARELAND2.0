using UnityEngine;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class OverlaysSetting : Setting {
		
		public float y = 0;
		
		public float x = 0;
		
		public float textMaximumWidth = 200;
		
		public int fontSize = 15;
		
		public string fontColor = "255.255.255";
		
		public string backgroundColor = "0.0.0";
		
		public OverlaysSetting (XmlNode playerNode) {
			XmlAttributeCollection attrColl = playerNode.Attributes;

			if (attrColl["y"] != null) {
				y = float.Parse(attrColl["y"].Value);
			}
			if (attrColl["x"] != null) {
				x = float.Parse(attrColl["x"].Value);
			}
			if (attrColl["text_maximum_width"] != null) {
				textMaximumWidth = float.Parse(attrColl["text_maximum_width"].Value);
			}
			if (attrColl["font_size"] != null) {
				fontSize = int.Parse(attrColl["font_size"].Value);
			}
			if (attrColl["font_color"] != null) {
				fontColor = attrColl["font_color"].Value;
			}
			if (attrColl["background_color"] != null) {
				backgroundColor = attrColl["background_color"].Value;
			}
		}
	}
}
