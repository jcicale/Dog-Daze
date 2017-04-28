using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public GameObject winks;

	private float distance;
	private bool isAlive;
	private bool distanceTripped;
	private bool isMoving;
	private bool isStuck;
	private float directionFacing;
	private bool stopLerping;

	private Vector3 positionOnScreen;


	private Animator animator;
	private Rigidbody2D enemyBody;

	private int moveSpeed;

	// Use this for initialization
	void Start () {
		isAlive = true;
		isMoving = false;
		isStuck = false;
		stopLerping = false;
		moveSpeed = 3;
		distance = Vector3.Distance(winks.transform.position, transform.position);
		distanceTripped = false;
		directionFacing = -1;

		positionOnScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0f));

		animator = GetComponent<Animator> ();
		enemyBody = GetComponent<Rigidbody2D> ();

	}

	
	// Update is called once per frame
	void Update () {

		if (distanceTripped) {
			moveMotion ();
		}

		distance = transform.position.x - winks.transform.position.x;
		if (distance < 4f) {
			if (gameObject.layer == LayerMask.NameToLayer ("FlyingEnemy") && !stopLerping) {
				float time = (moveSpeed * Time.deltaTime)/3f;
				Vector3 x = new Vector3 (0, .7f);
				x = winks.transform.position - x;
				if (distance > .5f) {
					transform.position = Vector3.Lerp (transform.position, x, time);
				} else {
					stopLerping = true;
				}
			}
			distanceTripped = true;
			isMoving = true;

		}

			
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.layer == LayerMask.NameToLayer("ForegroundObjects")) {
			isStuck = true;
		}

	}

	public void moveMotion() {
		transform.localScale = new Vector3(1, 1, 1);
		enemyBody.velocity = new Vector2(directionFacing * moveSpeed, enemyBody.velocity.y);
		if (isStuck && GetComponent<Renderer>().isVisible) {
			transform.Rotate (0, 180, 0);
			directionFacing = directionFacing * -1;
			isStuck = false;
		}
	}

	void OnBecameInvisible() {
		if (gameObject.layer != LayerMask.NameToLayer("StuckEnemy")) {
			if (distanceTripped) {
				Destroy (gameObject);
			}
		}
	}
}
