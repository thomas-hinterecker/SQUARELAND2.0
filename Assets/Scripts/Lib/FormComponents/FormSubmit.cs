using UnityEngine;
using System.Collections;

namespace SquarelandSystem {

	public class FormSubmit : FormComponent {
		
		public string label = "";
		
		public FormSubmit (string given_label, float given_width, float given_height) {
			label = given_label;
			width = given_width;
			height = given_height;
		}
	}
}

