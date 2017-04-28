using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D winksBody;
	private SpriteRenderer renderer;
	private PolygonCollider2D carBone;
	private PolygonCollider2D carTop;
    private float horizontalMotion;
    private bool jumpActivated;
	private bool runActivated;

	private float hurtClock = 3.0f;
	private float poweredUpClock = 8.0f;
	[HideInInspector] public bool isHurt; 
	private bool isPoweredUp;

	public static bool inDoghouse;

	public static int numberOfBones;

    public static int moveSpeed;

	public static bool isOffScreen;

	private Animator animator;

	void Start()
	{
        horizontalMotion = 0;
        moveSpeed = 2;
		numberOfBones = 3;
		jumpActivated = false;
		runActivated = false;
		isHurt = false;
		isPoweredUp = false;
		inDoghouse = false;
		isOffScreen = false;

        winksBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		renderer = GetComponent<SpriteRenderer> ();
		carBone = GameObject.Find ("Bone2").GetComponent<PolygonCollider2D> ();
		carTop = GameObject.Find ("CarTop").GetComponent<PolygonCollider2D> ();


        PlayerState.Instance.Horizontal = Horizontal.Idle;
        PlayerState.Instance.Vertical = Vertical.Grounded;
        PlayerState.Instance.DirectionFacing = DirectionFacing.Right;
	}

    void FixedUpdate()
    {
        WalkMotion();
        JumpMotion();
    }

	void Update()
	{
		stopAtScreenEdge ();

		if(isHurt) {
			hurtCountdown();
		}

		if (isPoweredUp) {
			poweredUpCountdown ();
		}

		if (winksBody.velocity.y == 0)
			PlayerState.Instance.Vertical = Vertical.Grounded;
		

        horizontalMotion = Input.GetAxisRaw("Walk");

        if (horizontalMotion != 0) {
            transform.localScale = new Vector3(horizontalMotion, 1, 1);
            PlayerState.Instance.DirectionFacing = (DirectionFacing)horizontalMotion;
        }

		if (Input.GetButtonDown ("Jump")) {
			jumpActivated = true;
		}

		if (Input.GetButtonDown ("Run")) {
			runActivated = true;
			moveSpeed = 4;
		} else if (Input.GetButtonUp ("Run")) {
			runActivated = false;
			moveSpeed = 2;
		}
			
		if (checkIfAirborne ()) {
				animator.SetTrigger ("winksJump");
			}


		if (horizontalMotion != 0 && runActivated) {
			if (!checkIfAirborne ()) {
				animator.SetTrigger ("winksRun");
			}				
		} else if (horizontalMotion != 0 && !checkIfAirborne()) {
			animator.SetTrigger ("winksWalk");
		} 
			

        Horizontal previousMotion = PlayerState.Instance.Horizontal;
        Horizontal currentMotion = PlayerState.Instance.Horizontal = (Horizontal)horizontalMotion;

        //Fixes an error with the camera following the player incorrectly if quickly changing direction
        //while at the furthest possible positions at each side of the screen.
		if ((int)previousMotion * (int)currentMotion == -1) {
			PlayerState.Instance.Horizontal = Horizontal.Idle;
		}
	}

    private void WalkMotion()
    {
        winksBody.velocity = new Vector2(horizontalMotion * moveSpeed, winksBody.velocity.y);
    }
		
    private void JumpMotion()
    {
        if (jumpActivated)
        {
            if (PlayerState.Instance.Vertical == Vertical.Grounded)
            {
                PlayerState.Instance.Vertical = Vertical.Airborne;
                winksBody.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
				animator.SetTrigger ("winksJump");
                GetComponents<AudioSource>()[0].Play();
            }
			jumpActivated = false;
        }
    }

	private bool checkIfAirborne() {
		if (PlayerState.Instance.Vertical == Vertical.Airborne) {
			return true;
		}
		else {
			PlayerState.Instance.Vertical = Vertical.Grounded;
			return false;
		}
	}
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Exit") {
			inDoghouse = true;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.tag == "Bone") {
			GameObject.Find("FriendlyMan").GetComponent<AudioSource>().Play();
			GameObject.Destroy (coll.gameObject);
			numberOfBones++;
		}

		if (coll.gameObject.tag == "Ball") {
			isPoweredUp = true;
			animator.SetBool ("poweredUp", true);
			animator.SetTrigger ("powerUpTrigger");
			GameObject.Destroy (coll.gameObject);

			GameObject.Find ("Background Music").gameObject.GetComponent<AudioSource>().Pause ();
			GameObject.Find ("PowerUp Music").gameObject.GetComponent<AudioSource>().Play ();
		}

		if (coll.gameObject.tag == "Enemy" && !isPoweredUp) {
			if (!isHurt) {
				numberOfBones--;
				isHurt = true;
				if (numberOfBones > 0) {
					GetComponents<AudioSource> () [1].Play ();
				} else if (numberOfBones == 0) {
					GameObject.Find("Yelp").GetComponent<AudioSource> ().Play();
				}
			}
		} else if (coll.gameObject.tag == "Enemy" && isPoweredUp) {
			string name = coll.gameObject.transform.name.ToString();
			if (name == "Squirrel1" || name == "Squirrel2") {
				coll.gameObject.GetComponentsInParent<AudioSource> () [0].Play ();
			} else if (name == "Elephant1" || name == "Elephant2") {
				coll.gameObject.GetComponentsInParent<AudioSource> () [1].Play ();
			} else if (name == "Bird1") {
				coll.gameObject.GetComponentsInParent<AudioSource> () [2].Play ();
			}
			GameObject.Destroy (coll.gameObject);
		}

		if (coll.gameObject.tag == "Enemy" && isHurt) {
			Physics2D.IgnoreCollision (GetComponent<PolygonCollider2D> (), coll.gameObject.GetComponent<PolygonCollider2D>(), true);
		}

		if (coll.gameObject.tag == "Car" && isHurt) {
			Physics2D.IgnoreCollision (GetComponent<PolygonCollider2D> (), coll.gameObject.GetComponent<PolygonCollider2D>(), true);

		}

		if (coll.gameObject.tag == "Car" && isPoweredUp) {
			coll.gameObject.GetComponentsInParent<AudioSource> () [3].Play ();
			GameObject.Destroy (coll.gameObject);
		} else if (coll.gameObject.tag == "Car" && !isPoweredUp && coll.collider == carBone) {
			GameObject.Find ("FriendlyMan").GetComponent<AudioSource> ().Play ();
			GameObject.Destroy (GameObject.Find("Bone2"));
			numberOfBones++;
		} else if (coll.gameObject.tag == "Car" && !isPoweredUp && coll.collider == carTop) {
			GetComponents<AudioSource> () [1].Play ();
		} else if (coll.gameObject.tag == "Car" && !isPoweredUp) {
			if (!isHurt) {
				numberOfBones--;
				isHurt = true;
				GetComponents<AudioSource> () [1].Play ();
			}
		}


	}

	private void hurtCountdown() {
		hurtClock -= Time.deltaTime;
		if (hurtClock <= 0) {
			isHurt = false;
			renderer.color = new Color (1f, 1f, 1f, 1f);
			hurtClock = 3.0f;
			return;
		}
		renderer.color = new Color (1f, 1f, 1f, .5f);
	}

	public bool checkIfHurt() {
		return isHurt;
	}

	private void poweredUpCountdown() {
		poweredUpClock -= Time.deltaTime;
		if (poweredUpClock <= 0) {
			GameObject.Find ("PowerUp Music").gameObject.GetComponent<AudioSource> ().Stop();
			GameObject.Find ("Background Music").gameObject.GetComponent<AudioSource>().UnPause ();
			animator.SetBool ("poweredUp", false);
			isPoweredUp = false;
		}

	}

	private void stopAtScreenEdge() {
		if (transform.position.x <= -23f) {
			transform.position = new Vector2 (-23f, transform.position.y);
		} else if (transform.position.x >= 55f) {
			transform.position = new Vector2 (55f, transform.position.y);
		}
	}

	void OnBecameInvisible() {
		isOffScreen = true;
	}

}