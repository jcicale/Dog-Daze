using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour {
	public float speed;
	public float autoscroll;

	Camera mainCamera;
	Vector3 parallaxFollowCamera;

	private float scroll;
	private float offSet;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		parallaxFollowCamera = transform.position;
		offSet = transform.position.x;

	}

	// Update is called once per frame
	void LateUpdate () {
		scroll += autoscroll;
		parallaxFollowCamera.x = mainCamera.transform.position.x * speed + scroll + offSet;
		transform.position = parallaxFollowCamera;
	}
}
