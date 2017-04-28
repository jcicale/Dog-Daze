using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TypeWriterEffect : MonoBehaviour {

	public float letterPause = 0.01f;
	private string name;

	string message;
	Text textComp;

	// Use this for initialization
	void Start () {
		textComp = GetComponent<Text>();
		message = textComp.text;
		textComp.text = "";
		StartCoroutine(TypeText ());

		name = SceneManager.GetActiveScene ().name;
	}

	IEnumerator TypeText () {
		foreach (char letter in message.ToCharArray()) {
			textComp.text += letter;
			yield return 0;
			if (name == "TitleText" || name == "Level1Text") {
				yield return new WaitForSecondsRealtime (letterPause);
			}
		}
	}
}
