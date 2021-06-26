using UnityEngine;
using FactionSystem;
using System;

namespace SaveSystem {
	[Serializable]
	public class ActorDto {

		[SerializeField]
		public Vector3 Position;

		[SerializeField]
		public string Guid;

		[SerializeField]
		public bool Playable;

		[SerializeField]
		public FactionType FactionType;

		[SerializeField]
		public AttributesDto AttributesDto;

		[SerializeField]
		public StatusDto StatusDto;

		[SerializeField]
		public FeaturesDto FeaturesDto;

		public ActorDto(Actor actor) {
			Position = actor.transform.position;
			Guid = actor.Guid.ToString();
			Playable = actor.Playable;
			FactionType = actor.FactionType;
			AttributesDto = new AttributesDto(actor.Attributes);
			StatusDto = new StatusDto(actor.Status);
			FeaturesDto = new FeaturesDto(actor.Body, actor.Face);
			Debug.Log("Here !");
		}
	}
}
