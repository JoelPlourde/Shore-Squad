using System;
using UnityEngine;

namespace NodeSystem {
	public class Node {

		private float _threshold = 0;
		private float _maxThreshold = 0;

		private Action<Actor> _onHarvestEvent;

		/// <summary>
		/// Construct a node with its core informations
		/// </summary>
		/// <param name="health">The healtbar of the node.</param>
		/// <param name="durability">The durability value of the node.</param>
		/// <param name="capacity">The capacity of the node initally.</param>
		public Node(int health, int durability, int capacity) {
			Health = health;
			Durability = durability;
			Capacity = capacity;

			_threshold = 0;
			_maxThreshold = Health / Capacity;
		}

		public Node(NodeData nodeData) : this(nodeData.Health, nodeData.Durability, nodeData.Capacity) {}

		/// <summary>
		/// Initialize the node with a callback for when the node is stricken.
		/// </summary>
		/// <param name="callback">The callback to call whenever the node gets hit.</param>
		public void Initialize(Action<Actor> callback) {
			_onHarvestEvent = callback;
		}

		/// <summary>
		/// Reduce the healthbar of this node
		/// </summary>
		/// <param name="damage">The raw damage inflicted by the character.</param>
		/// <returns>The remaining health of this node.</returns>
		public int ReduceHealth(Actor actor, int damage) {
			int calculatedDamage = GameplayUtils.CalculateDamageBasedOnStrength(Durability, damage, actor.Statistics.GetStatistic(ItemSystem.EquipmentSystem.StatisticType.LUCK));

			Health -= calculatedDamage;
			_threshold += calculatedDamage;

			while (_threshold >= _maxThreshold && Capacity > 0) {
				_threshold = _threshold - _maxThreshold;
				Capacity--;

				_onHarvestEvent?.Invoke(actor);
			}

			if (Health < 0) {
				Health = 0;
			}
			return Health;
		}

		/// <summary>
		/// Return whether or not the node is depleted.
		/// </summary>
		/// <returns>True if the node is depleted, else false.</returns>
		public bool IsDepleted() {
			return Health <= 0;
		}

		public int Health { get; private set; }
		public int Durability { get; private set; }
		public int Capacity { get; private set; }
	}
}
