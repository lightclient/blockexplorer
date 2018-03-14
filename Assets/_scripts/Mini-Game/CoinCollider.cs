using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollider : MonoBehaviour {

	public AudioSource source;
	public AudioClip coin;

	private bool alive = true;

	// Use this for initialization
	void Start () {
		source.clip = coin;
	}

	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" && alive) {
			alive = false;
			StartCoroutine (DestroyCoin());
		}
	}


	IEnumerator DestroyCoin(){
		source.Play ();
		gameObject.GetComponent<Renderer> ().enabled = false;
		PlayerPrefs.SetInt ("coins_collected", PlayerPrefs.GetInt ("coins_collected", 0) + 1);
		yield return new WaitWhile (()=> source.isPlaying);
		GameObject.Destroy (gameObject);
	}
}
