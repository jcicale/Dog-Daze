using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public GameObject Winks;
	public Transform bone;

	private static int numberOfBones;
	private GameObject[] bonesToDestroy;
	private bool isDead;
	private bool gameWon;

	void Start () {
		isDead = false;
		gameWon = false;
		numberOfBones = 3;
		for (int i = 0; i < numberOfBones; i++) {
			Transform newBone = Instantiate(bone, GameObject.Find("BoneCount").transform, false);
			float temp = newBone.position.x;
			temp -= i / 3.0f - .5f;
			temp += i * .6f;
			newBone.position = new Vector3(temp, newBone.transform.position.y, newBone.transform.position.z);
		}
	}
	
	// Update is called once per frame
	void Update () {
		boneCheck ();
		deathCheck ();
		winCheck ();
	}

	private void boneCheck() {
		if (numberOfBones == PlayerController.numberOfBones) {
			return;
		} else {
			numberOfBones = PlayerController.numberOfBones;
			bonesToDestroy = GameObject.FindGameObjectsWithTag ("BoneCount");
			for (int i = 0; i < bonesToDestroy.Length; i++) {
				Destroy (bonesToDestroy[i]);
			}	

			for (int i = 0; i < numberOfBones; i++) {
				Transform newBone = Instantiate (bone, GameObject.Find ("BoneCount").transform, false);
				float temp = newBone.position.x;
				temp -= i / 3.0f - .5f;
				temp += i * .6f;
				newBone.position = new Vector3 (temp, newBone.transform.position.y, newBone.transform.position.z);
			}
		}
	}

	private void deathCheck() {
		if (numberOfBones == 0) {
			Winks.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, .5f);
			Winks.GetComponent<PlayerController> ().enabled = false;
			isDead = true;
		}
		if (PlayerController.isOffScreen) {
			isDead = true;
		}
		if (isDead) {
			GameOver ();
		}
	}

	private void winCheck() {
		if (PlayerController.inDoghouse) {
			gameWon = true;
		}
		if (gameWon) {
			Winks.GetComponent<PlayerController> ().enabled = false;
			GameWon ();
		}
	}

	public void GameOver(){
		StartCoroutine("Wait");
	}

	public void GameWon() {
		StartCoroutine("Wait2");
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds (2);

		SceneManager.LoadScene("GameOver");
	}

	IEnumerator Wait2() {
		yield return new WaitForSeconds (2);
		SceneManager.LoadScene ("WinScene");
	}
}
