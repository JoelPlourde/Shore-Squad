using System;
using CameraSystem;
using UnityEngine;

namespace SaveSystem {
    [Serializable]
    public class CameraTargetDto {

        [SerializeField]
        public Vector3 Position;

        public CameraTargetDto() {
            Position = Vector3.zero;
        }

        public CameraTargetDto(CameraTarget cameraTarget) {
            Position = cameraTarget.transform.position;
        }
    }
}