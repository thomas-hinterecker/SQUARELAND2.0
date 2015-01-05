using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class XmlLoader {

		public static bool xmlLoaded = false;

		public static XmlNode xmlRoot;

		public static string settingsFile = "Settings.xml";

		public static string procedureFile = "Procedure.xml";

		/**
		 * Opens the Xml file 
		 */
		public static void Load () {
			if (xmlLoaded == false) {
				xmlLoaded = true;

				loadSettingsFile();
				loadProcedureFile();
			}
		}

		/**
		 * 
		 */
		protected static void loadSettingsFile () {
			XmlDocument xmlDoc = new XmlDocument();
			try {
				xmlDoc.Load("Assets/" + settingsFile);
				xmlRoot = xmlDoc.FirstChild;
				XmlNode firstLevelNode, secondLevelNode, thirdLevelNode;
				
				if (xmlRoot.HasChildNodes) {
					// Go through child elements
					for (int i = 0; i < xmlRoot.ChildNodes.Count; i++) {
						firstLevelNode = xmlRoot.ChildNodes[i];

						if (firstLevelNode.Name == "preparation") {
							if (firstLevelNode.HasChildNodes) {
								for (int z = 0; z < firstLevelNode.ChildNodes.Count; z++) {
									secondLevelNode = firstLevelNode.ChildNodes[z];

									if (secondLevelNode.Name == "settings") {
										if (secondLevelNode.HasChildNodes) {
											// Which type of setting?
											for (int y = 0; y < secondLevelNode.ChildNodes.Count; y++) {
												thirdLevelNode = secondLevelNode.ChildNodes[y];

												switch (thirdLevelNode.Name) {
												case "camera":
													Controller.settings.Add("camera", new CameraSetting(thirdLevelNode));
													break;
												case "haze":
													Controller.settings.Add("haze", new HazeSetting(thirdLevelNode));
													break;
												case "movement":
													Controller.settings.Add("movement", new MovementSetting(thirdLevelNode));
													break;
												case "geometrie":
													Controller.settings.Add("geometrie", new GeometrieSetting(thirdLevelNode));
													break;
												case "textures":
													Controller.settings.Add("textures", new TexturesSetting(thirdLevelNode));
													break;
												case "overlays":
													Controller.settings.Add("overlays", new OverlaysSetting(thirdLevelNode));
													break;
												case "instructions":
													Controller.settings.Add("instructions", new InstructionsSetting(thirdLevelNode));
													break;
												case "files":
													Controller.settings.Add("files", new FilesSetting(thirdLevelNode));
													break;
												}
											}
										}
									} else if (secondLevelNode.Name == "routes") {
										Controller.routes = new Routes(secondLevelNode);
									}
								}
							}
						}
					}
				}
			} catch (UnityException e) {
				ErrorLog.Log(e.Message, ErrorLog.CRITICAL);
			}
		}

		/**
		 * 
		 */
		protected static void loadProcedureFile () {
			XmlDocument xmlDoc = new XmlDocument();
			try {
				xmlDoc.Load("Assets/" + procedureFile);
				xmlRoot = xmlDoc.FirstChild;
				XmlNode firstLevelNode, secondLevelNode;
				
				if (xmlRoot.HasChildNodes) {
					// Go through child elements
					for (int i = 0; i < xmlRoot.ChildNodes.Count; i++) {
						firstLevelNode = xmlRoot.ChildNodes[i];
						
						if (firstLevelNode.Name == "procedure") {
							Controller.procedure = new Procedure(firstLevelNode);
						}
					}
				}
			} catch (UnityException e) {
				ErrorLog.Log(e.Message, ErrorLog.CRITICAL);
			}
		}
	}
}

