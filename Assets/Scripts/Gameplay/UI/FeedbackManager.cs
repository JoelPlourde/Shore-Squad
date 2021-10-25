using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay {
	public class FeedbackManager : MonoBehaviour {
		public static FeedbackManager Instance;

		private static readonly int CAPACITY = 10;

		public GameObject prefab;
		private CircularBuffer<FeedbackComponent> _circularBuffer = new CircularBuffer<FeedbackComponent>(CAPACITY);

		private void Awake() {
			Instance = this;

			for (int i = 0; i < CAPACITY; i++) {
				_circularBuffer.Insert(Instantiate(prefab, transform).GetComponent<FeedbackComponent>()).Disable();
			}
		}

		public void DisplayExperienceGain(Actor actor, int experience) {
			DisplayMessage(actor, "<color=#35fc03>+" + experience + "</color>");
		}

		public void DisplayError(Actor actor, string error) {
			DisplayMessage(actor, "<color=#ff000000>+" + error + "</color>");
		}

		public void DisplayMessage(Actor actor, string message) {
			_circularBuffer.Next().Enable(actor, message);
		}
	}
}
