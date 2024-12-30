using System;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(GridLayoutGroup))]
    public class MessageHandler : MonoBehaviour {

        public static MessageHandler Instance;

        [Header("Configuration")]
        [Tooltip("The time in seconds the message will be displayed.")]
        public float AliveTime = 5f;

        [Header("Font")]
        public Font Font;
        public int FontSize = 50;
        public Color FontColor = Color.black;

        private Text _messageTemplate;

        private void Awake() {
            Instance = this;

            // Configure the GridLayoutGroup.
            GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();
            gridLayoutGroup.cellSize = new Vector2(1000, 50);
            gridLayoutGroup.startCorner = GridLayoutGroup.Corner.LowerLeft;
            gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
            gridLayoutGroup.childAlignment = TextAnchor.LowerLeft;
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = 1;
            gridLayoutGroup.padding.left = 10;

            // Define a template for the message to be re-used.
            GameObject template = new GameObject("Message");
            template.transform.SetParent(transform);
            Text text = template.AddComponent<Text>();
            text.fontSize = FontSize;
            text.color = FontColor;
            text.font = Font;

            _messageTemplate = text;
            _messageTemplate.gameObject.SetActive(false);
        }

        /**
        * Show a message on the screen.
        */
        public void ShowMessage(string message) {
            Text textComponent = Instantiate(_messageTemplate, transform);
            textComponent.text = message;
            textComponent.gameObject.SetActive(true);
            LeanTween.alphaText(textComponent.rectTransform, 0f, 1f).setOnComplete(() => {
                Destroy(textComponent.gameObject);
            }).setDelay(5f);
        }
    }
}