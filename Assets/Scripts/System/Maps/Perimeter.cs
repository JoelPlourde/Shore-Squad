using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem {
	public class Perimeter {
		public Border X { get; private set; }
		public Border Y { get; private set; }

		public Perimeter(Vector2Int mapSize, Vector2Int rectangleSize, int limit = 0) {
			X = new Border(mapSize.x, rectangleSize.x, limit);
			Y = new Border(mapSize.y, rectangleSize.y, limit);
		}
	}
}
