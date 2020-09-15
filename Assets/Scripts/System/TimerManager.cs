using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour {

	public static TimerManager Instance;

	private static readonly Queue<DelayedAction> _actions = new Queue<DelayedAction>();

	private void Awake() {
		Instance = this;
	}

	public void Enqueue(DelayedAction delayedAction) {
		_actions.Enqueue(delayedAction);
		if (!IsInvoking()) {
			InvokeRepeating(nameof(Routine), 0f, 0.016f);
		}
	}

	private void Routine() {
		if (Time.frameCount % 2 == 0) {
			if (_actions.Count > 0) {
				if (_actions.Peek().ReadyTime < DateTime.Now) {
					_actions.Dequeue().Action();
				}
			} else {
				CancelInvoke();
			}
		}
	}
}

public class DelayedAction {
	public Action Action { get; private set; }
	public DateTime ReadyTime { get; private set; }

	public DelayedAction(Action callback, float seconds) {
		Action = callback;
		ReadyTime = DateTime.Now.Add(TimeSpan.FromSeconds(seconds));
	}
}
