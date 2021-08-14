using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour {

	public static IconManager Instance;

	private static Dictionary<string, Sprite> _icons = new Dictionary<string, Sprite>();

	private void Awake() {
		Instance = this;

		Sprite[] spriteDatas = Resources.LoadAll<Sprite>("Sprites");
		foreach (Sprite sprite in spriteDatas) {
			_icons.Add(sprite.name, sprite);
		}
	}

	public Sprite GetSprite(string name) {
		if (_icons.TryGetValue(name, out Sprite sprite)) {
			return sprite;
		} else {
			throw new UnityException("This icon is not found under Resources/Sprites, please define this: " + name);
		}
	}
}
