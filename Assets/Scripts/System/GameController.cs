using UnityEngine;
using StatusEffectSystem;

[RequireComponent(typeof(DamageOverTimeEffect))]
[RequireComponent(typeof(SlowEffect))]
[RequireComponent(typeof(TimerManager))]
[RequireComponent(typeof(Foreman))]
[RequireComponent(typeof(Time))]
public class GameController : SingletonBehaviour<GameController> {
}
