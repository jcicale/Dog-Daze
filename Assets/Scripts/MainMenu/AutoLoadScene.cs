using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AutoLoadScene : MonoBehaviour {

	public int sceneIndex;

	void Start() {
		ModeSelect ();
	}

	public void  ModeSelect(){
		StartCoroutine("Wait");
	}

	IEnumerator Wait()
	{
		if (sceneIndex == 0) {
			yield return new WaitForSeconds (5);
		} else if (sceneIndex == 2) {
			yield return new WaitForSeconds (10);
		} else if (sceneIndex == 3) {
			yield return new WaitForSeconds (10);
		} else if (sceneIndex == 4) {
			yield return new WaitForSeconds (10);
		} else if (sceneIndex == 5) {
			yield return new WaitForSeconds (6);
		} 
		SceneManager.LoadScene(sceneIndex);
	}
}
