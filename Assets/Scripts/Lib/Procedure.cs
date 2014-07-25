using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class Procedure {
		
		public List<Trial> trials = new List<Trial>();
		
		public int currentTrial = 0;

		/**
		 * Reads out the procedure trials.
		 */
		public Procedure (XmlNode procedureNode) {
			XmlNode procedureItemNode;
			Trial trial;
			int z = 0;

			// Read out the trials
			if (procedureNode.HasChildNodes) {
				for (z = 0; z < procedureNode.ChildNodes.Count; ++z) {
					procedureItemNode = procedureNode.ChildNodes[z];

					switch (procedureItemNode.Name) {
					case "trial":
						trial = new Trial(z + 1, procedureItemNode);
						trials.Add(trial);
						break;
					}
				}
			}
		}

		/**
		 * Returns the current trial.
		 */
		public Trial GetCurrentTrial () {
			return trials[currentTrial];
		}

		/**
		 * Loads the next trial.
		 */
		public void LoadNextTrial () {
			currentTrial++;
			if (trials.Count >= currentTrial) {
				Application.Quit();
			} else {
				GetCurrentTrial().LoadNextCommand();
			}
		}
	}
}

