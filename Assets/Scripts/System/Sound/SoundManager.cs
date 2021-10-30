using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance;

	private Dictionary<string, AudioClip> _audioClips;

	private void Awake() {
		Instance = this;

		AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Sounds");
		_audioClips = audioClips.ToDictionary(x => x.name);
	}

	public AudioClip GetAudioClip(string name) {
		if (_audioClips.TryGetValue(name, out AudioClip audioClip)) {
			return audioClip;
		} else {
			throw new UnityException("The AudioClip couldn't be found by its name. Please define this AudioClip: " + name);
		}
	}
}
