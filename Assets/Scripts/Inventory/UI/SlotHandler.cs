using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ItemSystem {
	namespace UI {
		[RequireComponent(typeof(Image))]
		public class SlotHandler : MonoBehaviour {

			private Image _image;
			private Text _text;

			private void Awake() {
				_image = transform.Find("Item").GetComponent<Image>();
				_text = GetComponentInChildren<Text>();

				_image.rectTransform.sizeDelta = new Vector2(45, 45);
			}

			public void Refresh(Item item) {
				if (ReferenceEquals(item, null)) {
					Enable(false);
					return;
				}

				_image.sprite = item.ItemData.Sprite;
				_text.text = (item.Amount > 1) ? item.Amount.ToString() : "";
				Enable(true);
			}

			public void Enable(bool enable) {
				_image.enabled = enable;
				_text.enabled = enable;
			}
		}
	}
}
