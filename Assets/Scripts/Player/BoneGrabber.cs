using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneGrabber : MonoBehaviour {

	public int numberOfBones;

	void OnStart() {
		numberOfBones = 3;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Bone") {
			coll.gameObject.GetComponentInParent<AudioSource> ().Play();
			GameObject.Destroy (coll.gameObject);
			numberOfBones++;
		}
	}
}
