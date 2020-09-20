using GameSystem;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTest
{
	[Test]
	public void InitializeArea_test() {
		Area area = new Area(new Vector2Int(0, 0), new Vector2Int(3, 4));
		Assert.That(area.Size.x, Is.EqualTo(3));
		Assert.That(area.Size.y, Is.EqualTo(4));
	}

	[Test]
	public void AreaIsLessOrEquals_test() {
		Area area = new Area(new Vector2Int(0, 0), new Vector2Int(3, 4));
		Assert.That(area.IsLessOrEquals(new Vector2Int(2, 2)), Is.True);
		Assert.That(area.IsLessOrEquals(new Vector2Int(1, 2)), Is.True);
		Assert.That(area.IsLessOrEquals(new Vector2Int(1, 1)), Is.True);
		Assert.That(area.IsLessOrEquals(new Vector2Int(3, 3)), Is.True);
		Assert.That(area.IsLessOrEquals(new Vector2Int(3, 4)), Is.True);
		Assert.That(area.IsLessOrEquals(new Vector2Int(4, 4)), Is.False);
		Assert.That(area.IsLessOrEquals(new Vector2Int(4, 3)), Is.False);
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
	public void RemoveRectangleFromArea_test() {
		Area area = new Area(new Vector2Int(2, 2), new Vector2Int(3, 4));

		List<Area> splitAreas = new List<Area>();
		Assert.That(Area.RemoveRectangleFromArea(area, new Vector2Int(2, 2), ref splitAreas), Is.True);

		Assert.That(splitAreas.Count, Is.EqualTo(2));
		Assert.That(splitAreas[0].Origin, Is.EqualTo(new Vector2Int(4, 2)));
		Assert.That(splitAreas[0].Size, Is.EqualTo(new Vector2Int(1, 2)));

		Assert.That(splitAreas[1].Origin, Is.EqualTo(new Vector2Int(2, 4)));
		Assert.That(splitAreas[1].Size, Is.EqualTo(new Vector2Int(3, 2)));
	}

	[Test]
	public void RemoveRectangleFromAreaWithRotation_test() {
		Area area = new Area(new Vector2Int(2, 2), new Vector2Int(3, 4));

		List<Area> splitAreas = new List<Area>();
		Assert.That(Area.RemoveRectangleFromArea(area, new Vector2Int(4, 2), ref splitAreas), Is.True);
		Assert.That(splitAreas.Count, Is.EqualTo(1));
		Assert.That(splitAreas[0].Origin, Is.EqualTo(new Vector2Int(4, 2)));
		Assert.That(splitAreas[0].Size, Is.EqualTo(new Vector2Int(1, 4)));
	}
}
