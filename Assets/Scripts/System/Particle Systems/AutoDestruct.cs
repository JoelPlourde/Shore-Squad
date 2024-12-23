using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestruct : MonoBehaviour {

    public float minimumDelay = 0.5f;

    private ParticleSystem[] _particleSystems;

    private float _timeAlive = 0;

    private void Awake() {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    public void Update() {
        _timeAlive += Time.deltaTime;

        if (_timeAlive < minimumDelay) {
            return;
        }

        foreach (ParticleSystem particleSystem in _particleSystems) {

            // Once the particle system has 0 particles, destroy the game object
            if (particleSystem.particleCount == 0) {
                Destroy(gameObject);
            }
        }
    }

    public void Destroy() {
        foreach (ParticleSystem particleSystem in _particleSystems) {
            var emission = particleSystem.emission;
            emission.rateOverTime = 0;
        }
    }
}