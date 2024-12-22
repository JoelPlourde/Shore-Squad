using CraftingSystem;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI {
    public class RecipeSlot : MonoBehaviour {

        // Components
        private Image _image;
        private Button _button;
        private RecipeData _recipeData;

        private Action<RecipeData> _callback;
        private Action _onClick;
        
        private void Awake() {
            _button = this.transform.Find("Background").GetComponent<Button>();
            _button.onClick.AddListener(() => {
                OnClick();
            });
            _image = _button.transform.Find("Image").GetComponent<Image>();
        }

        public void Initialize(RecipeData recipeData, Action onClick, Action<RecipeData> callback) {
            _recipeData = recipeData;
            _image.sprite = recipeData.Outputs[0].ItemData.Sprite;
            _callback = callback;
            _onClick = onClick;
        }

        /**
        * When the button is clicked, call the callback function with the RecipeData
        */
        public void OnClick() {
            this._onClick();
            this._callback(_recipeData);
        }
    }
}