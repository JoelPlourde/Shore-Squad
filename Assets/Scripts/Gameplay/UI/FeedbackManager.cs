using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay {
	public class FeedbackManager : MonoBehaviour {
		public static FeedbackManager Instance;

		public GameObject prefab;

		private int _index = 0;
		private FeedbackComponent[] _feedbacks = new FeedbackComponent[10];

		private void Awake() {
			Instance = this;

			for (int i = 0; i < _feedbacks.Length; i++) {
				_feedbacks[i] = Instantiate(prefab, transform).GetComponent<FeedbackComponent>();
				_feedbacks[i].Disable();
			}
		}

		public void DisplayExperienceGain(Actor actor, int experience) {
			DisplayMessage(actor, "<color=#ff0000ff>+" + experience + "</color>");
		}

		public void DisplayError(Actor actor, string error) {
			DisplayMessage(actor, "<color=#FF0000>" + error + "</color>");
		}

		public void DisplayMessage(Actor actor, string message) {
			_feedbacks[_index].Enable(actor, message);
			_index++;
			_index %= _feedbacks.Length;
		}
	}
}
