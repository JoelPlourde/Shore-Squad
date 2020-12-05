using UnityEngine;
using FactionSystem;
using System.Collections.Generic;
using TaskSystem;

namespace EncampmentSystem {
	public class Encampment : ZoneBehaviour {

		// TODO FIND A WAY TO POPULATE THIS LIST.
		public List<Actor> Workers;
		public Queue<ITaskArguments> Tasks = new Queue<ITaskArguments>();

		[SerializeField]
		public long coffer;							// The current value of currency in the coffers.

		[SerializeField]
		private FactionType _factionType = FactionType.FACTIONLESS;       // The Faction of the encampment.

		[SerializeField]
		private EncampmentType _encampmentType = EncampmentType.CAMP;       // Type of encampment

		[SerializeField]
		private Specification _specification = new Specification();

		[SerializeField]
		private Resources _resources = new Resources();

		public void AddToCoffer(int value) {
			coffer += value;
		}

		#region Tasks & Workers
		public void RegisterTask(ITaskArguments taskArguments) {
			Tasks.Enqueue(taskArguments);

			if (!IsInvoking()) {
				InvokeRepeating(nameof(Routine), 0f, Constant.TICK_RATE / 1000f);
			}
		}

		private void Routine() {
			Workers.ForEach(worker => {
				if (!worker.TaskScheduler.Busy) {
					if (Tasks.Count == 0) {
						CancelInvoke();
						return;
					} else {
						worker.TaskScheduler.CreateTask(Tasks.Dequeue());
					}
				}
			});
		}
		#endregion

		public Resources Resources { get { return _resources;  } }
		public Specification Specification { get { return _specification; } }
		public EncampmentType EncampmentType { get { return _encampmentType; } set { _encampmentType = value; } }
		public FactionType FactionType { get { return _factionType; } set { _factionType = value; } }
	}
}

