using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
	public static ParticleSystemManager Instance;

	private static Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();
	private static Dictionary<string, CircularBuffer<Particle>> _buffers = new Dictionary<string, CircularBuffer<Particle>>();

	private void Awake() {
		Instance = this;

		GameObject[] gameObjects = Resources.LoadAll<GameObject>("Particle Systems");
		_prefabs = gameObjects.ToDictionary(x => x.name);
	}

	public void SpawnParticleSystem(string particleSystemName, Vector3 position) {
		if (_prefabs.TryGetValue(particleSystemName, out GameObject prefab)) {
			HandleBuffer(particleSystemName, prefab, position);
		} else {
			throw new UnityException("Please define the following particule system: " + particleSystemName);
		}
	}

	/// <summary>
	/// Register the Particle System by its name.
	/// </summary>
	/// <param name="particleSystemName">The particle system name.</param>
	/// <param name="prefab">The prefab.</param>
	public void RegisterParticleSystem(string particleSystemName, GameObject prefab) {
		if (!_prefabs.ContainsKey(particleSystemName)) {
			_prefabs.Add(particleSystemName, prefab);
		}
	}

	private void HandleBuffer(string key, GameObject prefab, Vector3 position) {
		if (!_buffers.ContainsKey(key)) {
			Particle particle = SpawnParticle(prefab);
			_buffers.Add(key, new CircularBuffer<Particle>(particle.Capacity));
			InsertParticleAt(key, particle, position);
		} else {
			if (_buffers[key].IsNull()) {
				InsertParticleAt(key, SpawnParticle(prefab), position);
			} else {
				_buffers[key].Next().Enable(position);
			}
		}
	}

	private Particle SpawnParticle(GameObject prefab) {
		GameObject @object = Instantiate(prefab, transform);
		Particle particle = @object.GetComponent<Particle>();
		if (ReferenceEquals(particle, null)) {
			throw new UnityException("Please assign a Particle component unto the GameObject: " + prefab.name);
		}
		return particle;
	}

	private void InsertParticleAt(string particleSystemName, Particle particle, Vector3 position) {
		_buffers[particleSystemName].Insert(particle);
		particle.Enable(position);
	}
}