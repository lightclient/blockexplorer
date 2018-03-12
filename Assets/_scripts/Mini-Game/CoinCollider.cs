using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			PlayerPrefs.SetInt ("coins_collected", PlayerPrefs.GetInt ("coins_collected", 0) + 1);
			GameObject.Destroy (gameObject);
		}
	}

}
