using UnityEngine;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class GeometrieSetting : Setting {
		
		public float gapBetweenBlocks = 4.0f;

		public float blockWidth = 20.0f;

		public float blockHeight = 3.5f;

		public float blockLandmarkWidth = 3.5f;

		public float blockLandmarkHeight = 3.5f;

		public string blockType = "fullLandmarkSize";

		public int mazeColumn = 10;

		public int mazeRow = 10;

		public bool inDoor = false;

		public bool borders = false;
		
		public GeometrieSetting (XmlNode geometrieNode) {
			XmlNode node;
			if (geometrieNode.HasChildNodes) {
				for (int i = 0; i < geometrieNode.ChildNodes.Count; i++) {
					node = geometrieNode.ChildNodes[i];
					switch (node.Name) {
					case "blocks":
						BlocksSettings(node);
						break;
					case "paths":
						PathsSettings(node);
						break;
					case "maze":
						MazeSettings(node);
						break;
					}
				}
			}
		}

		protected void BlocksSettings (XmlNode node) {
			XmlAttributeCollection attrColl = node.Attributes;

			/*if (attrColl["type"] != null) {
				blockType = attrColl["type"].Value;
			}*/

			/*switch (blockType) {
			case "fullLandmarkSize":
				blockLandmarkWidth = 3.5f;
				break;
			case "smallerLandmarkSize":
				blockLandmarkWidth = 2.0f;
				break;
			}*/

			if (attrColl["width"] != null) {
				blockWidth = float.Parse(attrColl["width"].Value);
			}
			if (attrColl["height"] != null) {
				blockHeight = float.Parse(attrColl["height"].Value);
			}

			if (attrColl["landmark_width"] != null) {
				blockLandmarkWidth = float.Parse(attrColl["landmark_width"].Value);
			}
			if (attrColl["landmark_height"] != null) {
				blockLandmarkHeight = float.Parse(attrColl["landmark_height"].Value);
			}
		}

		protected void PathsSettings (XmlNode node) {
			XmlAttributeCollection attrColl = node.Attributes;
			
			if (attrColl["width"] != null) {
				gapBetweenBlocks = float.Parse(attrColl["width"].Value);
			}
		}

		protected void MazeSettings (XmlNode node) {
			XmlAttributeCollection attrColl = node.Attributes;
			
			if (attrColl["columns"] != null) {
				mazeColumn = int.Parse(attrColl["columns"].Value);
			}
			if (attrColl["rows"] != null) {
				mazeRow = int.Parse(attrColl["rows"].Value);
			}
			if (attrColl["indoor"] != null) {
				if (int.Parse(attrColl["indoor"].Value) == 1) {
					inDoor = true;
				}
			}
			if (attrColl["borders"] != null) {
				if (int.Parse(attrColl["borders"].Value) == 1) {
					borders = true;
				}
			}
		}
	}
}
