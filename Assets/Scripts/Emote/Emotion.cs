using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmoteSystem {
	// Attach this to the Actor.
	public class Emotion : MonoBehaviour {

		private Actor _actor;

		public void Initialize(Actor actor) {
			_actor = actor;
		}

		public void PlayEmote(EmoteType emoteType) {
			EmoteData emoteData = EmoteManager.Instance.GetEmoteData(emoteType);

			_actor.Animator.SetInteger("Emote Type", (int) emoteData.emoteType);
			_actor.Animator.SetTrigger("Emote");

			if (emoteData.ParticleSystem != null) {
				GameObject particleSystemObj = Instantiate(emoteData.ParticleSystem, transform);
				particleSystemObj.transform.localPosition = emoteData.LocalPosition;
			}

			if (!ReferenceEquals(emoteData.Sound, null)) {
				_actor.AudioSource.PlayOneShot(emoteData.Sound);
			}
		}
	}
}
