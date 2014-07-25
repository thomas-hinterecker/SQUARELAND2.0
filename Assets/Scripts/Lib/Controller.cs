using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {
	public class Controller {
		
		public static string participant_id;
		
		public static Dictionary<string, Setting> settings = new Dictionary<string, Setting>();

		public static Routes routes;

		public static Procedure procedure;
		
		public static float timeSinceStart = 0.0f;

		/**
		 *
		 */
		public static void StartSquareland () {
			XmlLoader.Load();
		}
	}
}

