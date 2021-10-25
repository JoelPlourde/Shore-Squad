﻿using System;
using UnityEngine;
using DropSystem;
using ItemSystem.EquipmentSystem;
using SkillSystem;

namespace NodeSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "NodeData", menuName = "ScriptableObjects/Node Data")]
	public class NodeData : ScriptableObject {

		[Header("Harvest Parameters")]
		[Tooltip("The healthbar of this node.")]
		public int Health = 100;

		[Tooltip("The durability of the node indicates how hard it is to get its health to 0.")]
		public int Durability = 5;

		[Tooltip("Number of times this node can be harvested.")]
		public int Capacity = 5;

		[Header("Drops")]
		[Tooltip("Drop table that will rolled on whenever a node is harvested.")]
		public DropTable DropTable;

		[Header("Skill parameters")]
		[Tooltip("The Skill that this node grants experience in.")]
		public SkillType SkillType;

		[Tooltip("The Tool required to harvest this node.")]
		public WeaponType WeaponType;

		[Tooltip("The level required in the respective skill in order to harvest this node.")]
		public int Requirement = 1;

		[Tooltip("The experience granted by each harvest strike.")]
		public int Experience = 1;

		[Header("Improvement Parameters")]
		[Tooltip("The Statistic to look at to determine how is the damage calculated")]
		public StatisticType DamageStatistic;

		[Tooltip("The Statistic to look at to determine how is the speed calculated")]
		public StatisticType SpeedStatistic;

		[Header("Particle System")]
		[Tooltip("The particle system that will played when the node is hit.")]
		public GameObject HitParticleSystem;

		[Tooltip("The relative Position where the particle system will be spawn from relative to the character.")]
		public Vector3 HitRelativePosition;

		public AudioClip HitSound;
	}
}
