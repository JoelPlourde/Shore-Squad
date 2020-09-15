using System.Collections.Generic;
using UnityEngine;
using FactionSystem;
using TaskSystem;
using System.Linq;
using System;

namespace TroopSystem {
	public class TroopBehaviour : MonoBehaviour {
		public FactionType FactionType;

		public List<Actor> Actors { get; private set; }

		private void Awake() {
			Actors = new List<Actor>(transform.GetComponentsInChildren<Actor>());
			CalculateAveragePosition();
		}

		/// <summary>
		/// Move every actors towards a position while staying in formation.
		/// </summary>
		/// <param name="position">Position to reach for.</param>
		public void MoveTowards(Vector3 position) {
			float minSpeed = Actors.Min(x => x.NavMeshAgent.speed);
			Actors.ForEach(x => {
				x.NavMeshAgent.speed = minSpeed;
				x.TaskScheduler.CreateTask<Move>(new MoveArguments(position + (x.transform.position - AveragePosition)));
			});
		}

		public void MoveTowards(Vector3 position, Action<TaskBehaviour> callback) {
			MoveTowards(position);
			Leader.TaskScheduler.OnEndTaskEvent += callback;
		}

		public void Unsubscribe(Action<TaskBehaviour> callback) {
			Leader.TaskScheduler.OnEndTaskEvent -= callback;
		}

		/// <summary>
		/// Calculate the average position of the troop
		/// </summary>
		private Vector3 CalculateAveragePosition() {
			Vector3 averagePosition = Vector3.zero;
			Actors.ForEach(x => averagePosition += x.transform.position);
			averagePosition /= Actors.Count;
			return averagePosition;
		}

		public Vector3 AveragePosition { get { return CalculateAveragePosition(); } }
		public Actor Leader { get { return Actors.First(); } }
	}
}
