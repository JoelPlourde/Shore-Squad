using UnityEngine;

namespace SaveSystem {
    /// <summary>
    /// Interface for objects that can be saved in the world.
    /// </summary>
    public interface IWorldSaveable : ISaveable{

        /// <summary>
        /// Compare the state of the object in the world with the state of the object in the player save to determine how to handle the object.
        /// </summary>
        /// <param name="worldSave">The world save</param>
        /// <param name="playerSave">The player save</param>
        /// <returns>Whether to load this object.</returns>
        bool DetermineState(Save worldSave, Save playerSave);

        /// <summary>
		/// Get the UUID of the object in the world.
		/// </summary>
		/// <returns>The UUID if any, else an empty string.</returns>
		string GetUUID();
    }
}