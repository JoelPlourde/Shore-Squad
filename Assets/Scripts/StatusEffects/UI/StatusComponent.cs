using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UI;

namespace StatusEffectSystem {
	namespace UI {
		public class StatusComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

			private Text _stack;
			private Text _duration;
			private Image _image;
			private Image _background;

			private Status _status;

			private void Awake() {
				_stack = transform.Find("stack").GetComponent<Text>();
				if (_stack == null) {
					throw new UnityException("Please verify the structure of StatusEffectComponent, cannot find any object named 'stack'");
				}

				_duration = transform.Find("duration").GetComponent<Text>();
				if (_duration == null) {
					throw new UnityException("Please verify the structure of StatusEffectComponent, cannot find any object named 'duration'");
				}

				_background = transform.GetComponent<Image>();
				if (_background == null) {
					throw new UnityException("Please verify the structure of StatusEffectComponent, cannot find any Image component on this object.");
				}

				_image = transform.Find("image").GetComponent<Image>();
				if (_image == null) {
					throw new UnityException("Please verify the structure of StatusEffectComponent, cannot find any component 'Image'.");
				}

				_image.rectTransform.sizeDelta = transform.parent.GetComponent<GridLayoutGroup>().cellSize;
			}

			public void Initialize(Status status, StatusHandler statusHandler) {
				_status = status;
				switch (status.StatusEffectData.StatusEffectCategory) {
					case StatusEffectCategory.NEUTRAL:
						_background.color = Color.black;
						break;
					case StatusEffectCategory.HARMFUL:
						_background.color = Color.red;
						break;
					case StatusEffectCategory.BENEFICIAL:
						_background.color = Color.green;
						break;
					default:
						_background.color = Color.white;
						break;
				}
				UpdateStack(status.Stack);
				if (status.StatusEffectData.Temporary == true) {
					UpdateDuration(status.Duration);
				}
				_image.sprite = status.StatusEffectData.Sprite;
				_image.rectTransform.sizeDelta = statusHandler.GridLayoutGroup.cellSize;
			}

			public void UpdateStack(int value) {
				if (value > 0) {
					_stack.text = value.ToString();
				} else {
					_stack.text = "";
				}
			}

			public void UpdateDuration(int value) {
				if (_status.StatusEffectData.Temporary == false) {
					return;
				}

				if (value >= 60) {
					_duration.text = Mathf.FloorToInt(value / 60).ToString() + "m";
				} else {
					_duration.text = value.ToString();
				}
			}

			public void Destroy() {
				Tooltip.Instance.HideTooltip();
				Destroy(gameObject);
			}

			public void OnPointerEnter(PointerEventData eventData) {
				Tooltip.Instance.ShowTooltip(_status.StatusEffectData.Tooltip);
			}

			public void OnPointerExit(PointerEventData eventData) {
				Tooltip.Instance.HideTooltip();
			}
		}
	}
}
