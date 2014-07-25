using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {
	public class Instructions : Command {
		
		public List<Instruction> instructions = new List<Instruction>();
		
		public Instructions (int given_num, string given_name, XmlNode command_item_node) {
			XmlNode instruction_node;
			XmlAttributeCollection instruction_node_attr_coll;
			
			num = given_num;
			name = given_name;
			
			if (command_item_node.HasChildNodes) {
				// Read the instructions
				for (int j = 0; j < command_item_node.ChildNodes.Count; j++) {
					instruction_node = command_item_node.ChildNodes[j];
					
					if (instruction_node.Name == "instruction") {
						instruction_node_attr_coll = instruction_node.Attributes;
			
						if (instruction_node_attr_coll["file"] != null) {
							instructions.Add(new ImageInstruction(instruction_node));
						} else {
							instructions.Add(new TextInstruction(instruction_node));
						}
					}
				}
			}
		}
	}
}

