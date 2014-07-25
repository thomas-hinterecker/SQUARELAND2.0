using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class FormColumn : FormComponent {

		public List<FormComponent> components = new List<FormComponent>();

		public FormColumn (XmlNode xml_node, float given_width, float given_height) {
			width = given_width;
			height = given_height;

			XmlNode form_node;
			XmlAttributeCollection form_node_attr_coll;
			string input_type = "";
			string input_name = "";
			string input_label = "";

			if (xml_node.HasChildNodes) {
				for (int i = 0; i < xml_node.ChildNodes.Count; i++) {
					form_node = xml_node.ChildNodes[i];
					form_node_attr_coll = form_node.Attributes;
					
					if (form_node_attr_coll["width"] != null) {
						width = float.Parse(form_node_attr_coll["width"].Value);
					}
					if (form_node_attr_coll["height"] != null) {
						height = float.Parse(form_node_attr_coll["height"].Value);
					}
					
					switch (form_node.Name) {
					case "input":
						if (form_node_attr_coll["type"] != null) {
							input_type = form_node_attr_coll["type"].Value;
						}
						if (form_node_attr_coll["name"] != null) {
							input_name = form_node_attr_coll["name"].Value;
						}
						if (form_node_attr_coll["label"] != null) {
							input_label = form_node_attr_coll["label"].Value;
						}
						components.Add(new FormInputField(input_type, input_name, input_label, width, height));
						break;
					case "text":
						components.Add(new FormText(form_node.InnerText.ToString(), width, height));
						break;
					case "submit":
						if (form_node_attr_coll["label"] != null) {
							input_label = form_node_attr_coll["label"].Value;
						}
						components.Add(new FormSubmit(input_label, width, height));
						break;
					}
					width = -1.0f;
					height = -1.0f;
				}
			}
		}
	}
}

