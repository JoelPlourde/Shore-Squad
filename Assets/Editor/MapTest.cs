using UnityEngine;
using NUnit.Framework;
using GameSystem;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;
using NavigationSystem;

public class MapTest {

	[Test]
	public void InitializeMap_test() {
		Map map = new Map(100);
		Assert.That(map.Size, Is.EqualTo(new Vector2Int(100, 100)));
		Assert.That(map.Count, Is.EqualTo(10000));
		Assert.That(map.Texture, Is.Not.EqualTo(null));
		Assert.That(map.IsPositionValid(new Vector2Int(0,0), new Vector2Int(100, 100)));
	}

	[Test]
	public void InitializeMapWithExistingTexture_test() {
		Color[] pixels = Enumerable.Repeat(Color.blue, 25 * 25).ToArray();
		Texture2D Texture = new Texture2D(25, 25, TextureFormat.RGBA32, false) {
			filterMode = FilterMode.Point
		};
		Map map = new Map(25, 25, Texture);
		Assert.That(map.Size, Is.EqualTo(new Vector2Int(25, 25)));
		Assert.That(map.Count, Is.EqualTo(25 * 25));
		Assert.That(map.Texture, Is.Not.EqualTo(null));
		Assert.That(map.IsPositionValid(new Vector2Int(0, 0), new Vector2Int(1, 1)));
	}

	[Test]
	public void DrawMap_test() {
		Map map = new Map(100);

		Vector2Int origin = new Vector2Int(50, 50);
		Vector2Int size = new Vector2Int(10, 10);
		map.DrawRectangle(origin, size, Color.cyan);
		Color[] colors = map.GetColorsAt(origin, size);
		foreach (var color in colors) {
			Assert.That(color, Is.EqualTo(Color.cyan));
		}
	}

	[Test]
	public void DrawFullMap_test() {
		Map map = new Map(100);
		Vector2Int origin = new Vector2Int(0, 0);
		Vector2Int size = new Vector2Int(100, 100);
		map.DrawRectangle(origin, size, Color.cyan);
		Color[] colors = map.GetColorsAt(origin, size);
		foreach (var color in colors) {
			Assert.That(color, Is.EqualTo(Color.cyan));
		}
	}

	[Test]
	public void DrawRectangleFromObstacle_test() {
		Map map = new Map(100);

		GameObject flag = new GameObject();
		GameObject @object = new GameObject();
		Obstacle obstacle = @object.AddComponent<Obstacle>();
		obstacle.RegisterBox(new Box(Vector3.zero, new Vector3(100, 0, 100)));
		Vector2Int objectSize = Map.GetSizeFromObstacle(obstacle);

		flag.transform.position = new Vector3(50, 0, 50);
		@object.transform.position = new Vector3(50, 0, 50);
		Assert.That(Map.GetObstacleOriginRelativeToMap(map, flag.transform.position, obstacle, out Vector2Int res), Is.True);
		Assert.That(res, Is.EqualTo(new Vector2Int(0, 0)));
	}

	[Test]
	public void ClearMap_Test() {
		Map map = new Map(100);
		map.DrawRectangle(new Vector2Int(50, 50), new Vector2Int(10, 10), Color.cyan);
		map.Clear();
		Color[] colors = map.GetColorsAt(new Vector2Int(0, 0), new Vector2Int(100, 100));
		foreach (var color in colors) {
			Assert.That(color, Is.EqualTo(Color.white));
		}
	}

	[Test]
	public void CheckPosition_test() {
		Map map = new Map(5);
		map.DrawRectangle(new Vector2Int(0, 0), new Vector2Int(1, 2), Color.red);
		map.DrawRectangle(new Vector2Int(4, 0), new Vector2Int(1, 5), Color.red);
		map.DrawRectangle(new Vector2Int(1, 2), new Vector2Int(3, 2), Color.red);
		Assert.That(map.IsPositionValid(new Vector2Int(0, 0), new Vector2Int(1, 2)), Is.False);
		Assert.That(map.IsPositionValid(new Vector2Int(4, 0), new Vector2Int(1, 5)), Is.False);
		Assert.That(map.IsPositionValid(new Vector2Int(1, 2), new Vector2Int(3, 2)), Is.False);
		Assert.That(map.IsPositionValid(new Vector2Int(0, 4), new Vector2Int(4, 1)), Is.True);
		Assert.That(map.IsPositionValid(new Vector2Int(0, 2), new Vector2Int(1, 3)), Is.True);
		Assert.That(map.IsPositionValid(new Vector2Int(1, 0), new Vector2Int(3, 2)), Is.True);
		Assert.That(map.IsPositionValid(new Vector2Int(0, 0), new Vector2Int(2, 4)), Is.False);
		Assert.That(map.IsPositionValid(new Vector2Int(0, 3), new Vector2Int(5, 2)), Is.False);
		Assert.That(map.IsPositionValid(new Vector2Int(0, 0), new Vector2Int(5, 5)), Is.False);
		Assert.That(map.IsPositionValid(new Vector2Int(2, 2), new Vector2Int(3, 3)), Is.False);
	}

