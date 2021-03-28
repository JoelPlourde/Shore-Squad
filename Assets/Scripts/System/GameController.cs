using UnityEngine;
using StatusEffectSystem;

[RequireComponent(typeof(AdjustTemperatureStatusEffect))]
[RequireComponent(typeof(HungerStatusEffect))]
[RequireComponent(typeof(HealOverTimeEffect))]
[RequireComponent(typeof(HealthRegenerationStatusEffect))]
[RequireComponent(typeof(SlowOverTimeEffect))]
[RequireComponent(typeof(DamageOverTimeEffect))]
[RequireComponent(typeof(TimerManager))]
[RequireComponent(typeof(Foreman))]
[RequireComponent(typeof(Time))]
public class GameController : SingletonBehaviour<GameController> {
}
