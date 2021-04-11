using DropSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeSystem {
	public class NodeBehaviour : MonoBehaviour, IInteractable {

		[SerializeField]
		private NodeData _nodeData;

		private int currentCapacity = 0;

		private void Awake() {

		}

		public void OnInteractEnter() {
			Debug.Log("Interact Enter !");

			if (!IsInvoking()) {
				InvokeRepeating(nameof(Routine), Constant.SCALED_TICK_RATE, Constant.SCALED_TICK_RATE);
			}
		}

		public void Routine() {
			currentCapacity++;
			if (currentCapacity > _nodeData.HarvestCapacity) {
				// Calculate a probability to deplete
				int rand = Random.Range(0, 101);
				if (rand < _nodeData.Probability) {
					CancelInvoke();
					Debug.Log("Deplete !");
					// OnInteractExit();
				}
			} else {
				Drop drop = _nodeData.DropTable.GetRandomDrop();
				Debug.Log("Harvested: " + drop.ToString());
			}
		}

		public void OnInteractExit() {
			Debug.Log("Interact Exit !");
		}
	}
}
