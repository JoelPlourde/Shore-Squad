using UnityEngine;
using ItemSystem;
using CraftingSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace UI {
    public class RecipeHandler : MonoBehaviour {

            // Static instance of itself
            public static RecipeHandler Instance;

            public GameObject RecipeSlotTemplate;

            public float AnimationSpeed = 0.1f;
            public float Radius = 200f;
            public int MinimumSteps = 0;

            // For testing purposes.
            public List<RecipeData> TestingRecipeDatas;

            private Transform _canvas;

            private Transform _recipesParent;
            private Image _primaryImage;

            private void Awake () {
                Instance = this;

                _canvas = transform.Find("Canvas");

                _recipesParent = _canvas.transform.Find("Recipes");
                _primaryImage = _canvas.transform.Find("Primary").GetComponent<Image>();

                // Find the child object with the name "Buttons", underneath there find the "Exit"
                Button _exitButton = _canvas.transform.Find("Buttons").Find("Exit").GetComponent<Button>();
                _exitButton.transform.localPosition = new Vector3(Radius, Radius, 0);
                _exitButton.onClick.AddListener(() => {
                    Close();
                });
            }

            /**
            * Show the Recipes
            */
            public void Show(List<RecipeData> recipeDatas, Action<RecipeData> callback, Sprite primaryImage = null) {
                this._canvas.gameObject.SetActive(true);
                if (!ReferenceEquals(primaryImage, null)) {
                    _primaryImage.sprite = primaryImage;
                }

                this.InitializeRecipes(recipeDatas, callback);
            }

            /**
            * Close the Menu
            */
            public void Close() {
                // Delete all children underneath the _recipesParent
                foreach (Transform child in _recipesParent) {
                    Destroy(child.gameObject);
                }
                this._canvas.gameObject.SetActive(false);
            }

            private void InitializeRecipes(List<RecipeData> recipeDatas, Action<RecipeData> callback) {
                int numberSteps = recipeDatas.Count;
                if (numberSteps < MinimumSteps) {
                    numberSteps = MinimumSteps;
                }

                // From the recipe Count, calculate the angle between each recipe
                float angleStep = 360f / numberSteps;

                // From the angleStep, calculate its position in X and Y around the circle
                for (int i = 0; i < recipeDatas.Count; i++) {
                    float angle = i * angleStep;
                    float x = Mathf.Sin(angle * Mathf.Deg2Rad) * Radius;
                    float y = Mathf.Cos(angle * Mathf.Deg2Rad) * Radius;

                    // Create a new instance of RecipeSlotTemplate
                    GameObject recipeSlotObj = Instantiate(RecipeSlotTemplate, _recipesParent);
                    RecipeSlot recipeSlot = recipeSlotObj.GetComponent<RecipeSlot>();
                    recipeSlot.Initialize(recipeDatas[i], Close, callback);
                    recipeSlotObj.transform.localPosition = Vector3.zero;

                    Vector3 finalPosition = new Vector3(x, y, 0);

                    // Animate the RecipeSlot
                    LeanTween.moveLocal(recipeSlotObj, finalPosition, AnimationSpeed);
                }
            }
        }
}