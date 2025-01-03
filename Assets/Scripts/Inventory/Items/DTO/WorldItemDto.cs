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

        [SerializeField]
        public SerializableDictionary<string, string> conditionalData;

        public WorldItemDto() {
            Position = Vector3.zero;
        }

        public WorldItemDto(string uuid, string id, int amount, Vector3 position, Vector3 rotation) : base(id, amount) {
            UUID = uuid;
            Position = position;
            Rotation = rotation;
        }

        /// <summary>
        /// Append data to the conditional data.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AppendData(string key, string value) {
            if (ReferenceEquals(conditionalData, null)) {
                conditionalData = new SerializableDictionary<string, string>();
            }
            conditionalData.Add(key, value);
        }
    }
}
