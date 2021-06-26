using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTester : MonoBehaviour
{
	public Actor actor;

	public int mouth = 0;
	public int eyebrow = 0;
	public int eyes = 0;

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyUp(KeyCode.H)) {
			eyes--;
			if (eyes < 0) {
				eyes = 7;
			}
			actor.Face.ChangeFeature(BodySystem.FeatureType.EYES, eyes);
		}

		if (Input.GetKeyUp(KeyCode.J)) {
			eyes++;
			if (eyes > 7) {
				eyes = 0;
			}
			actor.Face.ChangeFeature(BodySystem.FeatureType.EYES, eyes);
		}

		if (Input.GetKeyUp(KeyCode.Y)) {
			mouth--;
			if (mouth < 0) {
				mouth = 7;
			}
			actor.Face.ChangeFeature(BodySystem.FeatureType.MOUTH, mouth);
		}

		if (Input.GetKeyUp(KeyCode.U)) {
			mouth++;
			if (mouth > 7) {
				mouth = 0;
			}
			actor.Face.ChangeFeature(BodySystem.FeatureType.MOUTH, mouth);
		}

		if (Input.GetKeyUp(KeyCode.N)) {
			eyes--;
			if (eyes < 0) {
				eyes = 7;
			}
			actor.Face.ChangeFeature(BodySystem.FeatureType.EYEBROW, eyes);
		}

		if (Input.GetKeyUp(KeyCode.M)) {
			eyebrow++;
			if (eyebrow > 7) {
				eyebrow = 0;
			}
			actor.Face.ChangeFeature(BodySystem.FeatureType.EYEBROW, eyebrow);
		}
	}
}
