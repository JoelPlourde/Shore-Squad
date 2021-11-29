using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

namespace CraftingSystem {
	namespace UI {
		[RequireComponent(typeof(RectTransform))]
		public class PowerBar : MonoBehaviour {

			private Image _image;
			private bool _forceRefreshed = false;

			private LTDescr description;

			private void Start() {
				_image = GetComponent<Image>();
				_image.type = Type.Filled;
				_image.fillMethod = FillMethod.Vertical;
				_image.fillOrigin = 0;
			}

			public void Initialize(FillMethod fillMethod) {
				_image.fillMethod = fillMethod;
			}

			public void ForceUpdateValue(float percentage) {
				LeanTween.cancel(gameObject);
				_image.fillAmount = percentage;
			}

			public void UpdateValue(float percentage) {
				description = LeanTween.value(gameObject, _image.fillAmount, percentage, 1f).setOnUpdate((float value) => {
					_image.fillAmount = value;
				});
			}
		}
	}
}