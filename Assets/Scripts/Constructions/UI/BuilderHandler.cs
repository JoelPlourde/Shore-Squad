using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace ConstructionSystem {
	namespace UI {
		[RequireComponent(typeof(Canvas))]
		[RequireComponent(typeof(GridLayoutGroup))]
		[RequireComponent(typeof(GraphicRaycaster))]
		public class BuilderHandler : MonoBehaviour, IMenu {

			public static BuilderHandler Instance;

			private RectTransform _rectTransform;
			private GridLayoutGroup _gridLayoutGroup;

			private Transform _previousParent;
			private GameObject _currentExpandedContent;

			private void Awake() {
				Instance = this;
				Canvas = GetComponent<Canvas>();

				// Fetch the required component.
				_rectTransform = GetComponent<RectTransform>();
				_gridLayoutGroup = GetComponent<GridLayoutGroup>();

				// Initialize the grid.
				InitializeGridLayoutGroup(_gridLayoutGroup, 5, 75, 10);

				// Populate the grid.
				PopulateConstructionData();

				// Re-adjust the parent based on the content.
				ResizeRectTransform(_rectTransform, _gridLayoutGroup);
			}

			/// <summary>
			/// Populate the Construction data
			/// </summary>
			private void PopulateConstructionData() {
				foreach (Category category in ConstructionManager.Architect.Categories) {
					GameObject categoryObject = CreateCategory(category);
					Tuple<RectTransform, GridLayoutGroup> content = CreateContent(categoryObject.transform);

					foreach (ConstructionData constructionData in category.ConstructionDatas) {
						CreateConstructionData(content.Item1.transform, constructionData);
					}

					ResizeRectTransform(content.Item1, content.Item2);

					content.Item1.gameObject.SetActive(false);
				}
			}

			#region Create operations
			/// <summary>
			/// Create a category and attach it to its parent. The Category is a button.
			/// </summary>
			/// <param name="category">The Category</param>
			/// <returns>The Category object that has been created.</returns>
			private GameObject CreateCategory(Category category) {
				GameObject categoryObject = new GameObject {
					name = category.Name
				};

				Image image = categoryObject.AddComponent<Image>();
				image.sprite = category.DefaultSprite;

				Button button = categoryObject.AddComponent<Button>();
				button.onClick.AddListener(() => ExpandContent(categoryObject));
				button.transition = Selectable.Transition.SpriteSwap;
				button.targetGraphic = image;
				button.spriteState = new SpriteState {
					highlightedSprite = category.HighlightedSprite,
					pressedSprite = category.PressedSprite,
					selectedSprite = category.HighlightedSprite,
					disabledSprite = category.DefaultSprite
				};

				string tooltip = I18N.GetValue("constructions.categories." + category.Name.Replace(" ", "_").ToLower() + ".description");

				categoryObject.AddComponent<InteractiveButton>().Initialize(tooltip);

				categoryObject.transform.SetParent(transform);
				categoryObject.transform.SetAsLastSibling();

				return categoryObject;
			}

			/// <summary>
			/// Create a content object to attach every construction data underneath. The content is a simple rectangle.
			/// </summary>
			/// <param name="category">The category.</param>
			/// <returns>Returns a tuple containing both the RectTransform and the GridLayoutGroup of the content.</returns>
			private Tuple<RectTransform, GridLayoutGroup> CreateContent(Transform category) {
				GameObject contentObject = new GameObject {
					name = "Content"
				};

				contentObject.transform.SetParent(category);

				// Add a grid layout group.
				GridLayoutGroup gridLayoutGroup = contentObject.AddComponent<GridLayoutGroup>();
				RectTransform contentRectTransform = contentObject.GetComponent<RectTransform>();

				contentObject.AddComponent<LayoutElement>().ignoreLayout = true;

				InitializeGridLayoutGroup(gridLayoutGroup, 5, 50, 5);

				gridLayoutGroup.startCorner = GridLayoutGroup.Corner.LowerLeft;
				gridLayoutGroup.childAlignment = TextAnchor.LowerLeft;
				gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
				gridLayoutGroup.constraintCount = 8;

				// Set its anchor
				contentRectTransform.anchorMax = new Vector2(0, 1);
				contentRectTransform.anchorMin = new Vector2(0, 1);
				contentRectTransform.pivot = new Vector2(0, 0);

				return new Tuple<RectTransform, GridLayoutGroup>(contentRectTransform, gridLayoutGroup);
			}

			/// <summary>
			/// Create a Construction data which is a simple button and an image
			/// </summary>
			/// <param name="content">The content</param>
			/// <param name="constructionData">The ConstructionData</param>
			private void CreateConstructionData(Transform content, ConstructionData constructionData) {
				GameObject constructionObject = new GameObject {
					name = constructionData.name
				};

				// Add a button.
				constructionObject.AddComponent<Button>().onClick.AddListener(() => OnConstructionClick(constructionData));
				constructionObject.AddComponent<Image>().sprite = constructionData.Sprite;

				string name = constructionData.name.Replace(" ", "_");
				string tooltip = I18N.GetValue("constructions." + name + ".description");

				constructionObject.AddComponent<InteractiveButton>().Initialize(tooltip);

				constructionObject.transform.SetParent(content);
				constructionObject.transform.SetAsLastSibling();
			}
			#endregion

			#region Click events
			/// <summary>
			/// Expand the Content to display the Constructions
			/// </summary>
			/// <param name="object">The object that is clicked.</param>
			private void ExpandContent(GameObject @object) {

				if (!ReferenceEquals(_currentExpandedContent, null)) {
					_currentExpandedContent.transform.SetParent(_previousParent);
					_currentExpandedContent.SetActive(false);

					// Verify if its the same content that is re-clicked, if so, close it.
					if (ReferenceEquals(_currentExpandedContent.transform.parent, @object.transform)) {
						_currentExpandedContent = null;
						return;
					}
				}

				_previousParent = @object.transform;
				_currentExpandedContent = @object.transform.GetChild(0).gameObject;

				_currentExpandedContent.transform.SetParent(transform);


				RectTransform rectTransform = _currentExpandedContent.GetComponent<RectTransform>();
				rectTransform.anchoredPosition = Vector2.zero;
				rectTransform.localScale = Vector3.one;
				// _currentExpandedContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
				
				_currentExpandedContent.SetActive(true);
			}

			private void OnConstructionClick(ConstructionData constructionData) {
				Foreman.Instance.StartPlacement(constructionData);
			}
			#endregion

			#region UI scaling
			/// <summary>
			/// Initialize a GridLayoutGroup
			/// </summary>
			/// <param name="gridLayoutGroup">The GridLayoutGroup</param>
			/// <param name="padding">The amount of padding to apply</param>
			/// <param name="cellSize">The cellSize</param>
			/// <param name="spacing">The spacing between cellsize</param>
			private void InitializeGridLayoutGroup(GridLayoutGroup gridLayoutGroup, int padding, int cellSize, int spacing) {
				gridLayoutGroup.padding.bottom = padding;
				gridLayoutGroup.padding.top = padding;
				gridLayoutGroup.padding.left = padding;
				gridLayoutGroup.padding.right = padding;

				gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
				gridLayoutGroup.spacing = new Vector2(spacing, spacing);
			}

			/// <summary>
			/// Resize the RectTransform based on the underlying content of its children.
			/// </summary>
			/// <param name="rectTransform">The RectTransform</param>
			/// <param name="gridLayoutGroup">The GridLayoutGroup containing its children.</param>
			private void ResizeRectTransform(RectTransform rectTransform, GridLayoutGroup gridLayoutGroup) {
				// Padding in X
				float sizeX = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;

				// Size based on the number of children
				sizeX += rectTransform.childCount * gridLayoutGroup.cellSize.x;

				// Add the spacing between children.
				sizeX += (rectTransform.childCount - 1) * gridLayoutGroup.spacing.x;

				// Padding in Y
				float sizeY = gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;

				// Size based on a cell in Y
				sizeY += gridLayoutGroup.cellSize.y;

				// Set the Size.
				rectTransform.sizeDelta = new Vector2(sizeX, sizeY);
			}
			#endregion

			#region Menu
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
			#endregion

			public Canvas Canvas { get; set; }
		}
	}
}
