using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitTest {
	public class MapUtilsTest {

		[Test]
		public void RoundToNearest_test() {
			Assert.That(MapUtils.RoundToNearest(12.4534f), Is.EqualTo(12.5f));
			Assert.That(MapUtils.RoundToNearest(12.2312390f), Is.EqualTo(12.0f));
			Assert.That(MapUtils.RoundToNearest(12.2712390f), Is.EqualTo(12.5f));
			Assert.That(MapUtils.RoundToNearest(12.5543f), Is.EqualTo(12.5f));
			Assert.That(MapUtils.RoundToNearest(12.8742f), Is.EqualTo(13.0f));
		}

		[Test]
		public void GetMapPositionFromWorldPosition_test() {
			Assert.That(MapUtils.GetMapPositionFromWorldPosition(new Vector3(245.67f, 45.23f, 4503.432f)), Is.EqualTo(new Vector3(245.5f, 45.0f, 4503.5f)));
		}
	}
}
