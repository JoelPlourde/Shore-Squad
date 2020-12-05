using ItemSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EncampmentSystem {
	/// <summary>
	/// This class keeps track of the encampment's resources. Which storage this encampment has, how much resources it has ?
	/// </summary>
	[System.Serializable]
	public class Resources {

		// This variable keeps track of which Items and in which quantity the encampment has.
		private Dictionary<string, int> _resources = new Dictionary<string, int>();

		// Contains all storage solution that contains the resources.
		private List<Storage> _storages = new List<Storage>();

		public void AddResources(string name, int value) {
			if (!_resources.ContainsKey(name)) {
				_resources.Add(name, value);
			} else {
				_resources[name] += value;
			}
		}

		public void RemoveResources(string name, int value) {
			if (_resources.ContainsKey(name)) {
				if (_resources[name] >= value) {
					_resources[name] -= value;
				} else {
					throw new UnityException("The resources value is less than what you are trying to remove from.");
				}
			} else {
				throw new UnityException("The resources is not found within this encampment. Please ensure that your code works.");
			}
		}

		public int GetResourcesValue(string name) {
			return (_resources.ContainsKey(name)) ? _resources[name] : 0;
		}

		public void RegisterStorage(Storage storage) {
			_storages.Add(storage);

			// Add all the resources contained within the storage to the Resources dictionary.
			foreach (var groupedItem in storage.Inventory.GetGroupedItems()) {
				AddResources(groupedItem.Key, groupedItem.Value);
			}
		}

		public void UnregisterStorage(Storage storage) {
			_storages.Remove(storage);

			foreach (var groupedItem in storage.Inventory.GetGroupedItems()) {
				RemoveResources(groupedItem.Key, groupedItem.Value);
			}
			// Remove all the resources contained within the storage to the Resources dictionary.
		}
	}
}
