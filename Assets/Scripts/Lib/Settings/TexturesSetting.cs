using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class TexturesSetting : Setting {
		
		public string type = "";
		
		public string blockTextureName = "";
		
		public Texture2D blockTexture = null;
		
		public float blockTextureTilingX = 1.0f;
		
		public float blockTextureTilingY = 1.0f;
		
		public string blockColor = "227.227.227";
		
		public string surfaceTextureName = "";
		
		public Texture2D surfaceTexture = null;
		
		public float surfaceTextureTilingX = 1.0f;
		
		public float surfaceTextureTilingY = 1.0f;

		public string skyColor = "0.87.181";
		
		public TexturesSetting (XmlNode textureNode) {
			XmlNode node;
			if (textureNode.HasChildNodes) {
				for (int i = 0; i < textureNode.ChildNodes.Count; i++) {
					node = textureNode.ChildNodes[i];
					switch (node.Name) {
					case "blocks":
						BlocksSettings(node);
						break;
					case "surface":
						SurfaceSettings(node);
						break;
					case "sky":
						SkySettings(node);
						break;
					}
				}
			}
		}

		protected void BlocksSettings (XmlNode textureNode) {
			XmlAttributeCollection attrColl = textureNode.Attributes;
			
			if (attrColl["color"] != null) {
				blockColor = attrColl["color"].Value;
				type = "color";
			}
			if (attrColl["file"] != null) {
				blockTextureName = attrColl["file"].Value;
				byte[] textureData = File.ReadAllBytes(blockTextureName);
				blockTexture = new Texture2D(0, 0);
				blockTexture.LoadImage(textureData);
				type = "file";
			}
			if (attrColl["tiling_x"] != null) {
				blockTextureTilingX = float.Parse(attrColl["tiling_x"].Value);
			}
			if (attrColl["tiling_y"] != null) {
				blockTextureTilingY = float.Parse(attrColl["tiling_y"].Value);
			}
		}

		protected void SurfaceSettings (XmlNode textureNode) {
			XmlAttributeCollection attrColl = textureNode.Attributes;
			
			if (attrColl["file"] != null) {
				surfaceTextureName = attrColl["file"].Value;
				byte[] textureData = File.ReadAllBytes(surfaceTextureName);
				surfaceTexture = new Texture2D(0, 0);
				surfaceTexture.LoadImage(textureData);
				type = "file";
			}
			if (attrColl["tiling_x"] != null) {
				surfaceTextureTilingX = float.Parse(attrColl["tiling_x"].Value);
			}
			if (attrColl["tiling_y"] != null) {
				surfaceTextureTilingY = float.Parse(attrColl["tiling_y"].Value);
			}
		}

		protected void SkySettings (XmlNode textureNode) {
			XmlAttributeCollection attrColl = textureNode.Attributes;
			
			if (attrColl["color"] != null) {
				skyColor = attrColl["color"].Value;
			}
		}
	}
}
