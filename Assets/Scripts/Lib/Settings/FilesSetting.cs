using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class FilesSetting : Setting {

		public List<string> files = new List<string>();
		
		public FilesSetting (XmlNode filesNode) {
			XmlNode node;

			if (filesNode.HasChildNodes) {
				for (int i = 0; i < filesNode.ChildNodes.Count; i++) {
					node = filesNode.ChildNodes[i];
					switch (node.Name) {
					case "file":
						FileSettings(node);
						break;
					}
				}
			}

			XmlAttributeCollection attrColl = filesNode.Attributes;
			if (attrColl["randomize"] != null) {
				if (attrColl["randomize"].Value == "1") {
					for (int i = 0; i < files.Count; i++) {
						string temp = files[i];
						int randomIndex = Random.Range(i, files.Count);
						files[i] = files[randomIndex];
						files[randomIndex] = temp;
					}
				}
			}

		}

		protected void FileSettings (XmlNode fileNode) {
			XmlAttributeCollection attrColl = fileNode.Attributes;

			string path = "";

			if (attrColl["path"] != null) {
				path = attrColl["path"].Value;
			}

			files.Add(path);
		}
	}
}
