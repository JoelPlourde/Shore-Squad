using NavigationSystem;
using NUnit.Framework;
using UnityEngine;

namespace UnitTest {
	public class ObstacleTest {
		[Test]
		public void RegisterBox_test() {
			GameObject @object = new GameObject();
			@object.transform.position = new Vector3(255, 128, 255);
			Obstacle obstacle = @object.AddComponent<Obstacle>();
			Assert.That(obstacle.Boxes, Is.Not.EqualTo(null));
			Assert.That(obstacle.Boxes.Count, Is.EqualTo(0));

			obstacle.RegisterBox(new Box(new Vector3(1, 0, 1), new Vector3(2, 2, 2)));

			Assert.That(obstacle.Boxes.Count, Is.EqualTo(1));
			Assert.That(obstacle.Size, Is.EqualTo(new Vector3(2, 0, 2)));
			Assert.That(obstacle.Center, Is.EqualTo(new Vector3(256, 128, 256)));
		}

		[Test]
		public void RegisterBoxRotated90_test() {
			RegisterBoxRotated(90);
		}

		[Test]
		public void RegisterBoxRotated270_test() {
			RegisterBoxRotated(270);
		}

		private void RegisterBoxRotated(float value) {
			GameObject @object = new GameObject();
			@object.transform.position = new Vector3(255, 128, 255);
			@object.transform.Rotate(Vector3.up, value);
			Obstacle obstacle = @object.AddComponent<Obstacle>();
			Assert.That(obstacle.Boxes, Is.Not.EqualTo(null));
			Assert.That(obstacle.Boxes.Count, Is.EqualTo(0));

			obstacle.RegisterBox(new Box(new Vector3(1, 0, 1), new Vector3(3, 2, 4)));

			Assert.That(obstacle.Boxes.Count, Is.EqualTo(1));
			Assert.That(obstacle.Size, Is.EqualTo(new Vector3(4, 0, 3)));
			Assert.That(obstacle.Center, Is.EqualTo(new Vector3(256, 128, 256)));
		}
	}
}
