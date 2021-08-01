using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem {
	[RequireComponent(typeof(Text))]
	public class TaskComponent : MonoBehaviour {

		private float _size;
		private Text _content;

		private void Awake() {
			_content = GetComponent<Text>();
		}

		public float Initialize(RectTransform parent, Quest quest) {
			UpdateDisplay(quest);

			_content.rectTransform.sizeDelta = new Vector2(parent.sizeDelta.x, _content.rectTransform.sizeDelta.y);
			_content.rectTransform.anchoredPosition = Vector2.zero;

			gameObject.SetActive(true);

			_size = LayoutUtility.GetPreferredHeight(_content.rectTransform);
			return _size;
		}

		public float Disable() {
			if (gameObject.activeInHierarchy) {
				gameObject.SetActive(false);
				return _size;
			} else {
				return 0;
			}
		}

		public void MarkCompleted() {
			_content.color = Color.grey;
		}

		public void UpdateDisplay(Quest quest) {
			_content.color = Color.black;
			_content.text = quest.CurrentProgress + "/" + quest.Task.Amount + " " + quest.Task.Title;
		}

		public void Delete() {
			Destroy(gameObject);
		}
	}
}