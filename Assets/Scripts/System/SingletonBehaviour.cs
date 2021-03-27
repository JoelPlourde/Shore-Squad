using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

	public static T Instance { get; protected set; }

	protected virtual void Awake() {

		if (Instance == null) {
			Instance = (T)FindObjectOfType(typeof(T));
			DontDestroyOnLoad(this);
		} else {
			throw new System.Exception("An instance of this singleton already exists.");
		}
		/*
		if (Instance != null && Instance != this) {
			Destroy(this);
			throw new System.Exception("An instance of this singleton already exists.");
		} else {
			Instance = (T) this;
		}
		*/
	}
}
