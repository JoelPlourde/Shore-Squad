namespace StatusEffectSystem {
	public class ReduceHungerRateEffect : StatusEffect<ReduceHungerRateEffect>, IStatusEffect {

		public override void Apply(Status status) {
			base.Apply(status);
			status.Actor.Attributes.ReduceHungerRate(status.Magnitude);
		}

		public override void Unapply(Status status) {
			base.Unapply(status);
			status.Actor.Attributes.IncreaseHungerRate(status.Magnitude);
		}
	}
}
