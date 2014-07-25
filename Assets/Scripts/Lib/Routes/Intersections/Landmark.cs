using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class Landmark {
		
		public string cube = "";
		
		public string type = "";
		
		public string fileName = "";
		
		public Texture2D file = null;
		
		public Color color;
		
		public Landmark (XmlNode landmarkNode) {
			XmlAttributeCollection attrColl = landmarkNode.Attributes;
			
			cube = attrColl["cube"].Value;
			if (attrColl["file"] != null) {
				SetFile(attrColl["file"].Value);
			}
			if (attrColl["color"] != null) {
				SetColor(attrColl["color"].Value);
			}
		}
		
		public void SetColor (string color_string) {
			string[] color_splitted = color_string.Split('.');
			float r = float.Parse(color_splitted[0]);
			float g = float.Parse(color_splitted[1]);
			float b = float.Parse(color_splitted[2]);
			color = new Color(r/255.0f, g/255.0f, b/255.0f);
			type = "color";
		}
		
		public void SetFile (string file_string) {
			fileName = file_string;
			byte[] textureData = File.ReadAllBytes(file_string);
			file = new Texture2D(0, 0);
			file.LoadImage(textureData);
			type = "file";
		}
	}
}
