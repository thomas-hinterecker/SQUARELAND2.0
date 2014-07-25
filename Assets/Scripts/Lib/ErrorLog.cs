using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class ErrorLog {

		public static int NOTICE = 1;

		public static int CRITICAL = 2;

		protected static List<string> messages = new List<string>();

		public static void Log (string message, int priority) {
			messages.Add(message);

			Debug.Log("Logger: " + message);

			if (priority > 1) {
				CreateLogFile();
				Application.Quit();
			}
		}

		public static void CreateLogFile () {

		}
	}
}

