using UnityEngine;

namespace ElementalRift {
    public class ElementalOrbBehaviour : MonoBehaviour {
        private ParticleSystem _circle;
        private ParticleSystem _particles;
        private ParticleSystem _beam;
        private ParticleSystem _smoke;

        void Awake() {
            Transform parent = transform.Find("Particle Systems");

            _particles = parent.Find("PS_Particles").GetComponent<ParticleSystem>();
            _circle = parent.Find("PS_Circle").GetComponent<ParticleSystem>();
            _beam = parent.Find("PS_Beam").GetComponent<ParticleSystem>();
            _smoke = parent.Find("PS_Smoke").GetComponent<ParticleSystem>();
        }

        public void Update() {
            // If the user presses up on key H
            if (Input.GetKeyDown(KeyCode.H)) {
                AdjustColors(Color.green, Color.blue);
            }
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