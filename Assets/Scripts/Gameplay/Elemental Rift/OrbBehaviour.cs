using System.Collections.Generic;
using UnityEngine;

namespace ElementalRift {
    public class OrbBehaviour : MonoBehaviour {

        private ParticleSystem _circle;
        private ParticleSystem _particles;
        private ParticleSystem _beam;
        private ParticleSystem _smoke;

        /** For reference, we'll scale these value over time, but we take a copy of what the Inspector value was. **/
        private float _default_particle_emission_rate;
        private float _default_beam_emission_rate;
        private float _default_circle_emission_rate;
        private float _default_smoke_emission_rate;

        private RiftBehaviour _riftBehaviour;

        void Awake() {
            // Find all the required component
            _particles = transform.Find("PS_Particles").GetComponent<ParticleSystem>();
            _circle = transform.Find("PS_Circle").GetComponent<ParticleSystem>();
            _beam = transform.Find("PS_Beam").GetComponent<ParticleSystem>();
            _smoke = transform.Find("PS_Smoke").GetComponent<ParticleSystem>();

            // Initialize the collision module in the Particle Systems
            var collision = _particles.collision;
            collision.type = ParticleSystemCollisionType.World;
            collision.quality = ParticleSystemCollisionQuality.High;
            collision.lifetimeLoss = 1.0f;
            collision.sendCollisionMessages = true;

            // Subscribe to the ParticleCollider
            _particles.GetComponent<ParticleCollider>().Initialize(this);

            // Record the default values
            _default_particle_emission_rate = _particles.emission.rateOverTime.constant;
            _default_beam_emission_rate = _beam.emission.rateOverTime.constant;
            _default_circle_emission_rate = _circle.emission.rateOverTime.constant;
            _default_smoke_emission_rate = _smoke.emission.rateOverTime.constant;
        }

        /**
        * This method will initialize the OrbBehaviour with the RiftBehaviour.
        **/
        public void Initialize(RiftBehaviour riftBehaviour) {
            _riftBehaviour = riftBehaviour;
        }

        public void SpawnOrb(ElementType primaryElementType, ElementType secondaryElementType = ElementType.NONE) {
            ChangeElement(primaryElementType, secondaryElementType);
            ScaleOrb(1.0f);
        }

        public void OnParticleCollision(GameObject other) {
            string layer = LayerMask.LayerToName(other.layer);
            GameObject rune = other.transform.parent.gameObject;
            switch (layer) {
                case "Air Element":
                    _riftBehaviour.ReduceHealth(rune, ElementType.AIR, 1);
                    break;
                case "Fire Element":
                    _riftBehaviour.ReduceHealth(rune, ElementType.FIRE, 1);
                    break;
                case "Water Element":
                    _riftBehaviour.ReduceHealth(rune, ElementType.WATER, 1);
                    break;
                case "Earth Element":
                    _riftBehaviour.ReduceHealth(rune, ElementType.EARTH, 1);
                    break;
                case "Life Element":
                    _riftBehaviour.ReduceHealth(rune, ElementType.LIFE, 1);
                    break;
                case "Death Element":
                    _riftBehaviour.ReduceHealth(rune, ElementType.DEATH, 1);
                    break;
                default:
                    Debug.Log("Unknown Layer: " + layer);
                    break;
            }
        }

        /**
        * This method will scale the orb based on the percentage.
        **/
        public void ScaleOrb(float percentage) {
            var _particleEmission = _particles.emission;
            _particleEmission.rateOverTime = _default_particle_emission_rate * percentage;

            var _beamEmission = _beam.emission;
            _beamEmission.rateOverTime = _default_beam_emission_rate * percentage;

            var _circleEmission = _circle.emission;
            _circleEmission.rateOverTime = _default_circle_emission_rate * percentage;

            var _smokeEmission = _smoke.emission;
            _smokeEmission.rateOverTime = _default_smoke_emission_rate * percentage;

            LeanTween.scale(transform.gameObject, Vector3.one * percentage, 0.5f);
        }

        public void ChangeElement(ElementType primaryElementType, ElementType secondaryElementType = ElementType.NONE) {
            ElementData primaryElementData = ElementManager.Instance.GetElementData(primaryElementType);
            ElementData secondaryElementData;
            if (secondaryElementType != ElementType.NONE) {
                secondaryElementData = ElementManager.Instance.GetElementData(secondaryElementType);
                AdjustColors(primaryElementData.PrimaryColor, secondaryElementData.SecondaryColor);
                UpdateExternalForce(primaryElementData.LayerMask);
                return;
            }

            AdjustColors(primaryElementData.PrimaryColor, primaryElementData.SecondaryColor);
            UpdateExternalForce(primaryElementData.LayerMask);
        }

        public void UpdateExternalForce(LayerMask LayerMask) {
            var externalForces = _particles.externalForces;
            externalForces.influenceMask = LayerMask;
        }

        public void AdjustColors(Color startColor, Color endColor) {
            Gradient gradient = BuildGradient(startColor, endColor);

            AdjustColor(_beam, gradient);
            AdjustColor(_circle, gradient);
            AdjustColor(_smoke, gradient);
            AdjustColor(_particles, gradient);
        }

        private void AdjustColor(ParticleSystem particleSystem, Gradient gradient) {
            var colorOverLifetime = particleSystem.colorOverLifetime;
            var startColorModule = particleSystem.main.startColor;
            startColorModule = gradient;
            colorOverLifetime.color = gradient;
        }

        private Gradient BuildGradient(Color startColor, Color endColor) {
            Gradient gradient = new Gradient();
            gradient.SetKeys(new GradientColorKey[] {
                new GradientColorKey(startColor, 0.0f),
                new GradientColorKey(endColor, 1.0f) },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(0.0f, 0.0f),
                    new GradientAlphaKey(1.0f, 0.5f),
                    new GradientAlphaKey(0.0f, 1.0f)
                }
            );
            return gradient;
        }
    }
}