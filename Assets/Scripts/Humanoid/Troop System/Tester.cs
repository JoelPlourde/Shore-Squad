using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TroopSystem;
using EncampmentSystem;
using UnityEngine.UI;
using GameSystem;
using System;
using TaskSystem;

public class Tester : MonoBehaviour
{
	public static Tester Instance;

	private Image image;

	private Map _map;

	public List<Encampment> Encampments1;
	public List<Encampment> Encampments2;

	public Actor actor;
	public NodeSystem.NodeBehaviour NodeBehaviour;

	void Awake() {
		Instance = this;
		image = GetComponent<Image>();
		_map = new Map(100, 100);
		image.sprite = Sprite.Create(_map.Texture, new Rect(0, 0, _map.Size.x, _map.Size.y), Vector2.zero);
	}

	public void Update() {
		if (Input.GetKeyUp(KeyCode.G)) {
			byte[] _bytes = _map.Texture.EncodeToPNG();
			System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + Guid.NewGuid() + ".png", _bytes);
		}

		if (Input.GetKeyUp(KeyCode.T)) {
			actor.TaskScheduler.CreateTask<Interact>(new InteractArguments(1f, NodeBehaviour.transform.position, NodeBehaviour));
		}
	}

	void Start()
    {
		TaxCollector inquisitor = TroopManager.InstantiateTroop("tax_collector", new Vector3(-20, 0, 0), FactionSystem.FactionType.INQUISITOR) as TaxCollector;
		inquisitor.StartCollection(100, Encampments1);

		TaxCollector sentinels = TroopManager.InstantiateTroop("tax_collector", new Vector3(20, 0, 0), FactionSystem.FactionType.SENTINELS) as TaxCollector;
		sentinels.StartCollection(100, Encampments2);
	}

	public void DrawMap(Map map) {
		_map = map;
		image.sprite = Sprite.Create(map.Texture, new Rect(0, 0, map.Size.x, map.Size.y), Vector2.zero);
	}
}
