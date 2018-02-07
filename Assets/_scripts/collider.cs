using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider : MonoBehaviour {
	bool gameCanStart;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (gameCanStart && Input.GetKeyDown ("space")) {
			Debug.Log ("start her uppppp");
		}	
	}


	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "player")
		gameCanStart = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject.tag == "player")
			
		gameCanStart = false;
	}
		
}

