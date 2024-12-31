using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Image))]
    public class CameraTint: MonoBehaviour {

        public static CameraTint Instance;

        [Tooltip("The scaling factor of the camera tint")]
        [Range(0.0f, 0.25f)]
        public float ScalingFactor = 0.15f;

        private Image _image;
        
        private void Awake() {
            Instance = this;

            _image = GetComponent<Image>();
        }
    
        public void UpdateTint(Color color, float alpha) {
            _image.color = new Color(color.r, color.g, color.b, alpha * ScalingFactor);
        }

        public void ResetTint() {
            LeanTween.alpha(_image.rectTransform, 0.0f, 5.0f);
        }
    }
}