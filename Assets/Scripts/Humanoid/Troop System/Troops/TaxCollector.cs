using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EncampmentSystem;
using TaskSystem;
using System.Linq;

namespace TroopSystem {
	public class TaxCollector : TroopBehaviour {

		private int _currency;              // How many to deposit/withdrawn from the coffer per encampment.
		private List<Encampment> _encampments;      // The encampment where to deposit/withdrawn from the coffer
		private int _index = -1;
		private Vector3 _initialPosition;

		private TaxCollectionState _collectionState;

		/// <summary>
		/// Start a sequence to deposit/withdrawn from a coffer.
		/// </summary>
		/// <param name="currency">How much currency to be deposit/withdrawn from the Encampment's coffer</param>
		/// <param name="encampment">The encampment to interact with.</param>
		public void StartCollection(int currency, List<Encampment> encampments) {
			_initialPosition = AveragePosition;
			_currency = currency;
			_encampments = encampments;

			_collectionState = TaxCollectionState.SETUP;

			CheckState();
		}

		public void CheckState() {
			switch (_collectionState) {
				case TaxCollectionState.SETUP:
					if (_encampments.Count - 1 > _index) {
						_index++;
						_collectionState = TaxCollectionState.TRAVEL;
					} else {
						_collectionState = TaxCollectionState.ORIGIN;
					}
					CheckState();
					break;
				case TaxCollectionState.TRAVEL:
					Vector3 destination = (_encampments[_index].transform.position - AveragePosition).normalized * _encampments[_index].InfluenceRadius;
					MoveTowards(_encampments[_index].transform.position - destination, OnTaskEnd);
					_collectionState = TaxCollectionState.COLLECT;
					break;
				case TaxCollectionState.COLLECT:
					Leader.TaskScheduler.CreateTask<Move>(new MoveArguments(_encampments[_index].transform.position));
					Leader.TaskScheduler.OnEndTaskEvent += OnTaskEnd;
					_collectionState = TaxCollectionState.TRANSFER;
					break;
				case TaxCollectionState.TRANSFER:
					_encampments[_index].AddToCoffer(_currency);
					_collectionState = TaxCollectionState.REGROUP;
					CheckState();
					break;
				case TaxCollectionState.REGROUP:
					Leader.TaskScheduler.CreateTask<Move>(new MoveArguments(AveragePosition));
					Leader.TaskScheduler.OnEndTaskEvent += OnTaskEnd;
					_collectionState = TaxCollectionState.SETUP;
					break;
				case TaxCollectionState.ORIGIN:
					MoveTowards(_initialPosition, OnCollectionEnd);
					break;
				default:
					break;
			}
		}

		private void OnTaskEnd(TaskBehaviour taskBehaviour) {
			Unsubscribe(OnTaskEnd);
			CheckState();
		}

		private void OnCollectionEnd(TaskBehaviour taskBehaviour) {
			Unsubscribe(OnTaskEnd);
		}
	}

	public enum TaxCollectionState {
		SETUP, TRAVEL, COLLECT, TRANSFER, REGROUP, ORIGIN
	}
}
