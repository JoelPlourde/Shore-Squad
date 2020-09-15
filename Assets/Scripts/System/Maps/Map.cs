using EncampmentSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace GameSystem {
	/// <summary>
	/// This class represents a 1:1 map of an area. 1 pixel on the map is equals to 1 unit in the world.
	/// </summary>
	[System.Serializable]
	public class Map {

		public static Color[] Invalid = { Color.black, Color.red };

		[SerializeField]
		public Texture2D Texture { get; private set; }
		public Vector2Int Size { get; private set; }
		public int Count { get; private set; }

		/// <summary>
		/// Create a new map.
		/// </summary>
		/// <param name="mapSize">Size of the map to generate.</param>
		public Map(int mapSize) {
			Size = new Vector2Int(mapSize, mapSize);
			Texture = new Texture2D(Size.x, Size.y, TextureFormat.RGBA32, false) {
				filterMode = FilterMode.Point
			};
			Count = Size.x * Size.y;
			Color[] pixels = Enumerable.Repeat(Color.white, Count).ToArray();
			Texture.SetPixels(0,0, Size.x, Size.y, pixels);
		}

		public Map(int mapSize, Texture2D texture2D) {
			Color[] pixels = texture2D.GetPixels(0, 0, mapSize, mapSize);
			Size = new Vector2Int(mapSize, mapSize);
			Texture = new Texture2D(Size.x, Size.y, TextureFormat.RGBA32, false) {
				filterMode = FilterMode.Point
			};
			Count = Size.x * Size.y;
			Texture.SetPixels(0, 0, Size.x, Size.y, pixels);
		}

		// TODO implement increazing the size of the map.
		public Map(int mapSize, Map existingMap) {
		}

		/// <summary>
		/// Validate if the NavMeshObstacle can be placed unto the map.
		/// </summary>
		/// <param name="obstacle">The obstacle of the map.</param>
		/// <param name="worldPosition">The origin of where we want to place the object in world position.</param>
		/// <returns>Returns true if the origin position is valid, false if not.</returns>
		public bool IsPositionValid(NavMeshObstacle obstacle, ZoneBehaviour zoneBehaviour) {
			Vector2Int rectangleSize = GetSizeFromObstacle(obstacle);
			if (GetMapPositionFrom(this, zoneBehaviour.transform.position, obstacle.transform, rectangleSize, out Vector2Int relativePosition)) {
				return IsPositionValid(relativePosition, rectangleSize);
			}
			return false;
		}

		/// <summary>
		/// Validate if the map allows to place an object at origin of rectangleSize.
		/// </summary>
		/// <param name="rectangleSize">The size of the rectangle in map scale.</param>
		/// <param name="origin">Origin that is being verify</param>
		/// <returns>Returns true if the origin position is valid, false if not.</returns>
		public bool IsPositionValid(Vector2Int origin, Vector2Int rectangleSize) {
			foreach (Color color in GetColorsAt(origin, rectangleSize)) {
				if (Invalid.Contains(color)) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Draw a rectangle on the map.
		/// </summary>
		/// <param name="origin">The origin of the rectangle relative to the map.</param>
		/// <param name="size">The size of the rectangle relative to the map.</param>
		/// <param name="color">The color of the rectangle.</param>
		public void DrawRectangle(Vector2Int origin, Vector2Int size, Color color) {
			Color[] pixels = Enumerable.Repeat(color, size.x * size.y).ToArray();
			Texture.SetPixels(origin.x, origin.y, size.x, size.y, pixels);
			Texture.Apply();
		}

		public void DrawRectangle(NavMeshObstacle obstacle, ZoneBehaviour zoneBehaviour, Color color) {
			Vector2Int rectangleSize = GetSizeFromObstacle(obstacle);
			if (GetMapPositionFrom(this, zoneBehaviour.transform.position, obstacle.transform, rectangleSize, out Vector2Int relativePosition)) {
				DrawRectangle(relativePosition, rectangleSize, color);
			} else {
				Debug.LogError("Couldn't draw the rectangle.");
			}
		}

		public Color[] GetColorsAt(Vector2Int origin, Vector2Int rectangleSize) {
			return Texture.GetPixels(origin.x, origin.y, rectangleSize.x, rectangleSize.y);
		}

		/// <summary>
		/// Clear the map to its default color.
		/// </summary>
		public void Clear() {
			Color[] pixels = Enumerable.Repeat(Color.white, Count).ToArray();
			Texture.SetPixels(0, 0, Size.x, Size.y, pixels);
			Texture.Apply();
		}


		/// <summary>
		/// Get the map relative position from a World reference position, an object and its dimensions.
		/// </summary>
		/// <param name="reference">The reference in a world position.</param>
		/// <param name="object">The object to be recorded on the map.</param>
		/// <param name="objectSize">The object's size</param>
		/// <returns>Returns the position on the map.</returns>
		public static bool GetMapPositionFrom(Map map, Vector3 reference, Transform @object, Vector2Int objectSize, out Vector2Int mapRelativePosition) {
			Vector3 origin = new Vector3(reference.x - (map.Size.x >> 1), 0, reference.z - (map.Size.y >> 1));
			Vector3 objectPosition = new Vector3(@object.position.x - (objectSize.x >> 1), 0, @object.position.z - (objectSize.y >> 1));
			Vector3 relativePosition = objectPosition - origin;
			mapRelativePosition = new Vector2Int(Mathf.RoundToInt(relativePosition.x), Mathf.RoundToInt(relativePosition.z));

			if ((mapRelativePosition.x + objectSize.x) > map.Size.x || (mapRelativePosition.y + objectSize.y) > map.Size.y) {
				return false;
			}

			if (mapRelativePosition.x < 0 || mapRelativePosition.y < 0) {
				return false;
			}

			return true;
		}

		/// <summary>
		/// Get the Size of an obstacle in map unit.
		/// </summary>
		/// <param name="obstacle">The Obstacle in the world.</param>
		/// <returns>Returns a size in map unit.</returns>
		public static Vector2Int GetSizeFromObstacle(NavMeshObstacle obstacle) {
			return new Vector2Int(Mathf.RoundToInt(obstacle.size.x + 1), Mathf.RoundToInt(obstacle.size.z + 1));
		}

		/// <summary>
		/// Get all areas from a map indicated by the color of the pixel.
		/// </summary>
		/// <param name="map">The map to get areas from.</param>
		/// <param name="color">The color of the pixel to look out for.</param>
		/// <returns></returns>
		public static void GetAreasFromMap(Map map, Color color) {
			List<Color> colors = map.Texture.GetPixels(0, 0, map.Size.x, map.Size.y).ToList();
			List<Area> areas = new List<Area>();

			int x = 0;
			foreach (Color item in colors) {
				if (item == color) {
					Vector2Int origin = new Vector2Int(x % map.Size.x, x / map.Size.x);
					Area area = new Area(origin, new Vector2Int(1, 1));



				}
				x++;
			}


			// 2D to 1D conversion.
			// x = i + j * size

			// 1D to 2D conversion
			// i = x % size
			// j = x / size
		}
	}
}
