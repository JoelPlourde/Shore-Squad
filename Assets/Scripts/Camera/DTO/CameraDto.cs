using System;
using CameraSystem;
using UnityEngine;

namespace SaveSystem {
    [Serializable]
    public class CameraDto {

        [SerializeField]
        public Vector3 Position;

        [SerializeField]
        public Vector3 Rotation;

        [SerializeField]
        public float Distance;

        public CameraDto() {
            Position = new Vector3(0, 5, -5);
            Rotation = new Vector3(35, 0, 0);
            Distance = 15;
        }

        public CameraDto(Vector3 expectedPosition, Vector3 expectedRotation, float expectedDistance) {
            Position = expectedPosition;
            Rotation = expectedRotation;
            Distance = expectedDistance;
        }
    }
}