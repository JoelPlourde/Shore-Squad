using NodeSystem;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitTest {
	public class NodeTest {

		[Test]
		public void ReduceHealth_with_no_damage_test() {
			Actor actor = new Actor();
			Node node = new Node(100, 5, 5);
			node.ReduceHealth(actor, 5);
			Assert.That(node.Health, Is.EqualTo(100));
		}

		[Test]
		public void ReduceHealth_test() {
			Actor actor = new Actor();
			Node node = new Node(100, 5, 5);
			node.ReduceHealth(actor, 15);
			Assert.That(node.Health, Is.EqualTo(90));
		}

		[Test]
		public void CheckIfMultipleLootIfBigHit_test() {
			Actor actor = new Actor();
			Node node = new Node(100, 5, 5);
			node.ReduceHealth(actor, 150);
			Assert.That(node.Health, Is.EqualTo(0));
			Assert.That(node.Capacity, Is.EqualTo(0));
		}

		[Test]
		public void CheckIfCapacityIsRemovedAfterThreshold_test() {
			Actor actor = new Actor();
			Node node = new Node(100, 5, 5);
			node.ReduceHealth(actor, 25);
			Assert.That(node.Health, Is.EqualTo(80));
			Assert.That(node.Capacity, Is.EqualTo(4));
		}

		[Test]
		public void Constructor_test() {
			Node node = new Node(100, 5, 5);
			Assert.That(node.Health, Is.EqualTo(100));
			Assert.That(node.Durability, Is.EqualTo(5));
			Assert.That(node.Capacity, Is.EqualTo(5));
		}
	}
}
