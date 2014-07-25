using UnityEngine;
using System.Xml;
using System.Collections;

namespace SquarelandSystem {

	public class CameraSetting : Setting {
		
		public float fieldOfView = 35.0f;
		
		public float eyeHeight = 1.7f;
		
		public float farClipPlane = 12.0f;
		
		public CameraSetting (XmlNode cameraNode) {
			XmlAttributeCollection attrColl = cameraNode.Attributes;
			
			if (attrColl["field_of_view"] != null) {
				fieldOfView = float.Parse(attrColl["field_of_view"].Value);
			}
			if (attrColl["far_clip_plane"] != null) {
				farClipPlane = float.Parse(attrColl["far_clip_plane"].Value);
			}
			if (attrColl["eye_height"] != null) {
				eyeHeight = float.Parse(attrColl["eye_height"].Value);
			}
		}
		
		public void ApplyCameraSetting () {
			Camera.main.fieldOfView = fieldOfView;
			Camera.main.farClipPlane = farClipPlane;
		}
	}
}
