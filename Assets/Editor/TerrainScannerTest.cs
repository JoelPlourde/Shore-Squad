using GameSystem;
using NUnit.Framework;
using UnityEngine;

public class TerrainScannerTest
{
	[Test]
	public void CheckIfZoneIsWithinTerrain_test() {
		TerrainData terrainData = new TerrainData {
			size = new Vector3(512, 512, 512)
		};
		Terrain terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
		terrain.transform.position = Vector3.zero;

		Assert.That(TerrainScanner.CheckIfZoneIsWithinTerrain(terrain, new Vector3(256, 0, 256), 100, 100), Is.True);
		Assert.That(TerrainScanner.CheckIfZoneIsWithinTerrain(terrain, new Vector3(0, 0, 0), 100, 100), Is.False);
		Assert.That(TerrainScanner.CheckIfZoneIsWithinTerrain(terrain, new Vector3(256, 0, 0), 100, 100), Is.False);
		Assert.That(TerrainScanner.CheckIfZoneIsWithinTerrain(terrain, new Vector3(0, 0, 256), 100, 100), Is.False);
		Assert.That(TerrainScanner.CheckIfZoneIsWithinTerrain(terrain, new Vector3(50, 0, 50), 100, 100), Is.True);
	}

	[Test]
	public void GetTerrainMap_test() {
		TerrainData terrainData = new TerrainData {
			size = new Vector3(512, 512, 512),
			heightmapResolution = 512
		};
		Terrain terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
		float[,] heights = terrain.terrainData.GetHeights(0, 0, 512, 512);
		terrain.transform.position = Vector3.zero;

		heights[255, 255] = 100;
		terrain.terrainData.SetHeights(0, 0, heights);

		Assert.That(TerrainScanner.GetTerrainMap(terrain, new Vector3(256, 0, 256), 100, 100, out Map map), Is.True);
		Assert.That(map.IsPositionValid(new Vector2Int(50, 50), new Vector2Int(1, 1)), Is.False);
		Assert.That(map.IsPositionValid(new Vector2Int(51, 51), new Vector2Int(1, 1)), Is.True);
		Assert.That(map.IsPositionValid(new Vector2Int(48, 48), new Vector2Int(1, 1)), Is.True);
	}
}
