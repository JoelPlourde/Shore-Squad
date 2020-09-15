using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem {
	/// <summary>
	/// The Area class indicates an area on a map defines by its origin and its size.
	/// </summary>
	public class Area {
		public Vector2Int Origin;
		public Vector2Int Size;

		public Area(Vector2Int origin, Vector2Int size) {
			Origin = origin;
			Size = size;
		}

		/// <summary>
		/// Verify if the size in parameters fits in this area.
		/// </summary>
		/// <param name="size">Rectangle to try to fit in the area.</param>
		/// <returns>Return true if the rectangle fits in the area, else false.</returns>
		public bool IsLessOrEquals(Vector2Int size) {
			return (Size.x >= size.x && Size.y >= size.y);
		}

		/// <summary>
		/// Verify if the size in parameters fits in this area, allow that the rectangle is rotated to verify the condition.
		/// </summary>
		/// <param name="size">Rectangle to try to fit in the area.</param>
		/// <param name="rotate">Determine if the rotation had to made to make it fit.</param>
		/// <returns>Return true if the rectangle fits in the area, else false. Validate rotate to see if it has to be rotate first.</returns>
		public bool IsLessOrEquals(Vector2Int size, out bool rotate) {
			rotate = false;
			if (Size.x >= size.x && Size.y >= size.y) {
				return true;
			}

			if (Size.x >= size.y && Size.y >= size.x) {
				rotate = true;
				return true;
			}

			return false;
		}
	}
}
