using ItemSystem.EffectSystem;
using PointerSystem;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ItemSystem {
	namespace UI {
		[RequireComponent(typeof(Image))]
		public class SlotHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

			public event Action OnBeginDragEvent;
			public event Action OnEndDragEvent;

			private void Awake() {
				Image = transform.Find("Item").GetComponent<Image>();
				Amount = GetComponentInChildren<Text>();

				Image.rectTransform.sizeDelta = new Vector2(45, 45);
			}

			public void Initialize(IContainer container, bool readOnly, int id) {
				Container = container;
				ReadOnly = readOnly;
				ID = id;
			}

			public void Refresh(Item item) {
				if (ReferenceEquals(item, null)) {
					Disable();
					return;
				}

				Item = item;
				Image.sprite = item.ItemData.Sprite;
				Amount.text = (item.Amount > 1) ? item.Amount.ToString() : "";
				Enable();
			}

			public void Enable() {
				Image.enabled = true;
				Amount.enabled = true;
			}

			public void Disable() {
				Image.enabled = false;
				Amount.enabled = false;

				Item = null;
			}

			#region Pointer
			public void OnPointerEnter(PointerEventData eventData) {
				if (!ReferenceEquals(Item, null)) {
					Tooltip.Instance.ShowTooltip(Item.ItemData.Tooltip, Constant.TOOLTIP_DELAY);
				} else {
					Tooltip.Instance.HideTooltip();
				}
			}

			public void OnPointerExit(PointerEventData eventData) {
				if (!ReferenceEquals(Item, null)) {
					Tooltip.Instance.HideTooltip();
				}
			}

			public void OnPointerUp(PointerEventData eventData) {
				PointerManager.Instance.SetPointer(PointerMode.DEFAULT);

				if (_dragging) {
					return;
				}

				if (Input.GetMouseButtonUp(1)) {
					OptionsHandler.Instance.Open(this);
					return;
				}

				if (HasItem) {
					if (Squad.FirstSelected(out Actor actor)) {
						if (ReadOnly) {
							if (InventoryUtils.TransferItem(Container.Inventory, actor.Inventory, Item.ItemData, Item.Amount)) {
								// TODO Give Experience based on the Recipe.

								Tooltip.Instance.HideTooltip();

								OnEndDragEvent?.Invoke();
							}
						} else {
							ItemEffectFactory.Activate(actor, Item);

							if (!HasItem) {
								Tooltip.Instance.HideTooltip();
							}
						}
					}
				}

				ItemSelector.UnselectItem();
			}

			public void OnPointerDown(PointerEventData eventData) {
				PointerManager.Instance.SetPointer(PointerMode.DEFAULT_PRESSED);
			}
			#endregion

			#region Drag
			private bool _dragging = false;

			public void OnBeginDrag(PointerEventData eventData) {
				if (ReadOnly) {
					return;
				}

				if (ReferenceEquals(Item, null)) {
					return;
				}

				_dragging = true;

				Image.transform.SetParent(transform.root);
				Amount.transform.SetParent(Image.transform);

				OnBeginDragEvent?.Invoke();
			}

			public void OnDrag(PointerEventData eventData) {
				if (ReadOnly) {
					return;
				}

				Image.rectTransform.position = Input.mousePosition;
			}

			public void OnEndDrag(PointerEventData eventData) {
				if (ReadOnly) {
					return;
				}

				_dragging = false;

				var raycastResults = new List<RaycastResult>();
				EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = Input.mousePosition }, raycastResults);

				if (raycastResults.Count > 0) {
					bool handle = false;
					foreach (var result in raycastResults) {
						SlotHandler destinationSlot = result.gameObject.GetComponent<SlotHandler>();
						if (!ReferenceEquals(destinationSlot, null)) {

							if (ReferenceEquals(destinationSlot, this)) {
								ReplaceImage(eventData.pointerDrag.transform);
								handle = true;
								break;
							}

							if (ReferenceEquals(Container, destinationSlot.Container)) {
								Container.SwapItems(Item.Index, destinationSlot.ID);
							} else {
								InventoryUtils.SwapItemsBetweenContainers(Item, Container, destinationSlot.Container, Item.Index, destinationSlot.ID);
							}

							ReplaceImage(eventData.pointerDrag.transform);
							handle = true;
							break;
						}
					}

					if (!handle) {
						ReplaceImage(eventData.pointerDrag.transform);
					}

					OnEndDragEvent?.Invoke();
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

			public Item Item { get; private set; }
			public IContainer Container { get; private set; }

			public bool ReadOnly { get; private set; }
			public int ID { get; private set; }
			public bool HasItem { get { return !ReferenceEquals(Item, null); } }
		}
	}
}
