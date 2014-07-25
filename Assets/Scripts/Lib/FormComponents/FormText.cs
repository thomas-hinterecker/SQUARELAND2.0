using UnityEngine;
using System.Collections;

namespace SquarelandSystem {

	public class FormText : FormComponent {
		
		public string text = "";
		
		public FormText (string given_text, float given_width, float given_height) {
			text = given_text;
			width = given_width;
			height = given_height;
		}
	}
}

