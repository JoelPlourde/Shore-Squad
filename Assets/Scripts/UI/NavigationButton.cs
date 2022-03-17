using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	// A Navigation button goal is to bring the user's attention to the button itself.
	[RequireComponent(typeof(Button))]
	public class NavigationButton : MonoBehaviour {

		[Range(0f, 25f)]
		[Tooltip("How much the NavigationButton should move from side to side.")]
		public float Magnitude = 0f;

		[Range(0f, 5f)]
		[Tooltip("Time for NavigationButton to move (the higher the value, the slower the button it is)")]
		public float Time = 1f;

		[Tooltip("The direction where the NavigationButton moves between.")]
		public Direction direction = Direction.X;

		private Vector3 _initialPosition;
		private LTDescr _description;
		private Canvas _parentCanvas;
		private RectTransform _rectTransform;

		private void Awake() {
			_rectTransform = GetComponent<RectTransform>();
			_parentCanvas = GetFirstParentCanvas(transform);
			if (ReferenceEquals(_parentCanvas, null)) {
				throw new UnityException("The parent Canvas cannot be found on this NavigationButton. Please verify!");
			}

			_initialPosition = _rectTransform.localPosition;
		}

		void OnEnable() {
			if (_parentCanvas.isActiveAndEnabled) {
				if (direction == Direction.X) {
					_description = LeanTween.moveX(_rectTransform, Magnitude, Time).setLoopPingPong();
				} else {
					_description = LeanTween.moveY(_rectTransform, Magnitude, Time).setLoopPingPong();
				}
			}
		}

		void OnDisable() {
			_rectTransform.localPosition = _initialPosition;

			if (!ReferenceEquals(_description, null)) {
				_description.pause();
			}
		}

		/// <summary>
		/// Get the first parent canvas
		/// </summary>
		/// <param name="transform">The source transform</param>
		/// <returns>The first Parent canvas.</returns>
		private Canvas GetFirstParentCanvas(Transform source) {
			Canvas canvas = source.GetComponentInParent<Canvas>();
			if (ReferenceEquals(canvas, null)) {
				return GetFirstParentCanvas(source.parent);
			} else {
				return canvas;
			}
		}

		public enum Direction {
			X, Y
		}
	}
}
