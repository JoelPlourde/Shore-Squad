using EncampmentSystem;
using NavigationSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace GameSystem {
	/// <summary>
	/// This class represents a 1:1 map of an area. 1 pixel on the map is equals to 1 unit in the world.
	/// </summary>
	[Serializable]
	public class Map {

		public readonly static Color[] Invalid = { Color.black, Color.red };

		[SerializeField]
		public Texture2D Texture { get; private set; }
		public Vector2Int Size { get; private set; }
		public int Count { get; private set; }

		/// <summary>
		/// Create a new square map.
		/// </summary>
		/// <param name="size">Size of the map. Size in X == Size in Y.</param>
		public Map(int size) : this(size, size) {}

		/// <summary>
		/// Create a new rectangle map.
		/// </summary>
		/// <param name="mapWidth">Size of the map in x.</param>
		/// <param name="mapHeight">Size of the map in y.</param>
		public Map(int mapWidth, int mapHeight) : this(mapWidth, mapHeight, Enumerable.Repeat(Color.white, mapWidth * mapHeight).ToArray()) {}

		/// <summary>
		/// Create a new map based on an existing texture.
		/// </summary>
		/// <param name="mapWidth">Size of the map in x.</param>
		/// <param name="mapHeight">Size of the map in y.</param>
		/// <param name="texture2D">Texture to take a copy of.</param>
		public Map(int mapWidth, int mapHeight, Texture2D texture2D) : this(mapWidth, mapHeight, texture2D.GetPixels(0, 0, mapWidth, mapHeight)) {}

		/// <summary>
		/// Create a new map based on an array of color representing pixels.
		/// </summary>
		/// <param name="mapWidth">Size of the map in x.</param>
		/// <param name="mapHeight">Size of the map in y.</param>
		/// <param name="pixels">The array of pixels color.</param>
		private Map(int mapWidth, int mapHeight, Color[] pixels) {
			Size = new Vector2Int(mapWidth, mapHeight);
			Texture = new Texture2D(Size.x, Size.y, TextureFormat.RGBA32, false) {
				filterMode = FilterMode.Point
			};
			Count = Size.x * Size.y;
			Texture.SetPixels(0, 0, Size.x, Size.y, pixels);
		}

		// TODO implement increazing the size of the map.

		/// <summary>
		/// Validate if the NavMeshObstacle can be placed unto the map.
		/// </summary>
		/// <param name="obstacle">The obstacle of the map.</param>
		/// <param name="worldPosition">The origin of where we want to place the object in world position.</param>
		/// <returns>Returns true if the origin position is valid, false if not.</returns>
		public bool IsPositionValid(Obstacle obstacle, ZoneBehaviour zoneBehaviour) {
			if (GetObstacleOriginRelativeToMap(this, zoneBehaviour.transform.position, obstacle, out Vector2Int relativePosition)) {
				return IsPositionValid(relativePosition, GetSizeFromObstacle(obstacle));
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

		/// <summary>
		/// Draw a rectangle on the map from an actual object.
		/// </summary>
		/// <param name="obstacle">The obstacle to draw a rectangle for.</param>
		/// <param name="zoneBehaviour">The zone where to draw the rectangle.</param>
		/// <param name="color">The color to represent on the map.</param>
		public void DrawRectangle(Obstacle obstacle, ZoneBehaviour zoneBehaviour, Color color) {
			if (GetObstacleOriginRelativeToMap(this, zoneBehaviour.transform.position, obstacle, out Vector2Int relativePosition)) {
				DrawRectangle(relativePosition, GetSizeFromObstacle(obstacle), color);
			} else {
				Debug.LogError("Couldn't draw the rectangle.");
			}
		}

		/// <summary>
		/// Get Colors at position in a 1d array.
		/// </summary>
		/// <param name="origin">Origin of the rectangle</param>
		/// <param name="rectangleSize">Size of the rectangle</param>
		/// <returns>Returns all colors in the rectangle as 1d array</returns>
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
		/// Get the map origin based on an reference object which is located in the center of the map in World position.
		/// </summary>
		/// <param name="map">The map to retrieve the origin from.</param>
		/// <param name="reference">The reference object on which the map is attached to.</param>
		/// <returns>Origin of the map in World position.</returns>
		public static Vector3 GetMapOriginInWorldPos(Map map, Vector3 reference) {
			return new Vector3(reference.x - (map.Size.x >> 1), 0, reference.z - (map.Size.y >> 1));
		}

		/// <summary>
		/// Get the origin of an obstacle in World Position.
		/// </summary>
		/// <param name="obstacle">The obstacle.</param>
		/// <returns>Origin of the obstacle in World Position</returns>
		public static Vector3 GetObstacleOriginInWorldPos(Obstacle obstacle) {
			Vector2Int rectangleSize = GetSizeFromObstacle(obstacle);
			return new Vector3(obstacle.transform.position.x - ((float)rectangleSize.x / 2), 0, obstacle.transform.position.z - ((float)rectangleSize.y / 2));
		}

		/// <summary>
		/// Get the obstacle origin relative to the map.
		/// </summary>
		/// <param name="map">The map</param>
		/// <param name="reference">The reference which the map is designated to.</param>
		/// <param name="obstacle">The obstacle.</param>
		/// <param name="relativePosition">The relative position of the obstacle unto the map.</param>
		/// <returns>Return True if the obstacle is within the bounds of the map. Else return false.</returns>
		public static bool GetObstacleOriginRelativeToMap(Map map, Vector3 reference, Obstacle obstacle, out Vector2Int relativePosition) {
			Vector3 origin = GetMapOriginInWorldPos(map, reference);
			Vector3 objectPos = GetObstacleOriginInWorldPos(obstacle);
			Vector3 pos = objectPos - origin;
			relativePosition = new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.z));

			if ((relativePosition.x + obstacle.Size.x) > map.Size.x || (relativePosition.y + obstacle.Size.y) > map.Size.y) {
				return false;
			}

			if (relativePosition.x < 0 || relativePosition.y < 0) {
				return false;
			}

			return true;
		}

		/// <summary>
		/// Get the obstacle position in World position.
		/// </summary>
		/// <param name="map">The map where the object is located.</param>
		/// <param name="reference">The reference which the map is designated to.</param>
		/// <param name="obstacle">The obstacle</param>
		/// <param name="area">The area where to place the obstacle.</param>
		/// <returns>Returns the obstacle position in world position. (Center of the object)</returns>
		public static Vector3 GetObstaclePositionInWorldPos(Map map, Vector3 reference, Obstacle obstacle, Area area) {
			Vector3 obstaclePosition = GetMapOriginInWorldPos(map, reference);      // Account for the map origin in World pos.
			obstaclePosition += new Vector3(area.Origin.x, 0, area.Origin.y);           // Account for the Area origin.
			obstaclePosition += obstacle.Size / 2;                                      // Account for the obstacle size.
			obstaclePosition.x = Mathf.RoundToInt(obstaclePosition.x);
			obstaclePosition.z = Mathf.RoundToInt(obstaclePosition.z);
			return obstaclePosition;
		}

		/// <summary>
		/// Get the Size of an obstacle in map unit.
		/// </summary>
		/// <param name="obstacle">The Obstacle in the world.</param>
		/// <returns>Returns a size in map unit.</returns>
		public static Vector2Int GetSizeFromObstacle(Obstacle obstacle) {
			return new Vector2Int(Mathf.RoundToInt(obstacle.Size.x), Mathf.RoundToInt(obstacle.Size.z));
		}

		/// <summary>
		/// Get all areas from a map indicated by the color of the pixel.
		/// </summary>
		/// <param name="map">The map to get areas from.</param>
		/// <returns></returns>
		public static List<Area> GetAreasFromMap(Map map) {
			return GetAreasFromMap(map, Color.blue);
		}

		/// <summary>
		/// Get all areas from a map indicated by the color of the pixel.
		/// </summary>
		/// <param name="map">The map to get areas from.</param>
		/// <returns></returns>
		public static List<Area> GetAreasFromMap(Map map, Color color) {
			List<Area> areas = new List<Area>();

			// Colors of the texture as a 2d array.
			Color[,] colors = ColorsToArray(map.Texture.GetPixels(0, 0, map.Size.x, map.Size.y), map.Size.x, map.Size.y);

			// Pixels on the map already handled by one of the rectangle and/or is not of the necessary color are marked as -1.
			int[,] indexes = new int[map.Size.x, map.Size.y];

			for (int i = 0; i < map.Size.x; i++) {
				for (int j = 0; j < map.Size.y; j++) {
					if (indexes[i,j] != -1 && colors[i,j] == color) {
						// Initialize the origin at the current position.
						Vector2Int origin = new Vector2Int(i, j);

						// Initialize a new area at current origin of size 1,1
						Area area = new Area(origin, new Vector2Int(1, 1));

						// Scan the diagonals to potential increase the size of the area.
						ScanDiagonals(indexes, colors, color, origin, map.Size.x, map.Size.y, ref area);

						// Scan up to potential increase the size up.
						ScanUpDirection(indexes, colors, color, origin, map.Size.x, map.Size.y, ref area);

						// Scan right to potential increase the size to the right.
						ScanRightDirection(indexes, colors, color, origin, map.Size.x, map.Size.y, ref area);

						// Mark all indexes within the area as invalid.
						for (int x = 0; x < area.Size.x; x++) {
							for (int y = 0; y < area.Size.y; y++) {
								indexes[x + area.Origin.x, y + area.Origin.y] = -1;
							}
						}
						areas.Add(area);
					} 
					else {
						indexes[i, j] = -1;
					}
				}
			}
			return areas;
		}

		/// <summary>
		/// Scan the diagonal direction and increases the size of the area if the condition is valid.
		/// </summary>
		/// <param name="indexes">The indexes of the cells.</param>
		/// <param name="colors">The colors of the cells.</param>
		/// <param name="origin">The origin where to begin the scanning operation.</param>
		/// <param name="size">The size of the map.</param>
		/// <param name="area">The area to be modified.</param>
		private static void ScanDiagonals(int[,] indexes, Color[,] colors, Color color, Vector2Int origin, int mapWidth, int mapHeight, ref Area area) {
			origin = new Vector2Int(origin.x + 1, origin.y + 1);

			if (origin.x < mapWidth && origin.y < mapHeight) {
				if (colors[origin.x, origin.y] != color) {
					return;
				}

				int leftCount = GetCount(origin, indexes, colors, color, mapWidth, mapHeight, o => { return (o.x > 0); }, o => { o.x--; return o; });
				int downCount = GetCount(origin, indexes, colors, color, mapWidth, mapHeight, o => { return (o.y > 0); }, o => { o.y--; return o; });
				if (leftCount > 0 && leftCount == downCount) {
					area.Size.x++;
					area.Size.y++;
					ScanDiagonals(indexes, colors, color, origin, mapWidth, mapHeight, ref area);
				} else {
					return;
				}
			}
		}

		/// <summary>
		/// Scan in the right-direction and increases the size of the area if the condition is valid.
		/// </summary>
		/// <param name="indexes">The indexes of the cells.</param>
		/// <param name="colors">The colors of the cells.</param>
		/// <param name="origin">The origin where to begin the scanning operation.</param>
		/// <param name="size">The size of the map.</param>
		/// <param name="area">The area to be modified.</param>
		private static void ScanRightDirection(int[,] indexes, Color[,] colors, Color color, Vector2Int origin, int mapWidth, int mapHeight, ref Area area) {
			origin = new Vector2Int(area.Origin.x + area.Size.x, area.Origin.y - 1);
			int count = GetCount(origin, indexes, colors, color, mapWidth, mapHeight, o => { return (o.y < mapHeight); }, o => { o.y++; return o; });
			if (count > 0 && area.Size.y == count) {
				area.Size.x++;
				origin = new Vector2Int(area.Origin.x + area.Size.x - 1, area.Origin.y);
				ScanRightDirection(indexes, colors, color, origin, mapWidth, mapHeight, ref area);
			} else {
				return;
			}
		}

		/// <summary>
		/// Scan in the up-direction and increases the size of the area if the condition is valid.
		/// </summary>
		/// <param name="indexes">The indexes of the cells.</param>
		/// <param name="colors">The colors of the cells.</param>
		/// <param name="origin">The origin where to begin the scanning operation.</param>
		/// <param name="size">The size of the map.</param>
		/// <param name="area">The area to be modified.</param>
		private static void ScanUpDirection(int[,] indexes, Color[,] colors, Color color, Vector2Int origin, int mapWidth, int mapHeight, ref Area area) {
			origin = new Vector2Int(area.Origin.x - 1, area.Origin.y + area.Size.y);
			int count = GetCount(origin, indexes, colors, color, mapWidth, mapHeight, o => { return (o.x < mapWidth); }, o => { o.x++; return o; });
			if (count > 0 && area.Size.x == count) {
				area.Size.y++;
				origin = new Vector2Int(area.Origin.x, area.Origin.y + area.Size.y - 1);
				ScanUpDirection(indexes, colors, color, origin, mapWidth, mapHeight, ref area);
			} else {
				return;
			}
		}

		/// <summary>
		/// Get the count of cells that respects the condition.
		/// </summary>
		/// <param name="origin">Origin where to begin the sequences.</param>
		/// <param name="indexes">The indexes of the cells.</param>
		/// <param name="colors">The colors of the cells.</param>
		/// <param name="condition">The condition to respect.</param>
		/// <param name="operation">The operation to be carried on the origin to traverse the 2d array.</param>
		/// <returns>Return the count of cells that respect the condition.</returns>
		private static int GetCount(Vector2Int origin, int[,] indexes, Color[,] colors, Color color, int width, int height, Func<Vector2Int, bool> condition, Func<Vector2Int, Vector2Int> operation) {
			int count = 0;
			while (condition(origin)) {
				origin = operation(origin);
				int length = Mathf.RoundToInt(Mathf.Sqrt(indexes.Length));
				// Limit check !
				if (origin.x >= width || origin.y >= height) {
					return count;
				}
				if (indexes[origin.x, origin.y] != -1 && colors[origin.x, origin.y] == color) {
					count++;
				} else {
					return count;
				}
			}
			return count;
		}

		/// <summary>
		/// Convert a List of color into a squared 2d array of colors.
		/// </summary>
		/// <param name="colors"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static Color[,] ColorsToArray(Color[] colors, int arrayWidth, int arrayHeight) {
			Color[,] array = new Color[arrayWidth, arrayHeight];
			int i = 0;
			int j = 0;
			foreach (var color in colors) {
				array[i, j] = color;
				i++;
				if (i % arrayWidth == 0) {
					i = 0;
					j++;
				}
			}
			return array;
		}
	}
}
