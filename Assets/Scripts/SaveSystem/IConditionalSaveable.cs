using UnityEngine;

namespace SaveSystem {
    /// <summary>
    /// Interface for objects that require a save, but only under certain conditions.
    /// </summary>
    public interface IConditionalSaveable {

        /// <summary>
        /// Check if the object requires saving.
        /// </summary>
        /// <returns>Whether the object requires saving.</returns>
        bool IsSaveRequired();

        /// <summary>
        /// Get the serialized data of the object.
        /// </summary>
        /// <returns>A string representing the object to be saved.</returns>
        string GetSerializedData();

        /// <summary>
        /// Method that dictates how the object should be loaded from the serialized data.
        /// </summary>
        /// <param name="serializedData">The serialized data</param>
        void LoadSerializedData(string serializedData);
    }
}