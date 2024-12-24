using UnityEngine;

namespace ElementalRift {
    public class OrbBehaviour : MonoBehaviour {

        // This value will be in the RiftBehavior
        public float _percentage = 1.0f;

        private Transform _orb;

        private ParticleSystem _circle;
        private ParticleSystem _particles;
        private ParticleSystem _beam;
        private ParticleSystem _smoke;

        /** For reference, we'll scale these value over time, but we take a copy of what the Inspector value was. **/
        private float _default_particle_emission_rate;
        private float _default_beam_emission_rate;
        private float _default_circle_emission_rate;
        private float _default_smoke_emission_rate;

        void Awake() {
            _orb = transform.Find("Particle Systems");

            _particles = _orb.Find("PS_Particles").GetComponent<ParticleSystem>();
            _circle = _orb.Find("PS_Circle").GetComponent<ParticleSystem>();
            _beam = _orb.Find("PS_Beam").GetComponent<ParticleSystem>();
            _smoke = _orb.Find("PS_Smoke").GetComponent<ParticleSystem>();

            _default_particle_emission_rate = _particles.emission.rateOverTime.constant;
            _default_beam_emission_rate = _beam.emission.rateOverTime.constant;
            _default_circle_emission_rate = _circle.emission.rateOverTime.constant;
            _default_smoke_emission_rate = _smoke.emission.rateOverTime.constant;
        }

        public void Update() {
            // If the user presses up on key H
            if (Input.GetKeyUp(KeyCode.A)) {
                ChangeElement(ElementType.AIR);
            }

            if (Input.GetKeyUp(KeyCode.F)) {
                ChangeElement(ElementType.FIRE);
            }

            if (Input.GetKeyUp(KeyCode.L)) {
                ChangeElement(ElementType.LIFE);
            }

            if (Input.GetKeyUp(KeyCode.D)) {
                ChangeElement(ElementType.DEATH);
            }

            if (Input.GetKeyUp(KeyCode.W)) {
                ChangeElement(ElementType.WATER);
            }
            
            if (Input.GetKeyUp(KeyCode.E)) {
                ChangeElement(ElementType.EARTH);
            }

            if (Input.GetKeyUp(KeyCode.P)) {
                _percentage += 0.1f;
                ScaleOrb(_percentage);
                Debug.Log("Percentage: " + _percentage);
            }

            if (Input.GetKeyUp(KeyCode.M)) {
                _percentage -= 0.1f;
                ScaleOrb(_percentage);
                Debug.Log("Percentage: " + _percentage);
            }
        }

        /**
        * This method will scale the orb based on the percentage.
        **/
        private void ScaleOrb(float percentage) {
            var _particleEmission = _particles.emission;
            _particleEmission.rateOverTime = _default_particle_emission_rate * percentage;

            var _beamEmission = _beam.emission;
            _beamEmission.rateOverTime = _default_beam_emission_rate * percentage;

            var _circleEmission = _circle.emission;
            _circleEmission.rateOverTime = _default_circle_emission_rate * percentage;

            var _smokeEmission = _smoke.emission;
            _smokeEmission.rateOverTime = _default_smoke_emission_rate * percentage;

            LeanTween.scale(_orb.gameObject, Vector3.one * percentage, 0.5f);
        }

        private void ChangeElement(ElementType elementType) {
            ElementData elementData = ElementManager.Instance.GetElementData(elementType);

            AdjustColors(elementData.PrimaryColor, elementData.SecondaryColor);
            UpdateExternalForce(elementData.LayerMask);
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