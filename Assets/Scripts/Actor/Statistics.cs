using System;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem.EquipmentSystem;

public class Statistics {

	// Initially the statistics are all at zero. On load game, the equipment, status, etc. will alter the statistics.
	public readonly Dictionary<StatisticType, int> CurrentStatistics = new Dictionary<StatisticType, int>() {
		{ StatisticType.ARMOR, 0},
		{ StatisticType.CONSTITUTION, 0},
		{ StatisticType.STRENGTH, 0},
		{ StatisticType.INTELLIGENCE, 0},
		{ StatisticType.DEXTERITY, 0},
		{ StatisticType.FAITH, 0},
		{ StatisticType.COLD_RESISTANCE, 0},
		{ StatisticType.HEAT_RESISTANCE, 0},
	};

	public event Action<StatisticType, int> OnUpdateStatisticEvent;					// Event triggered whenever a statistic is updated.

	/// <summary>
	/// Adjust multiple statistics depending whether or not its positive or negative.
	/// </summary>
	/// <param name="statistics"></param>
	/// <param name="increment"></param>
	public void UpdateStatistics(List<Statistic> statistics, bool increment) {
		foreach (Statistic statistic in statistics) {
			UpdateStatistic(statistic.StatisticType, statistic.Value, increment);
		}
	}

	/// <summary>
	/// Adjust a statistic by a value either positive or negative based on the increment parameter.
	/// </summary>
	/// <param name="statisticType">The type of statistic to be adjusted.</param>
	/// <param name="value">The value to update the value by.</param>
	/// <param name="increment">If true, increase the value. Else decrease.</param>
	public void UpdateStatistic(StatisticType statisticType, int value, bool increment) {
		if (CurrentStatistics.ContainsKey(statisticType)) {
			CurrentStatistics[statisticType] += (increment) ? value : -value;
			OnUpdateStatisticEvent?.Invoke(statisticType, CurrentStatistics[statisticType]);

			Debug.Log("Newest value of : " + statisticType + " is: " + CurrentStatistics[statisticType]);
		} else {
			throw new UnityException("The StatisticType key does not exist, please define it in the Statistics class dictionary.");
		}
	}
}
