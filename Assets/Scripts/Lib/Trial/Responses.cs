using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class Responses {
		
		public List<Response> responses = new List<Response>();
		
		protected string nextEvent = "";
		
		protected string nextEventAdditionalInfo = "";
		
		protected float nextEventTime = 0.0f;

		protected string lastEvent = "";

		protected string lastEventAdditionalInfo = "";

		/**
		 * Adds a response
		 */
		public void Add (Response response) {
			Trial trial = Controller.procedure.GetCurrentTrial();
			response.trialItem = trial.currentCommand;
			response.number = responses.Count + 1;
			
			//Debug.Log(nextEvent);

			if (nextEvent == "") {
				nextEvent = lastEvent;
				if (lastEventAdditionalInfo != "" && nextEventAdditionalInfo == "") {
					lastEventAdditionalInfo = nextEventAdditionalInfo;
				}
			}

			response.eventName = nextEvent;
			response.eventInfo = nextEventAdditionalInfo;
			response.timeSinceStart = Controller.timeSinceStart;
			if (nextEventTime > 0) {
				response.time = Controller.timeSinceStart - nextEventTime;
			} else {
				response.time = 0;
			}
			
			//Debug.Log("pressed: " + response.time + " - " + Main.timeSinceStart + " - " + nextEventTime);
			
			responses.Add(response);

			lastEvent = nextEvent;
			lastEventAdditionalInfo = nextEventAdditionalInfo;
		}

		/**
		 * Sets info text for the next response.
		 */
		public void NextEvent (string next_event) {
			nextEvent = next_event;
			nextEventAdditionalInfo = "";
		}

		/**
		 * Sets info text for the next event. With clear option.
		 */
		public void NextEvent (string next_event, bool clear) {
			if (true == clear) {
				if (nextEvent == "") {
					nextEvent = next_event;
					nextEventAdditionalInfo = "";
				}
			} else {
				nextEvent = next_event;
				nextEventAdditionalInfo = "";
			}
		}

		/**
		 * Sets info text for the next event. With clear and additional info option.
		 */
		public void NextEvent (string next_event, bool clear, string additional_info) {
			if (true == clear) {
				if (nextEvent == "") {
					nextEvent = next_event;
					nextEventAdditionalInfo = additional_info;
				}
			} else {
				nextEvent = next_event;
				nextEventAdditionalInfo = additional_info;
			}
		}

		/**
		 * Clears the event data.
		 */
		public void ClearEventData () {
			ClearTime();
			nextEvent = "";
			nextEventAdditionalInfo = "";
		}

		/**
		 * Clears the next event time.
		 */
		public void ClearTime () {
			nextEventTime = 0.0f;
		}

		/**
		 * Starts a time recorder for the next response.
		 */
		public void SetTimeRecorder () {
			if (nextEventTime == 0.0f) {
				nextEventTime = Controller.timeSinceStart;
				//Debug.Log("start: " + Main.timeSinceStart);
			} else {
				//Debug.Log(Main.timeSinceStart - nextEventTime);
			}
		}
	}
}
