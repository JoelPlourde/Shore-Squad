using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StatusEffectSystem {
	namespace UI {
		[RequireComponent(typeof(GridLayoutGroup))]
		public class StatusHandler : MonoBehaviour {

			public GameObject statusComponentPrefab;

			private List<StatusComponent> _statusComponents = new List<StatusComponent>();
			private bool _initialized = false;

			private void Awake() {
				if (statusComponentPrefab == null) {
					throw new UnityException("Please provide the prefab for the StatusEffectComponent!");
				}

				GridLayoutGroup = GetComponent<GridLayoutGroup>();
				GridLayoutGroup.cellSize = new Vector2(25, 25);
				GridLayoutGroup.spacing = new Vector2(2, 2);
				GridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
				GridLayoutGroup.constraintCount = 8;
			}

			public void Initialize(Guid guid) {
				Debug.Log("Initialized status handler !");
				if (_initialized == false) {
					_initialized = true;
					StatusEffectScheduler.Instance(guid).OnAddStatusEffectEvent += OnAdd;
					StatusEffectScheduler.Instance(guid).OnRemoveStatusEffectEvent += OnRemove;
					StatusEffectScheduler.Instance(guid).OnUpdateStatusEffectsEvent += OnBulkUpdate;
					StatusEffectScheduler.Instance(guid).OnUpdateStatusEffectEvent += OnUpdate;
				} else {
					Debug.LogError("This component has already been initialized with: " + guid.ToString());
				}
			}

			public void OnAdd(Status status) {
				Debug.Log("Adding: " + status.StatusEffectData.Name);
				GameObject statusEffectComponentObj = Instantiate(statusComponentPrefab, transform);
				statusEffectComponentObj.transform.SetAsLastSibling();
				statusEffectComponentObj.transform.name = status.StatusEffectData.Name;

				StatusComponent statusEffectComponent = statusEffectComponentObj.GetComponent<StatusComponent>();
				statusEffectComponent.Initialize(status, this);
				_statusComponents.Add(statusEffectComponent);
			}

			public void OnRemove(string name) {
				int index = _statusComponents.FindIndex(x => x.transform.name == name);
				if (index != -1) {
					StatusComponent statusEffectComponent = _statusComponents[index];
					_statusComponents.RemoveAt(index);
					statusEffectComponent.Destroy();
				}
			}

			public void OnUpdate(Status status) {
				int index = _statusComponents.FindIndex(x => x.transform.name == status.StatusEffectData.Name);
				if (index != -1) {
					_statusComponents[index].UpdateDuration(status.Duration);
					_statusComponents[index].UpdateStack(status.Stack);
				} else {
					Debug.LogError("Trying to update an unknown status: " + status.StatusEffectData.Name);
				}
			}

			public void OnBulkUpdate(List<Status> statuses) {
				foreach (Status status in statuses) {
					OnUpdate(status);
				}
			}

			public GridLayoutGroup GridLayoutGroup { get; private set; }
		}
	}
}
