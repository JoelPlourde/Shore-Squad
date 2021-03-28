using System;

[Serializable]
public class Attributes {
	public float MaxHealth = 100f;
	public float Health = 100f;
	public float HealthRegeneration = 1f;		// Value per 5 seconds
	public float Speed = 1f;
	public float Damage = 2f;
	public float HungerRate = 0.05f;            // Value per 5 seconds
	public float Food = 20f;
	public float Temperature = 20f;
}
