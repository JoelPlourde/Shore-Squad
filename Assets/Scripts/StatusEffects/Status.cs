using System;

namespace StatusEffectSystem {
	public class Status {

		public Status(Actor actor, float magnitude, StatusEffectData statusEffectData) {
			Guid = Guid.NewGuid();
			Actor = actor;
			Magnitude = magnitude;
			StatusEffectData = statusEffectData;
		}

		public void ResetDuration() {
			Duration = StatusEffectData.Duration;
		} 

		public int IncreaseDuration(int value) {
			Duration += value;
			return Duration;
		}

		public int ReduceDuration() {
			Duration--;
			return Duration;
		}

		public int IncreaseStack() {
			Stack++;
			return Stack;
		}

		public int DecreaseStack() {
			Stack--;
			return Stack;
		}

		public Actor Actor { get; private set; }
		public StatusEffectData StatusEffectData { get; private set; }
		public float Magnitude { get; private set; }
		public int Duration { get; private set; }
		public int Stack { get; private set; }

		public Guid Guid { get; private set; }
	}
}
