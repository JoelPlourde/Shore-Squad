using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ItemSystem {
	namespace UI {
		[RequireComponent(typeof(Image))]
		public class SlotHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

			private Item _item;
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

				_item = item;
				_image.sprite = item.ItemData.Sprite;
				_text.text = (item.Amount > 1) ? item.Amount.ToString() : "";
				Enable(true);
			}

			public void Enable(bool enable) {
				_image.enabled = enable;
				_text.enabled = enable;

				if (!enable) {
					_item = null;
				}
			}

			public void OnPointerEnter(PointerEventData eventData) {
				if (!ReferenceEquals(_item, null)) {
					Tooltip.Instance.ShowTooltip(_item.ItemData.Tooltip);
				}
			}

			public void OnPointerExit(PointerEventData eventData) {
				if (!ReferenceEquals(_item, null)) {
					Tooltip.Instance.HideTooltip();
				}
			}
		}
	}
}
