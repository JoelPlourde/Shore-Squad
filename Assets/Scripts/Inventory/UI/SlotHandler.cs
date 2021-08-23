using ItemSystem.EffectSystem;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ItemSystem {
	namespace UI {
		[RequireComponent(typeof(Image))]
		public class SlotHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

			private Item _item;
			private Text _text;

			private void Awake() {
				Image = transform.Find("Item").GetComponent<Image>();
				_text = GetComponentInChildren<Text>();

				Image.rectTransform.sizeDelta = new Vector2(45, 45);
			}

			public void Refresh(Item item) {
				if (ReferenceEquals(item, null)) {
					Enable(false);
					return;
				}

				_item = item;
				Image.sprite = item.ItemData.Sprite;
				_text.text = (item.Amount > 1) ? item.Amount.ToString() : "";
				Enable(true);
			}

			public void Enable(bool enable) {
				Image.enabled = enable;
				_text.enabled = enable;

				if (!enable) {
					_item = null;
				}
			}

			#region Pointer
			public void OnPointerEnter(PointerEventData eventData) {
				if (!ReferenceEquals(_item, null)) {
					Tooltip.Instance.ShowTooltip(_item.ItemData.Tooltip);
				} else {
					Tooltip.Instance.HideTooltip();
				}
			}

			public void OnPointerExit(PointerEventData eventData) {
				if (!ReferenceEquals(_item, null)) {
					Tooltip.Instance.HideTooltip();
				}
			}

			public void OnPointerClick(PointerEventData eventData) {
				if (HasItem) {
					if (eventData.clickCount == 2) {
						if (Squad.FirstSelected(out Actor actor)) {
							ItemEffectFactory.Activate(actor, _item);

							ItemSelector.UnselectItem();
						}
					} else {
						ItemSelector.SelectItem(this);
					}
				}
			}
			#endregion

			public Image Image { get; private set; }

			public bool HasItem { get { return !ReferenceEquals(_item, null); } }
		}
	}
}
