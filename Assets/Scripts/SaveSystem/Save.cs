using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tutorial;
using UnityEditor.EditorTools;
using CameraSystem;

namespace SaveSystem {
	[Serializable]
	public class Save {

		[SerializeField]
		public CameraDto CameraDto = new CameraDto();

		[SerializeField]
		public CameraTargetDto CameraTargetDto = new CameraTargetDto();

		[SerializeField]
		public SquadDto SquadDto = new SquadDto();

		[SerializeField]
		public List<QuestDto> QuestDtos = new List<QuestDto>();

		[SerializeField]
		public List<ActorDto> ActorDtos = new List<ActorDto>();

		[SerializeField]
		public EncyclopediaDto EncyclopediaDto = new EncyclopediaDto();

		[SerializeField]
		// The list of items placed in the world.
		public SerializableDictionary<string, WorldItemDto> WorldItemDtos = new SerializableDictionary<string, WorldItemDto>();
	}
}
