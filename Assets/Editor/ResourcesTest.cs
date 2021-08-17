using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using EncampmentSystem;
using UnityEngine;

namespace UnitTest {
	public class ResourcesTest {
		[Test]
		public void AddResources_test() {
			string res1 = "res1";
			string res2 = "res2";

			EncampmentSystem.Resources resources = new EncampmentSystem.Resources();
			resources.AddResources(res1, 10);
			resources.AddResources(res2, 5);
			resources.AddResources(res1, 10);

			Assert.That(resources.GetResourcesValue(res1), Is.EqualTo(20));
			Assert.That(resources.GetResourcesValue(res2), Is.EqualTo(5));
		}

		[Test]
		public void RemoveResources_test() {
			string res1 = "res1";
			EncampmentSystem.Resources resources = new EncampmentSystem.Resources();
			resources.AddResources(res1, 20);
			resources.RemoveResources(res1, 10);
			Assert.That(resources.GetResourcesValue(res1), Is.EqualTo(10));
		}

		[Test]
		public void RemoveNegativeResources_test() {
			Assert.Throws<UnityException>(delegate () {
				new EncampmentSystem.Resources().RemoveResources("res", 10);
			});
		}

		[Test]
		public void GetResources_test() {
			EncampmentSystem.Resources resources = new EncampmentSystem.Resources();
			resources.AddResources("res", 10);
			Assert.That(resources.GetResourcesValue("res"), Is.EqualTo(10));
		}

		[Test]
		public void GetMissingResources_test() {
			EncampmentSystem.Resources resources = new EncampmentSystem.Resources();
			Assert.That(resources.GetResourcesValue("res"), Is.EqualTo(0));
		}
	}
}
