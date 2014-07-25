using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class Routes : Setting {
		
		public Dictionary<string, Route> routes = new Dictionary<string, Route>();
		
		public Routes (XmlNode routesNode) {
			Route route;
			if (routesNode.HasChildNodes) {
				for (int z = 0; z < routesNode.ChildNodes.Count; z++) {
					route = new Route(routesNode.ChildNodes[z]);
					routes.Add(route.name, route);
				}
			}
		}
	}
}
