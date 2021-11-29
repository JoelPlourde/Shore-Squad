public static class Constant
{
	public static readonly int DEFAULT_CAMERA_DISTANCE = 10;
	public static readonly int DEFAULT_CAMERA_HEIGHT = 5;

	public static readonly float DEFAULT_ACTOR_SPEED = 3.5f;
	public static readonly int TICK_RATE = 500;                 // Tick rate in milliseconds.
	public static readonly float SCALED_TICK_RATE = TICK_RATE / 1000f;	// Tick rate in seconds.
	public static readonly int BASE_INFLUENCE_RADIUS = 5;
	public static readonly int INFLUENCE_RADIUS_INCREMENT = 5;
	public static readonly int PLACEMENT_MAXIMUM_RADIUS = 100;
	public static readonly float DEFAULT_REFRESH_RATE = 1 / 60f;
	public static readonly int AGENT_MAX_SLOPE = 45;
	public static readonly float AGENT_BASE_WALK_SPEED = 1.5f;
	public static readonly float AGENT_BASE_RUN_SPEED = 4.5f;

	public static readonly float ACTOR_BASE_SPEED = 3.5f;
	public static readonly float ACTOR_BASE_ROTATION_SPEED = 480f;
	public static readonly float ACTOR_BASE_FOOD = 20f;

	public static readonly string STARVING = "Starving";
	public static readonly string HUNGRY = "Hungry";
	public static readonly string DEATH = "Dead";

	public static readonly float DEFAULT_TEMPERATURE = 20f;

	public static readonly float TIME_BETWEEN_TEMPERATURE_CHANGES = 60f;

	public static readonly int MINIMUM_LEVEL = 1;
	public static readonly int MAXIMUM_LEVEL = 100;

	public static readonly int MAXIMUM_LUCK = 100;

	public static readonly float TOOLTIP_DELAY = 0.2f;
}
