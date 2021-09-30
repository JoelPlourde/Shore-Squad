using UnityEngine;

public class SoundSystem : MonoBehaviour
{
	public static SoundSystem Instance;

	private AudioSource _audioSource;

	private void Awake() {
		Instance = this;

		_audioSource = GetComponent<AudioSource>();
	}

	/// <summary>
	/// Play a sound by its audio clip at at certain volume.
	/// </summary>
	/// <param name="audioClip">The Audio Clip to be played.</param>
	/// <param name="volume">The volume in percentage (%).</param>
	public void PlaySound(AudioClip audioClip, float volume) {
		_audioSource.volume = volume;
		_audioSource.PlayOneShot(audioClip);
	}
}