	[Test]
	public void GetRectangleSizeFromObstacle_test() {
		GameObject @object = new GameObject();
		Obstacle obstacle = @object.AddComponent<Obstacle>();
		obstacle.RegisterBox(new Box(Vector3.zero, new Vector3(3, 0, 4)));
		Assert.That(Map.GetSizeFromObstacle(obstacle), Is.EqualTo(new Vector2Int(3, 4)));
	}

	[Test]
	public void GetRectangleSizeFromObstacleRotated_test() {
		GameObject @object = new GameObject();
		Obstacle obstacle = @object.AddComponent<Obstacle>();
		obstacle.transform.Rotate(Vector3.up, 90f);
		obstacle.RegisterBox(new Box(Vector3.zero, new Vector3(3, 0, 4)));
		Assert.That(Map.GetSizeFromObstacle(obstacle), Is.EqualTo(new Vector2Int(4, 3)));
	}

	[Test]
	public void GetMapPositionFrom_test() {
		Map map = new Map(100);

		GameObject flag = new GameObject();
		GameObject @object = new GameObject();
		Obstacle obstacle = @object.AddComponent<Obstacle>();
		obstacle.RegisterBox(new Box(Vector3.zero, new Vector3(3, 0, 4)));
		Vector2Int objectSize = Map.GetSizeFromObstacle(obstacle);

		flag.transform.position = new Vector3(50, 0, 50);
		@object.transform.position = new Vector3(25, 0, 25);
		Assert.That(Map.GetObstacleOriginRelativeToMap(map, flag.transform.position, obstacle, out Vector2Int res1), Is.True);
		Assert.That(res1, Is.EqualTo(new Vector2Int(23, 23)));

		flag.transform.position = new Vector3(125, 0, 150);
		@object.transform.position = new Vector3(112, 0, 130);
		Assert.That(Map.GetObstacleOriginRelativeToMap(map, flag.transform.position, obstacle, out Vector2Int res2), Is.True);
		Assert.That(res2, Is.EqualTo(new Vector2Int(35, 28)));

		flag.transform.position = new Vector3(-147, 0, 163);
		@object.transform.position = new Vector3(-165, 0, 130);
		Assert.That(Map.GetObstacleOriginRelativeToMap(map, flag.transform.position, obstacle, out Vector2Int res3), Is.True);
		Assert.That(res3, Is.EqualTo(new Vector2Int(30, 15)));

		flag.transform.position = new Vector3(50, 0, 50);
		@object.transform.position = new Vector3(100, 0, 100);
		Assert.That(Map.GetObstacleOriginRelativeToMap(map, flag.transform.position, obstacle, out Vector2Int res4), Is.False);

		flag.transform.position = new Vector3(50, 0, 50);
		@object.transform.position = new Vector3(98, 0, 96);
		Assert.That(Map.GetObstacleOriginRelativeToMap(map, flag.transform.position, obstacle, out Vector2Int res5), Is.True);
	}

	[Test]
	public void GetAreasFromMapSquare_test() {
		Map map = new Map(25);
		map.DrawRectangle(new Vector2Int(0, 0), new Vector2Int(3, 3), Color.blue);

		List<Area> areas = Map.GetAreasFromMap(map);
		Assert.That(areas.Count, Is.EqualTo(1));
		Assert.That(areas[0].Size, Is.EqualTo(new Vector2Int(3, 3)));
		Assert.That(areas[0].Origin, Is.EqualTo(new Vector2Int(0, 0)));
	}

	[Test]
	public void GetAreasFromMapRightRectangle_test() {
		Map map = new Map(25);
		map.DrawRectangle(new Vector2Int(0, 0), new Vector2Int(5, 3), Color.blue);

		List<Area> areas = Map.GetAreasFromMap(map);
		Assert.That(areas.Count, Is.EqualTo(1));
		Assert.That(areas[0].Size, Is.EqualTo(new Vector2Int(5, 3)));
		Assert.That(areas[0].Origin, Is.EqualTo(new Vector2Int(0, 0)));
	}

