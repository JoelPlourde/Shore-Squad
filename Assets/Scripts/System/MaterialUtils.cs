using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialUtils {

	public readonly static Material TOON;
	public readonly static Material INVISIBLE;

	static MaterialUtils() {
		TOON = Resources.Load<Material>("Materials/Toon");
		INVISIBLE = Resources.Load<Material>("Materials/Invisible");
	}
}
