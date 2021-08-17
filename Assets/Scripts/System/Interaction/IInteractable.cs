public interface IInteractable
{
	void OnInteractEnter(Actor actor);

	void OnInteractExit(Actor actor);

	// To determine how close the Actor has to be to interact with this.
	float GetInteractionRadius();
}
