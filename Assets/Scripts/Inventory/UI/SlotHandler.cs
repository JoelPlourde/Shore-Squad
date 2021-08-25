using ItemSystem.EffectSystem;
using PointerSystem;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ItemSystem {
	namespace UI {
		[RequireComponent(typeof(Image))]
		public class SlotHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

			private Item _item;

			private void Awake() {
				Image = transform.Find("Item").GetComponent<Image>();
				Amount = GetComponentInChildren<Text>();

				Image.rectTransform.sizeDelta = new Vector2(45, 45);
			}

			public void Refresh(Item item) {
				if (ReferenceEquals(item, null)) {
					Enable(false);
					return;
				}

				_item = item;
				Image.sprite = item.ItemData.Sprite;
				Amount.text = (item.Amount > 1) ? item.Amount.ToString() : "";
				Enable(true);
			}

			public void Enable(bool enable) {
				Image.enabled = enable;
				Amount.enabled = enable;

				if (!enable) {
					_item = null;
				}
			}

			#region Pointer
			public void OnPointerEnter(PointerEventData eventData) {
				if (!ReferenceEquals(_item, null)) {
					Tooltip.Instance.ShowTooltip(_item.ItemData.Tooltip, Constant.TOOLTIP_DELAY);
				} else {
					Tooltip.Instance.HideTooltip();
				}
			}

			public void OnPointerExit(PointerEventData eventData) {
				if (!ReferenceEquals(_item, null)) {
					Tooltip.Instance.HideTooltip();
				}
			}

			public void OnPointerUp(PointerEventData eventData) {
				PointerManager.Instance.SetPointer(PointerMode.DEFAULT);

				if (_dragging) {
					return;
				}

				if (HasItem) {
					if (Squad.FirstSelected(out Actor actor)) {
						ItemEffectFactory.Activate(actor, _item);

						if (!HasItem) {
							Tooltip.Instance.HideTooltip();
						}
					}
				}
			}

			public void OnPointerDown(PointerEventData eventData) {
				PointerManager.Instance.SetPointer(PointerMode.DEFAULT_PRESSED);
			}
			#endregion

			#region Drag
			public bool _dragging = false;

			public void OnBeginDrag(PointerEventData eventData) {
				_dragging = true;

				Image.transform.SetParent(transform.parent);
				Amount.transform.SetParent(Image.transform);
			}

			public void OnDrag(PointerEventData eventData) {
				Image.rectTransform.position = Input.mousePosition;
			}

			public void OnEndDrag(PointerEventData eventData) {
				_dragging = false;

				var raycastResults = new List<RaycastResult>();
				EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = Input.mousePosition }, raycastResults);

				if (raycastResults.Count > 0) {
					bool handled = false;
					bool remainsInInventory = false;
					foreach (var result in raycastResults) {
						if (ReferenceEquals(result.gameObject.transform, transform.parent)) {
							remainsInInventory = true;
						}

						if (ReferenceEquals(result.gameObject.transform.parent, transform.parent) && !ReferenceEquals(result.gameObject, Image.gameObject)) {
							handled = true;
							if (Squad.FirstSelected(out Actor actor)) {
								ReplaceImage(eventData.pointerDrag.transform);
								actor.Inventory.SwapItems(eventData.pointerDrag.transform.GetSiblingIndex() - 1, result.gameObject.transform.GetSiblingIndex() - 1);
							}
						}
					}

					if (!handled) {
						if (remainsInInventory) {
							ReplaceImage(eventData.pointerDrag.transform);
						} else {
							// TODO implement drop the object in the world if not dropped on any other items.
							throw new UnityException("This functionality has not been implemented yet!");
						}
					}
				}
			}
			#endregion

			private void ReplaceImage(Transform parent) {
				Image.transform.SetParent(parent);
				Image.transform.SetAsFirstSibling();
				Image.rectTransform.sizeDelta = new Vector2(45, 45);
				Image.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
				Image.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
				Image.rectTransform.localPosition = Vector2.zero;

				Amount.rectTransform.SetParent(parent, true);
			}

			public Image Image { get; private set; }
			public Text Amount { get; private set; }

			public bool HasItem { get { return !ReferenceEquals(_item, null); } }
		}
	}
}
