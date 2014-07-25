using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {
	public class Form : Command {
		
		public string type = "";
		
		public string form_name = "";
		
		public List<FormComponent> components = new List<FormComponent>();
		
		public Form (int given_num, string given_name, XmlNode command_item_node) {
			XmlNode form_node;
			XmlAttributeCollection attrColl, form_node_attr_coll;
			string input_type = "";
			string input_name = "";
			string input_label = "";
			float width = -1.0f;
			float height = -1.0f;
			
			num = given_num;
			name = given_name;
			
			attrColl = command_item_node.Attributes;

			// Read the command attributes.
			if (attrColl["type"] != null) {
				type = attrColl["type"].Value;
			}
			
			if (attrColl["name"] != null) {
				form_name = attrColl["name"].Value;
			}

			// Read the form components.
			if (command_item_node.HasChildNodes) {
				for (int i = 0; i < command_item_node.ChildNodes.Count; i++) {
					form_node = command_item_node.ChildNodes[i];
					form_node_attr_coll = form_node.Attributes;
					
					if (form_node_attr_coll["width"] != null) {
						width = float.Parse(form_node_attr_coll["width"].Value);
					}
					if (form_node_attr_coll["height"] != null) {
						height = float.Parse(form_node_attr_coll["height"].Value);
					}
					
					switch (form_node.Name) {
					case "column":
						components.Add(new FormColumn(form_node, width, height));
						break;
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

