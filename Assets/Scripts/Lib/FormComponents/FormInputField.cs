using UnityEngine;
using System.Collections;

namespace SquarelandSystem {

	public class FormInputField : FormComponent {
		
		public string type = "";
		
		public string name = "";
		
		public string label = "";
		
		public FormInputField (string given_type, string given_name, string given_label, float given_width, float given_height) {
			type = given_type;
			name = given_name;
			label = given_label;
			width = given_width;
			height = given_height;
		}
	}
}

