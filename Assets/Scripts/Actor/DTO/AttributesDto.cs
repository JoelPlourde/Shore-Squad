using System;
using UnityEngine;

namespace SaveSystem {
	[Serializable]
	public class AttributesDto {

		[SerializeField]
		public float MaxHealth;

		[SerializeField]
		public float Health;

		[SerializeField]
		public float HealthRegeneration;

		[SerializeField]
		public float Speed;

		[SerializeField]
		public float Damage;

		[SerializeField]
		public float HungerRate;

		[SerializeField]
		public float Food;

		[SerializeField]
		public float Temperature;

		public AttributesDto() {
			MaxHealth = Constant.DEFAULT_HEALTH;
			Health = Constant.DEFAULT_HEALTH;
			HealthRegeneration = Constant.DEFAULT_HEALTH_REGENERATION;
			Speed = Constant.DEFAULT_SPEED;
			Damage = Constant.DEFAULT_DAMAGE;
			HungerRate = Constant.DEFAULT_HUNGER_RATE;
			Food = Constant.DEFAULT_FOOD;
			Temperature = Constant.DEFAULT_TEMPERATURE;
		}

		public AttributesDto(Attributes attributes) {
			if (ReferenceEquals(attributes, null)) {
				attributes = new Attributes();
			}

			MaxHealth = attributes.MaxHealth;
			Health = attributes.Health;
			HealthRegeneration = attributes.HealthRegeneration;
			Speed = attributes.Speed;
			Damage = attributes.Damage;
			HungerRate = attributes.HungerRate;
			Food = attributes.Food;
			Temperature = attributes.Temperature;
		}
	}
}
