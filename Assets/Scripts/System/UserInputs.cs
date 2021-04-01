using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseButton {
	LEFT_MOUSE_BUTTON, RIGHT_MOUSE_BUTTON, MIDDLE_MOUSE_BUTTON
}

public class UserInputs : MonoBehaviour
{
	public static UserInputs Instance;

	public LayerMask LayerMask;

	private Dictionary<string, Action<MouseButton, RaycastHit>> _actions = new Dictionary<string, Action<MouseButton, RaycastHit>>();
	private RaycastHit _hit;
	private Action<MouseButton, RaycastHit> _value;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else {
			throw new UnityException("A single entity of UserInputs can exists at any time.");
		}
	}

	private void Update()
    {
		if (Input.GetMouseButtonUp(0)) {
			OnMouseEvent(MouseButton.LEFT_MOUSE_BUTTON);
		} else if (Input.GetMouseButtonUp(1)) {
			OnMouseEvent(MouseButton.RIGHT_MOUSE_BUTTON);
		}
	}

	private void OnMouseEvent(MouseButton mouseButton) {
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _hit, Mathf.Infinity, LayerMask)) {
			if (_actions.TryGetValue(_hit.collider.name, out _value) || _actions.TryGetValue(_hit.collider.tag, out _value)) {
				_value?.Invoke(mouseButton, _hit);
			}
		}
	}

	/// <summary>
	/// Subscribe to a topic. The action will be triggered when the user clicks on an object with the same tag and/or name.
	/// </summary>
	/// <param name="topic">The topic to subscribe to.</param>
	/// <param name="action">The action to be triggered.</param>
	public void Subscribe(string topic, Action<MouseButton, RaycastHit> action) {
		if (_actions.TryGetValue(topic, out Action<MouseButton, RaycastHit> value)) {
			_actions[topic] += action;
		} else {
			_actions.Add(topic, action);
		}
	}

	/// <summary>
	/// Unsubscribe from a topic.
	/// </summary>
	/// <param name="topic">The topic to subscribe to.</param>
	/// <param name="action">The action to be triggered.</param>
	public void Unsubscribe(string topic, Action<MouseButton, RaycastHit> action) {
		if (_actions.TryGetValue(topic, out Action<MouseButton, RaycastHit> value)) {
			_actions[topic] -= action;
			if (_actions[topic].GetInvocationList().Length == 0) {
				_actions.Remove(topic);
			}
		}
	}
}