using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject camera;
	public GameObject player;

	// boundary triggers
	public float rightBound;
	public float leftBound;
	private float zBound = -10.0f;

	public float jumpPower;
	public float speed;

	// controls to enter mini game
	public bool in_mini_game;
	public Vector3 last_camera_position;
	public Vector3 last_player_position;

	// privates
	// bool isGrounded = false; 
	// int jumpCount = 0;
	private Rigidbody2D sprite;


	void Start() {
		sprite = player.transform.GetComponent<Rigidbody2D> ();
		in_mini_game = false;
	}
		
	void FixedUpdate() {
		if (!in_mini_game) {
			// if the sprite is at the boundary, move the camera with the sprite
			if (sprite.position.x > (leftBound + camera.transform.position.x)) {
				camera.transform.position = new Vector3 (sprite.position.x + rightBound, 0.0f, zBound);
			} else if (sprite.position.x < (rightBound + camera.transform.position.x)) {
				camera.transform.position = new Vector3 (sprite.position.x + leftBound, 0.0f, zBound);
			}

			// clamp the sprite's position to the boundary triggers
			sprite.position = new Vector3 (
				Mathf.Clamp (sprite.position.x, rightBound + camera.transform.position.x, leftBound + camera.transform.position.x), 
				sprite.position.y, 
				0.0f
			);

		} else {
			
			// in mini game ...
			if (Input.GetKeyDown(KeyCode.Q)) {
				exit_mini_game ();
			}
		}
	}

	public void enter_mini_game(Block b) {

		// save the current camera position to come back to later
		last_camera_position = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z);
		last_player_position = sprite.transform.position;

		MiniGameManager minigame_manager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager> ();

		minigame_manager.PrepareLevel (b);

		in_mini_game = true;
		camera.transform.position = new Vector3 (0.0f, 125.0f, -10.0f);

		sprite.transform.position = new Vector3 (-50.63f, 229.9f);
		sprite.transform.localScale = new Vector3 (2.5f, 2.5f, 2.5f);
		sprite.gravityScale = 0.0f;
	}

	public void exit_mini_game() {
		MiniGameManager minigame_manager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager> ();
		minigame_manager.in_mini_game = false;

		in_mini_game = false;
		camera.transform.position = last_camera_position;
		sprite.transform.position = last_player_position;
		sprite.transform.localScale = new Vector3 (3.0f, 3.0f, 3.0f);
		sprite.gravityScale = 50.0f;
		//camera.transform.position = new Vector3 (0.0f, 0.0f, -10.0f);
	}
}