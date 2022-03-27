using System;
using UnityEngine;

namespace Tutorial {
	[Serializable]
	[CreateAssetMenu(fileName = "AdviceData", menuName = "ScriptableObjects/Tutorial/Advice Data")]
	public class AdviceData : ScriptableObject {

		[SerializeField]
		[Tooltip("A unique identifier for this Advice.")]
		public AdviceType AdviceType;

		[SerializeField]
		[Tooltip("The Advice itself.")]
		public string Text;

		[SerializeField]
		[Tooltip("The Image to display underneath the text. Not mandatory.")]
		public Sprite Image;
	}

	// TODO
	public enum AdviceType {
		NONE, FIRST, SECOND
	}
}
