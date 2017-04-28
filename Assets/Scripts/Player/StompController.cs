using UnityEngine;
using System.Collections;



public class StompController : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D coll)
    {
		if (coll.gameObject.tag == "Enemy" && !GameObject.Find("Winks").GetComponent<PlayerController>().isHurt)
        {
			string name = coll.gameObject.transform.name.ToString();
			if (name == "Squirrel1" || name == "Squirrel2") {
				coll.gameObject.GetComponentsInParent<AudioSource> () [0].Play ();
			} else if (name == "Elephant1" || name == "Elephant2") {
				coll.gameObject.GetComponentsInParent<AudioSource> () [1].Play ();
			} else if (name == "Bird1") {
				coll.gameObject.GetComponentsInParent<AudioSource> () [2].Play ();
			}

			GameObject.Destroy(coll.gameObject);


            Rigidbody2D winks = GameObject.Find("Winks").GetComponent<Rigidbody2D>();
            winks.velocity = new Vector2(winks.velocity.x, 0);
            winks.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

        }

		if (coll.gameObject.tag == "Car" && !GameObject.Find ("Winks").GetComponent<PlayerController> ().isHurt) {
			coll.gameObject.GetComponentsInParent<AudioSource> ()[3].Play ();

			Rigidbody2D winks = GameObject.Find("Winks").GetComponent<Rigidbody2D>();
			winks.velocity = new Vector2(winks.velocity.x, 0);
			winks.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
		}
    }
 
}