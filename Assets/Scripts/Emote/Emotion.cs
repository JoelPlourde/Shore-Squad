using UnityEngine;
using System.Collections.Generic;

namespace EmoteSystem {
	public class Emotion : MonoBehaviour {

		private Actor _actor;

		// Initialize a dictionary of EmoteType to GameObject
		private Dictionary<EmoteType, GameObject> _emoteParticleSystems = new Dictionary<EmoteType, GameObject>();

		public void Initialize(Actor actor) {
			_actor = actor;
		}

		public void PlayEmote(EmoteType emoteType, bool isLooping = false) {
			EmoteData emoteData = EmoteManager.Instance.GetEmoteData(emoteType);

			_actor.Animator.SetInteger("Emote Type", (int) emoteData.emoteType);
			_actor.Animator.SetTrigger("Emote");

			if (emoteData.ParticleSystem != null) {
				GameObject particleSystemObj = Instantiate(emoteData.ParticleSystem, transform);
				particleSystemObj.transform.localPosition = emoteData.LocalPosition;
				if (isLooping) {
					_emoteParticleSystems.Add(emoteType, particleSystemObj);
				}
			}

			if (!ReferenceEquals(emoteData.Sound, null)) {
				if (isLooping) {
					_actor.AudioPlayer.PlayLooping(emoteData.Sound);
				} else {
					_actor.AudioPlayer.PlayOneShot(emoteData.Sound);
				}
			}
		}

		public void StopEmote(EmoteType emoteType) {
			if (_emoteParticleSystems.TryGetValue(emoteType, out GameObject particleSystem)) {
				AutoDestruct AutoDestruct = particleSystem.GetComponent<AutoDestruct>();
				if (ReferenceEquals(AutoDestruct, null)) {
					Destroy(particleSystem);
				} else {
					AutoDestruct.Destroy();
				}
			}

			_actor.AudioPlayer.Stop();
		}
	}
}
