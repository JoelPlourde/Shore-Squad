using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapUtils
{
	public static Vector3 GetMapPositionFromWorldPosition(Vector3 WorldPosition) {
		return new Vector3(
			RoundToNearest(WorldPosition.x),
			RoundToNearest(WorldPosition.y),
			RoundToNearest(WorldPosition.z)
		);
	}

	public static float RoundToNearest(float value) {
		float decimals = value - (float) Math.Truncate(value);
		float roundedDecimals = 0f;
		if (decimals <= 0.25f) {
			roundedDecimals = 0f;
		} else if (decimals <= 0.75f) {
			roundedDecimals = 0.5f;
		} else if (decimals <= 1f) {
			roundedDecimals = 1f;
		}

		value -= decimals;
		value += roundedDecimals;
		return value;
	}
}
