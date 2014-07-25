using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class Command {
		
		public int num;
		
		public string name;

		public Command () {

		}

		public Command (int given_num, string given_name) {
			num = given_num;
			name = given_name;
		}

		public Command (int given_num, string given_name, XmlNode command_item_node) {
			num = given_num;
			name = given_name;
		}
	}
}
