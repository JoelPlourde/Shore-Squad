using UnityEngine;

namespace AudioSystem {
	[RequireComponent(typeof(AudioSource))]
	public class AudioPlayer : MonoBehaviour {

		private AudioSource _audioSource;

		private void Awake() {
			_audioSource = GetComponent<AudioSource>();
		}

		public void PlayOneShot(AudioClip audioClip) {
			RandomizePitch();
			_audioSource.PlayOneShot(audioClip);
		}

		public void PlayDelayed(AudioClip audioClip, float delay) {
			RandomizePitch();
			_audioSource.clip = audioClip;
			_audioSource.PlayDelayed(delay);
		}

		private void RandomizePitch() {
			_audioSource.pitch = Random.Range(0.85f, 1.15f);
		}
	}
}
