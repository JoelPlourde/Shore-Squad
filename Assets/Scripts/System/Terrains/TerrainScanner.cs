using EncampmentSystem;
using GameSystem;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public static class TerrainScanner
{
	/// <summary>
	/// Check if the zone is within a terrain.
	/// </summary>
	/// <param name="terrain">The Terrain</param>
	/// <param name="zoneBehaviour">The Zone.</param>
	/// <returns>True if the zone is within the terrain, false if not.</returns>
	public static bool CheckIfZoneIsWithinTerrain(Terrain terrain, Vector3 position, int size) {
		Vector3 relativePosition = position - terrain.transform.position;
		float halfSize = size >> 1;
		if ((relativePosition.x - halfSize) < 0 || (relativePosition.x + halfSize) > terrain.terrainData.size.x) {
			return false;
		}

		if ((relativePosition.z - halfSize) < 0 || (relativePosition.z + halfSize) > terrain.terrainData.size.z) {
			return false;
		}

		return true;
	}

	/// <summary>
	/// Map the terrain unto a map.
	/// </summary>
	/// <param name="terrain">Terrain to map.</param>
	/// <param name="position">The position of the terrain to start from.</param>
	/// <param name="size">The size of the area to map.</param>
	/// <param name="map">The map that will be generated.</param>
	/// <returns>Return True if the map had been able to be mapped, else false.</returns>
	public static bool GetTerrainMap(Terrain terrain, Vector3 position, int size, out Map map) {
		map = new Map(size);
		if (!CheckIfZoneIsWithinTerrain(terrain, position, size)) {
			return false;
		}

		Vector3 relativePosition = position - terrain.transform.position;
		Vector2Int relativeOrigin = new Vector2Int(
			Mathf.RoundToInt((int) relativePosition.x - (map.Size.x >> 1)),
			Mathf.RoundToInt((int) relativePosition.z - (map.Size.y >> 1))
		);
		Color[] pixels = Enumerable.Repeat(Color.white, map.Count).ToArray();
		for (int i = relativeOrigin.x; i < (relativeOrigin.x + map.Size.x); i++) {
			for (int j = relativeOrigin.y; j < (relativeOrigin.y + map.Size.y); j++) {
				float x_01 = i / (float)terrain.terrainData.heightmapWidth;
				float y_01 = j / (float)terrain.terrainData.heightmapHeight;

				if (terrain.terrainData.GetSteepness(x_01, y_01) > Constant.AGENT_MAX_SLOPE) {
					pixels[((i - relativeOrigin.x)) + ((j - relativeOrigin.y) * map.Size.x)] = Color.black;
				}
			}
		}
		map.Texture.SetPixels(pixels);
		map.Texture.Apply();
		return true;
	}

	/// <summary>
	/// Get Terrain at the world position
	/// </summary>
	/// <param name="position">Position to check where there is a terrain.</param>
	/// <param name="terrain">Terrain that is returned if found.</param>
	/// <returns>Return true if a terrain has been found, else false.</returns>
	public static bool GetTerrainAtWorldPosition(Vector3 position, out Terrain terrain) {
		Ray ray = new Ray(position + Vector3.up * 600f, Vector3.down);
		int layer = 1 << 8;

		RaycastHit[] colliders = Physics.RaycastAll(ray, Mathf.Infinity, layer);
		foreach (var item in colliders) {
			terrain = item.collider.GetComponent<Terrain>();
			return true;
		}
		Debug.Log("A terrain couldn't be found at: " + position + ", have you assigned the terrain with the Terrain layer ?");
		terrain = default;
		return false;
	}
}
