using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class Trial {

		/**
		 * 
		 */
		public int trialNum;

		/**
		 * 
		 */
		public List<Command> commands = new List<Command>();

		/**
		 * 
		 */
		public int currentCommand = -1;

		/**
		 * User inputs
		 */
		public Responses responses = new Responses();

		/**
		 * Reads out the commands
		 */
		public Trial (int given_num, XmlNode trialNode) {
			XmlNode commandNode;
			Command command = null;
			string name;

			int command_count = 0;

			trialNum = given_num;

			if (trialNode.HasChildNodes) {
				for (int z = 0; z < trialNode.ChildNodes.Count; ++z) {
					commandNode = trialNode.ChildNodes[z];
					name = commandNode.Name;

					switch (commandNode.Name) {
					case "settings":
						//command = new Squareland(++command_count, name, commandNode);
						break;
					case "squareland":
						command = new Squareland(++command_count, name, commandNode);
						break;
					case "crosslines":
						command = new Crosslines(++command_count, name, commandNode);
						break;
					case "form":
						command = new Form(++command_count, name, commandNode);
						break;
					case "instructions":
						command = new Instructions(++command_count, name, commandNode);
						break;
					}
					
					commands.Add(command);
				}
			}
			
			commands.Add(new Command(++command_count, "log"));
		}

		/**
		 * Returns the current trial command.
		 */
		public Command GetCurrentCommand () {
			return commands[currentCommand];
		}
		
		/**
		 * Loads the next trial command.
		 */
		public void LoadNextCommand () {
			
			currentCommand++;
			
			responses.ClearEventData();
			
			string level_name = commands[currentCommand].name;
			level_name = char.ToUpper(level_name[0]) + level_name.Substring(1);
			
			Debug.Log("Loading: " + level_name + "; Trial: " + (Controller.procedure.currentTrial + 1));
			
			Application.LoadLevel(level_name);
		}
	}
}

