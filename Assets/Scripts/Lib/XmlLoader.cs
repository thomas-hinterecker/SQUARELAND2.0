using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class XmlLoader {

		public static bool xmlLoaded = false;

		public static XmlNode xmlRoot;

		/**
		 * Opens the Xml file 
		 */
		public static void Load () {
			if (xmlLoaded == false) {
				xmlLoaded = true;

				XmlDocument xmlDoc = new XmlDocument();
				try {
					xmlDoc.Load("Assets/Setup.xml");
					xmlRoot = xmlDoc.FirstChild;
					LoadChildNodes();
				} catch (UnityException e) {
					ErrorLog.Log(e.Message, ErrorLog.CRITICAL);
				}
			}
		}

		protected static void LoadChildNodes () {
			XmlNode firstLevelNode, secondLevelNode;

			if (xmlRoot.HasChildNodes) {
				// Go through child elements
				for (int i = 0; i < xmlRoot.ChildNodes.Count; i++) {
					firstLevelNode = xmlRoot.ChildNodes[i];
					
					if (firstLevelNode.Name == "settings") {
						if (firstLevelNode.HasChildNodes) {
							// Which type of setting?
							for (int z = 0; z < firstLevelNode.ChildNodes.Count; z++) {
								secondLevelNode = firstLevelNode.ChildNodes[z];
								switch (secondLevelNode.Name) {
								case "camera":
									Controller.settings.Add("camera", new CameraSetting(secondLevelNode));
									break;
								case "haze":
									Controller.settings.Add("haze", new HazeSetting(secondLevelNode));
									break;
								case "movement":
									Controller.settings.Add("movement", new MovementSetting(secondLevelNode));
									break;
								case "geometrie":
									Controller.settings.Add("geometrie", new GeometrieSetting(secondLevelNode));
									break;
								case "textures":
									Controller.settings.Add("textures", new TexturesSetting(secondLevelNode));
									break;
								case "overlays":
									Controller.settings.Add("overlays", new OverlaysSetting(secondLevelNode));
									break;
								case "instructions":
									Controller.settings.Add("instructions", new InstructionsSetting(secondLevelNode));
									break;
								}
							}
						}
					} else if (firstLevelNode.Name == "routes") {
						Controller.routes = new Routes(firstLevelNode);
					} else if (firstLevelNode.Name == "procedure") {
						Controller.procedure = new Procedure(firstLevelNode);
					}
				}
			}
		}
	}
}

