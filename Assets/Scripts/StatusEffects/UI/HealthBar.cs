using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
	namespace Portrait {
		public class HealthBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
			private Slider _slider;
			private Text _text;

			private void Awake() {
				_slider = GetComponent<Slider>();
				_text = GetComponentInChildren<Text>();
				_text.gameObject.SetActive(false);
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

			public void OnPointerEnter(PointerEventData eventData) {
				_text.text = (_slider.value * 100f) + "%";
				_text.gameObject.SetActive(true);
			}

			public void OnPointerExit(PointerEventData eventData) {
				_text.gameObject.SetActive(false);
			}
		}
	}
}
