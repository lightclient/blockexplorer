using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	public bool isStart, isQuit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Mouse is pressed down");

			RaycastHit2D hitInfo = Physics2D.Raycast (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, 0);

			if (hitInfo) {
				Debug.Log ("Object Hit is " + hitInfo.collider.gameObject.name);

				//If you want it to only detect some certain game object it hits, you can do that here
				if (hitInfo.collider.gameObject.CompareTag ("Dog")) {
					Debug.Log ("Dog hit");
					//do something to dog here
				} else if (hitInfo.collider.gameObject.CompareTag ("Cat")) {
					Debug.Log ("Cat hit");
					//do something to cat here
				}
			} 
		}
	}

	public void LoadScene() {
			
			StartCoroutine(AsyncLoadScene());

	}

	IEnumerator AsyncLoadScene()
	{
		// The Application loads the Scene in the background at the same time as the current Scene.
		//This is particularly good for creating loading screens. You could also load the Scene by build //number.
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("museum");

		//Wait until the last operation fully loads to return anything
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}
}
