using GameSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tester2 : MonoBehaviour {

	public static Tester2 Instance;

	private Image image;

	[SerializeField]
	private Map _map;

    void Awake()
    {
		Instance = this;
		image = GetComponent<Image>();
		_map = new Map(100);
		image.sprite = Sprite.Create(_map.Texture, new Rect(0, 0, _map.Size.x, _map.Size.y), Vector2.zero);
	}

	public void DrawMap(Map map) {
		Debug.Log("Displaying the map on screen !");
		image.sprite = Sprite.Create(map.Texture, new Rect(0, 0, map.Size.x, map.Size.y), Vector2.zero);
	}
}
