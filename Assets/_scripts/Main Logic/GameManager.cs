using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject camera;
	public GameObject player;
	public GameObject hud;

	private MiniGameManager minigame_manager;

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

		minigame_manager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager> ();

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

			// reset completed levels
			if (Input.GetKeyDown (KeyCode.R)) {
				PlayerPrefs.DeleteAll ();
			}

		} else {
			
			// in mini game ...
			if (Input.GetKeyDown(KeyCode.Q)) {
				exit_mini_game (false);
			}
		}
	}

	public void enter_mini_game(Block b) {

		// disable hud
		Canvas canvas = hud.GetComponentInChildren<Canvas>();
		canvas.enabled = false;

		// save the current camera position to come back to later
		last_camera_position = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z);
		last_player_position = sprite.transform.position;

		minigame_manager.PrepareLevel (b);

		in_mini_game = true;
		camera.transform.position = new Vector3 (0.0f, 125.0f, -10.0f);

		sprite.transform.position = new Vector3 (-50.63f, 229.9f);
		sprite.transform.localScale = new Vector3 (2.5f, 2.5f, 2.5f);
		sprite.gravityScale = 0.0f;

		Debug.Log(PlayerPrefs.HasKey(b.height.ToString()));
	}

	public void exit_mini_game(bool completed) {
		
		if (completed) {

			// increase total levels completed, only if level hasn't been completed before
			if (PlayerPrefs.GetInt (minigame_manager.height ().ToString (), 0) == 0) {
				PlayerPrefs.SetInt("completed_levels", PlayerPrefs.GetInt("completed_levels",0) + 1);
			}

			// set level as completed
			PlayerPrefs.SetInt (minigame_manager.height ().ToString(), 1);



			GameObject exhibits_holder = GameObject.Find("Exhibits");
			foreach (Transform child in exhibits_holder.transform) {
				
				BlockManager block_manager = child.GetComponent<BlockManager> ();

				if (block_manager.height == minigame_manager.height ()) {
					block_manager.completeBlock ();
				}

			}
		}

		minigame_manager.CleanUpLevel ();

		// enable hud
		Canvas canvas = hud.GetComponentInChildren<Canvas>();
		canvas.enabled = true;

		// reset camera & sprite position
		camera.transform.position = last_camera_position;
		sprite.transform.position = last_player_position;
		sprite.transform.localScale = new Vector3 (60.0f, 60.0f, 60.0f);
		sprite.gravityScale = 50.0f;
		//camera.transform.position = new Vector3 (0.0f, 0.0f, -10.0f);

		minigame_manager.in_mini_game = false;
		in_mini_game = false;
	}
}