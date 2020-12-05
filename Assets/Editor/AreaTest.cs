using GameSystem;
using NavigationSystem;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTest {
	[Test]
	public void InitializeArea_test() {
		Area area = new Area(new Vector2Int(0, 0), new Vector2Int(3, 4));
		Assert.That(area.Size.x, Is.EqualTo(3));
		Assert.That(area.Size.y, Is.EqualTo(4));
	}

	[Test]
	public void AreaIsLessOrEqualsWithRotation_test() {
		Area area = new Area(new Vector2Int(0, 0), new Vector2Int(3, 4));

		Assert.That(area.IsLessOrEquals(new Vector2Int(4, 3), out bool rotation1), Is.True);
		Assert.That(rotation1, Is.True);

		Assert.That(area.IsLessOrEquals(new Vector2Int(3, 4), out bool rotation2), Is.True);
		Assert.That(rotation2, Is.False);
	}

	[Test]
	public void RemoveObstacleFromArea_test() {
		GameObject @object = new GameObject();
		Obstacle obstacle = @object.AddComponent<Obstacle>();
		obstacle.RegisterBox(new Box(Vector3.zero, new Vector3(2, 0, 2)));

		Area area = new Area(new Vector2Int(2, 2), new Vector2Int(3, 4));

		List<Area> splitAreas = new List<Area>();
		Assert.That(Area.RemoveObstacleFromArea(area, obstacle, ref splitAreas), Is.True);

		Assert.That(splitAreas.Count, Is.EqualTo(2));
		Assert.That(splitAreas[0].Origin, Is.EqualTo(new Vector2Int(4, 2)));
		Assert.That(splitAreas[0].Size, Is.EqualTo(new Vector2Int(1, 2)));

		Assert.That(splitAreas[1].Origin, Is.EqualTo(new Vector2Int(2, 4)));
		Assert.That(splitAreas[1].Size, Is.EqualTo(new Vector2Int(3, 2)));
	}

	[Test]
	public void RemoveRectangleFromAreaWithRotation_test() {
		GameObject @object = new GameObject();
		Obstacle obstacle = @object.AddComponent<Obstacle>();
		obstacle.RegisterBox(new Box(Vector3.zero, new Vector3(4, 0, 2)));

		Area area = new Area(new Vector2Int(2, 2), new Vector2Int(3, 4));

		List<Area> splitAreas = new List<Area>();
		Assert.That(Area.RemoveObstacleFromArea(area, obstacle, ref splitAreas), Is.True);
		Assert.That(splitAreas.Count, Is.EqualTo(1));
		Assert.That(splitAreas[0].Origin, Is.EqualTo(new Vector2Int(4, 2)));
		Assert.That(splitAreas[0].Size, Is.EqualTo(new Vector2Int(1, 4)));
	}

	[Test]
	public void Equals_test() {
		Area area1 = new Area(new Vector2Int(0,0), new Vector2Int(1,1));
		Area area2 = new Area(new Vector2Int(0, 0), new Vector2Int(1, 1));
		Area area3 = new Area(new Vector2Int(0, 0), new Vector2Int(1, 2));

		Assert.That(area1.Equals(area2), Is.True);
		Assert.That(area1.Equals(area3), Is.False);
	}
}
