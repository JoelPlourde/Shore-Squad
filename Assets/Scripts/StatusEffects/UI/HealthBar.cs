using UnityEngine;
using UnityEngine.UI;

namespace UI {
	namespace Portrait {
		public class HealthBar : MonoBehaviour {
			private Slider _slider;

			private void Awake() {
				_slider = GetComponent<Slider>();
			}

			public void Initialize(Actor actor) {
				UpdateHealth(actor.Attributes.Health / actor.Attributes.MaxHealth);
				actor.Attributes.OnUpdateHealthEvent += UpdateHealth;
			}

			public void OnDelete(Actor actor) {
				actor.Attributes.OnUpdateHealthEvent -= UpdateHealth;
			}

			public void UpdateHealth(float percentage) {
				_slider.value = percentage;
			}
		}
	}
}
