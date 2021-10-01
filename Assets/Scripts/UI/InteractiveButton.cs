using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
	[RequireComponent(typeof(Button))]
	public class InteractiveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

		private static readonly Vector3 SCALE_UP = new Vector3(1.2f, 1.2f, 1.2f);

		private static readonly Vector3 SCALE_DOWN = new Vector3(1.15f, 1.15f, 1.15f);

		#region OnPointerEvent
		public virtual void OnPointerDown(PointerEventData eventData) {
			LeanTween.scale(gameObject, SCALE_DOWN, 0.05f);
		}

		public virtual void OnPointerUp(PointerEventData eventData) {
			LeanTween.scale(gameObject, SCALE_UP, 0.05f);
		}

		public virtual void OnPointerEnter(PointerEventData pointerEventData) {
			Tooltip.Instance.ShowTooltip(GetTooltip(), Constant.TOOLTIP_DELAY);
			LeanTween.scale(gameObject, SCALE_UP, 0.25f);
		}

		public virtual void OnPointerExit(PointerEventData pointerEventData) {
			Tooltip.Instance.HideTooltip();
			if (!ReferenceEquals(gameObject, null)) {
				LeanTween.cancel(gameObject);
				LeanTween.scale(gameObject, Vector3.one, 0.1f);
			}
		}
		#endregion

		public virtual string GetTooltip() {
			return "";
		}
	}
}
