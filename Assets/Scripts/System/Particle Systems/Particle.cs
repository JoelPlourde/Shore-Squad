using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Particle : MonoBehaviour {

	public float Lifespan;
	public int Capacity;

	public void Enable(Vector3 position) {
		transform.position = position;
		gameObject.SetActive(true);
		StartCoroutine(WaitForDelay());
	}

	private IEnumerator WaitForDelay() {
		yield return new WaitForSecondsRealtime(Lifespan);
		Disable();
	}

	public void Disable() {
		gameObject.SetActive(false);
	}
}
