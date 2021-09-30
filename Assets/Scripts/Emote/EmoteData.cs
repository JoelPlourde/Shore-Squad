using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmoteSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "EmoteData", menuName = "ScriptableObjects/Emote Data")]
	public class EmoteData : ScriptableObject {

		[SerializeField]
		[Tooltip("The type of Emote")]
		public EmoteType emoteType;

		[Header("Sound")]
		[SerializeField]
		[Tooltip("The sound that will be played during the Emote (if any)")]
		public AudioClip Sound;

		[Tooltip("The sound will be played after the delay (in seconds)")]
		public float Delay;

		[Header("Particle System")]
		[SerializeField]
		[Tooltip("Particle System to be displayed on top of the character's head (if any")]
		public GameObject ParticleSystem;

		[SerializeField]
		[Tooltip("Position to spawn the Particle System")]
		public Vector3 LocalPosition;
	}
}
