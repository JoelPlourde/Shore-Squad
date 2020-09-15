using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem {
	public class Border {
		public int Min;
		public int Max;

		public Border(int mapSize, int rectangleSize, int limit = 0) {
			int halfSize = rectangleSize >> 1;
			Min = halfSize + limit;
			Max = mapSize - halfSize - limit;
		}
	}
}
