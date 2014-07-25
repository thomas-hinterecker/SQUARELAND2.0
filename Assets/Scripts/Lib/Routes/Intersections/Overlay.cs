using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class Overlay {
		
		public string appearsAt = "";

		public float x = 0;

		public float y = 0;

		public string text = "";

		public float textMaximumWidth = 0;

		public string fileName = "";
		
		public Texture2D file = null;
		
		public float fileWidth = -1;
		
		public float fileHeight = -1;
		
		public Overlay (XmlNode overlayNode) {
			XmlAttributeCollection attrColl = overlayNode.Attributes;

			OverlaysSetting overlaysSetting = (OverlaysSetting) Controller.settings["overlays"];

			if (attrColl["appears_at"] != null) {
				appearsAt = attrColl["appears_at"].Value;
			}

			if (attrColl["y"] != null) {
				y = float.Parse(attrColl["y"].Value);
			} else {
				y = overlaysSetting.y;
			}
			if (attrColl["x"] != null) {
				x = float.Parse(attrColl["x"].Value);
			} else {
				x = overlaysSetting.x;
			}

			if (attrColl["text"] != null) {
				text = attrColl["text"].Value;
			}
			if (attrColl["text_maximum_width"] != null) {
				textMaximumWidth = float.Parse(attrColl["text_maximum_width"].Value);
			} else {
				textMaximumWidth = overlaysSetting.textMaximumWidth;
			}

			if (attrColl["file"] != null) {
				SetFile(attrColl["file"].Value);
			}
			if (attrColl["file_width"] != null) {
				fileWidth = float.Parse(attrColl["file_width"].Value);
			}
			if (attrColl["file_height"] != null) {
				fileHeight = float.Parse(attrColl["file_height"].Value);
			}
		}
		
		public void SetFile (string file_string) {
			fileName = file_string;
			byte[] textureData = File.ReadAllBytes(file_string);
			file = new Texture2D(0, 0);
			file.LoadImage(textureData);
		}
	}
}
