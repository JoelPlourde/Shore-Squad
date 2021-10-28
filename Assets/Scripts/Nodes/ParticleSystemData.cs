using System;
using UnityEngine;

[Serializable]
public class ParticleSystemData {

	[SerializeField]
	[Tooltip("The particle system")]
	public GameObject ParticleSystem;

	[SerializeField]
	[Tooltip("The relative position where the particle system should be spawned at.")]
	public Vector3 RelativePosition;

	[SerializeField]
	[Tooltip("The sound to be played whenever this particle system is played.")]
	public AudioClip Sound;
}
