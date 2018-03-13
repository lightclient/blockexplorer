using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour {

	public GameManager game_manager;
	public bool gameCanStart;

	// Use this for initialization
	void Start () {
		game_manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" && Input.GetKeyDown ("space") && !game_manager.in_mini_game) {
			BlockManager b = GetComponent<BlockManager> ();

			game_manager.GetComponent<GameManager> ().enter_mini_game (b.block);
		}
	}
}

