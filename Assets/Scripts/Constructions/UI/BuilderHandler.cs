using UI;
using UnityEngine;

namespace ConstructionSystem {
	namespace UI {
		[RequireComponent(typeof(Canvas))]
		public class BuilderHandler : MonoBehaviour, IMenu {

			public static BuilderHandler Instance;

			private RectTransform _rectTransform;

			private void Awake() {
				Instance = this;
				Canvas = GetComponent<Canvas>();
				_rectTransform = GetComponent<RectTransform>();
			}

			public void Open(Actor actor) {

				// Hide the Actor Menu Bar
				ActorMenuBar.Instance.HideActorMenuBar();

				Canvas.enabled = true;

				LeanTween.moveY(_rectTransform, 0, 0.25f);
			}

			public void Close(Actor actor) {
				// Show the Actor menu Bar
				ActorMenuBar.Instance.ShowActorMenuBar();

				LeanTween.moveY(_rectTransform, -_rectTransform.sizeDelta.y, 0.25f).setOnComplete(delegate () {
					Canvas.enabled = false;
				});
			}

			public Canvas Canvas { get; set; }
		}
	}
}