	[Test]
	public void GetAreasFromMapUpRectangle_test() {
		Map map = new Map(25);
		map.DrawRectangle(new Vector2Int(0, 0), new Vector2Int(3, 4), Color.blue);

		List<Area> areas = Map.GetAreasFromMap(map);
		foreach (var item in areas) {
			Debug.Log("Origin: " + item.Origin + " and " + item.Size);
		}


		Assert.That(areas.Count, Is.EqualTo(1));
		Assert.That(areas[0].Size, Is.EqualTo(new Vector2Int(3, 4)));
		Assert.That(areas[0].Origin, Is.EqualTo(new Vector2Int(0, 0)));
	}


	[Test]
	public void GetAreasFromMap_test() {
		Map map = new Map(25);
		map.DrawRectangle(new Vector2Int(0, 0), new Vector2Int(2, 2), Color.blue);
		map.DrawRectangle(new Vector2Int(3, 0), new Vector2Int(1, 2), Color.blue);
		map.DrawRectangle(new Vector2Int(1, 3), new Vector2Int(2, 2), Color.blue);

		map.DrawRectangle(new Vector2Int(0, 4), new Vector2Int(1, 1), Color.blue);
		map.DrawRectangle(new Vector2Int(3, 2), new Vector2Int(2, 1), Color.blue);
		map.DrawRectangle(new Vector2Int(4, 4), new Vector2Int(1, 1), Color.blue);

		List<Area> areas = Map.GetAreasFromMap(map);
		foreach (Area area in areas) {
			Debug.Log("Origin: " + area.Origin + " and size: " + area.Size);
		}
		Assert.That(areas.Count, Is.EqualTo(6));
	}

	[Test]
	public void GetAreasFromMapPerimeter_test() {
		Map map = new Map(5);
		map.DrawRectangle(new Vector2Int(0, 0), new Vector2Int(1, 5), Color.blue);
		map.DrawRectangle(new Vector2Int(0, 0), new Vector2Int(5, 1), Color.blue);
		map.DrawRectangle(new Vector2Int(0, 4), new Vector2Int(5, 1), Color.blue);
		map.DrawRectangle(new Vector2Int(4, 0), new Vector2Int(1, 5), Color.blue);

		List<Area> areas = Map.GetAreasFromMap(map);
		foreach (Area area in areas) {
			Debug.Log("Origin: " + area.Origin + " and size: " + area.Size);
		}
		Assert.That(areas.Count, Is.EqualTo(4));
	}

	[Test]
	public void GetAreasFromMapCorners_test() {
		Map map = new Map(25);
		map.DrawRectangle(new Vector2Int(0, 0), new Vector2Int(2, 2), Color.blue);
		map.DrawRectangle(new Vector2Int(0, 23), new Vector2Int(2, 2), Color.blue);
		map.DrawRectangle(new Vector2Int(23, 0), new Vector2Int(2, 2), Color.blue);
		map.DrawRectangle(new Vector2Int(23, 23), new Vector2Int(2, 2), Color.blue);

		List<Area> areas = Map.GetAreasFromMap(map);
		foreach (Area area in areas) {
			Debug.Log("Origin: " + area.Origin + " and size: " + area.Size);
		}
		Assert.That(areas.Count, Is.EqualTo(4));
	}

	[Test]
	public void ColorsToArray_test() {
		Map map = new Map(5);

		map.DrawRectangle(new Vector2Int(0, 0), new Vector2Int(2, 2), Color.blue);
		map.DrawRectangle(new Vector2Int(2, 2), new Vector2Int(2, 2), Color.blue);

		Color[,] colors = Map.ColorsToArray(map.Texture.GetPixels(0, 0, map.Size.x, map.Size.y), map.Size.x, map.Size.y);
		Assert.That(colors[0, 0], Is.EqualTo(Color.blue));
		Assert.That(colors[0, 1], Is.EqualTo(Color.blue));
		Assert.That(colors[1, 0], Is.EqualTo(Color.blue));
		Assert.That(colors[1, 1], Is.EqualTo(Color.blue));

		Assert.That(colors[2, 2], Is.EqualTo(Color.blue));
		Assert.That(colors[2, 3], Is.EqualTo(Color.blue));
		Assert.That(colors[3, 2], Is.EqualTo(Color.blue));
		Assert.That(colors[3, 3], Is.EqualTo(Color.blue));
	}

	[Test]
	public void GetMapOrigin_test() {
		Map map = new Map(20, 20);
		Vector3 reference = new Vector3(256, 0, 256);
		Vector3 mapOriginInWorldPos = Map.GetMapOriginInWorldPos(map, reference);
		Assert.That(mapOriginInWorldPos, Is.EqualTo(new Vector3(246, 0, 246)));
	}
}
