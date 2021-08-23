using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace ItemSystem {
	namespace UI {
		public static class ItemSelector {

			private static readonly Outline[] _outlines = new Outline[5];

			private readonly static Dictionary<int, Vector2> _positions = new Dictionary<int, Vector2>() {
				{ 0, new Vector2(-1, 0) },
				{ 1, new Vector2(1, 0) },
				{ 2, new Vector2(0, 1) },
				{ 3, new Vector2(0, -1) },
				{ 4, new Vector2(0, 0) },
			};

			private static SlotHandler _selectedItem;

			static ItemSelector() {
				Material _outlineMaterial = new Material(Shader.Find("GUI/Text Shader")) {
					color = Color.white
				};

				_outlines[0] = GenerateOutline(_outlineMaterial);
				_outlines[1] = GenerateOutline(_outlineMaterial);
				_outlines[2] = GenerateOutline(_outlineMaterial);
				_outlines[3] = GenerateOutline(_outlineMaterial);
				_outlines[4] = GenerateOutline(null);
			}

			#region Selection
			/// <summary>
			/// Select the item.
			/// </summary>
			/// <param name="slotHandler">The slot where the item is located.</param>
			public static void SelectItem(SlotHandler slotHandler) {
				if (slotHandler.HasItem && !ReferenceEquals(_selectedItem, slotHandler)) {
					_selectedItem = slotHandler;

					ApplyOutline(slotHandler.Image);
				} else {
					UnselectItem();
				}
			}

			/// <summary>
			/// Unselect the item, if any.
			/// </summary>
			public static void UnselectItem() {
				_selectedItem = null;

				ResetOutline();
			}
			#endregion

			#region Outline
			private static Outline GenerateOutline(Material material) {
				Outline outline = new GameObject().AddComponent<Outline>();
				outline.Initialize(material);
				return outline;
			}

			private static void ApplyOutline(Image image) {
				for (int i = 0; i < 5; i++) {
					_outlines[i].SetImage(image, _positions[i]);
				}
			}

			private static void ResetOutline() {
				for (int i = 0; i < 5; i++) {
					_outlines[i].Reset();
				}
			}

			public class Outline : MonoBehaviour {

				private Image _image;

				/// <summary>
				/// Initialize the outline with a material.
				/// </summary>
				/// <param name="material">The material.</param>
				public void Initialize(Material material) {
					transform.SetParent(UserInterface.Instance.transform);
					_image = gameObject.AddComponent<Image>();
					_image.material = material;
					gameObject.SetActive(false);
				}

				/// <summary>
				/// Set the image at its position based on an original image.
				/// </summary>
				/// <param name="original">The original image</param>
				/// <param name="position">The position of this outline.</param>
				public void SetImage(Image original, Vector2 position) {
					transform.SetParent(original.transform);
					_image.sprite = original.sprite;
					_image.rectTransform.localPosition = position;
					_image.rectTransform.sizeDelta = original.rectTransform.sizeDelta;
					gameObject.SetActive(true);
				}

				/// <summary>
				/// Reset the outline.
				/// </summary>
				public void Reset() {
					gameObject.SetActive(false);
				}
			}
			#endregion
		}
	}
}
