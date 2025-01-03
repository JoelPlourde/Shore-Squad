using System;
using UnityEngine;

namespace SaveSystem {
    [Serializable]
    public class WorldItemDto : ItemDto {

        [SerializeField]
        public string UUID;

        [SerializeField]
        public Vector3 Position;

        [SerializeField]
        public Vector3 Rotation;

        public WorldItemDto() {
            Position = Vector3.zero;
        }

        public WorldItemDto(string uuid, string id, int amount, Vector3 position, Vector3 rotation) : base(id, amount) {
            UUID = uuid;
            Position = position;
            Rotation = rotation;
        }
    }
}
