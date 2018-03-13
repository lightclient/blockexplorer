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

	private bool gameover = false;
	private Vector3 gameover_pos;
	public GameObject gameover_canvas;


	void Start() {
		sprite = player.transform.GetComponent<Rigidbody2D> ();

		minigame_manager = GameObject.Find("MiniGameManager").GetComponent<MiniGameManager> ();

		in_mini_game = false;
	}

	void Update() {
		if (!gameover && !in_mini_game) {
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

			// hackz
			if (Input.GetKeyDown (KeyCode.C)) {
				PlayerPrefs.SetInt ("coins_collected", 100000);
			}

		} else if(!gameover && in_mini_game) {

			// in mini game ...
			if (Input.GetKeyDown(KeyCode.Q)) {
				exit_mini_game (false);
			}
		}
	}

	void FixedUpdate() {

	}

	public void enter_mini_game(Block b) {

		in_mini_game = true;

		// disable hud
		Canvas canvas = hud.GetComponentInChildren<Canvas>();
		canvas.enabled = false;

		// save the current camera position to come back to later
		last_camera_position = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z);
		last_player_position = sprite.transform.position;

		camera.transform.position = new Vector3 (0.0f, 125.0f, -10.0f);

		minigame_manager.PrepareLevel (b);

		Debug.Log(PlayerPrefs.HasKey(b.height.ToString()));
	}

	public void exit_mini_game(bool completed) {

		minigame_manager.in_mini_game = false;
		in_mini_game = false;

		// reset hits
		PlayerPrefs.SetInt ("hits", 0);

		// reset allowed freeze blasts
		PlayerPrefs.SetInt ("freeze_blasts", 1 + PlayerPrefs.GetInt ("freeze", 0));

		if (completed) {

			// increase total levels completed, only if level hasn't been completed before
			if (PlayerPrefs.GetInt (minigame_manager.height ().ToString (), 0) == 0) {
				PlayerPrefs.SetInt ("completed_levels", PlayerPrefs.GetInt ("completed_levels", 0) + 1);
			}

			// set level as completed
			PlayerPrefs.SetInt (minigame_manager.height ().ToString (), 1);


			// find exhibit and set it as complete so it will be updated when we return to the main game
			GameObject exhibits_holder = GameObject.Find ("Exhibits");
			foreach (Transform child in exhibits_holder.transform) {
				
				BlockManager block_manager = child.GetComponent<BlockManager> ();

				if (block_manager.height == minigame_manager.height ()) {
					block_manager.completeBlock ();
				}

			}
		} else {
			// decrease number of lives

			// see if player has any lives left
			if (PlayerPrefs.GetInt ("lives", 3) == 0) {
				// game over
				gameover = true;


				minigame_manager.playing = false;

				// freeze player
				minigame_manager.player.GetComponent<Animator> ().enabled = false;

				// freeze coins
				foreach (Transform child in minigame_manager.coin_holder.transform) {
					child.GetComponent<Animator> ().enabled = false;
				}

				// freeze enemies
				foreach (Transform child in minigame_manager.enemy_holder.transform) {
					child.GetComponent<Animator> ().enabled = false;
				}

				StartCoroutine (BeginGameOverSequence());


				return;

			} else {
				// has lives left so decrease count by 1
				PlayerPrefs.SetInt ("lives", PlayerPrefs.GetInt ("lives", 3) - 1);
			}
		}

		minigame_manager.CleanUpLevel ();

		// enable hud
		Canvas canvas = hud.GetComponentInChildren<Canvas>();
		canvas.enabled = true;

		// reset camera & sprite position
		camera.transform.position = last_camera_position;
		sprite.transform.position = last_player_position;

		sprite.gravityScale = 50.0f;
	}

	IEnumerator BeginGameOverSequence() {
		yield return new WaitForSeconds(1);

		minigame_manager.player.GetComponent<Rigidbody2D> ().gravityScale = 30.0f;

		yield return new WaitForSeconds (0.10f);

		gameover_canvas.SetActive (true);
		gameover_canvas.GetComponent<Animator> ().enabled = true;

		yield return new WaitForSeconds (3);

		minigame_manager.player.GetComponent<Rigidbody2D> ().gravityScale = 0.0f;
		minigame_manager.player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0.0f, 0.0f);

		gameover_canvas.GetComponent<Animator> ().SetInteger ("game_over_state", 1);
	}

	public void RestartGame() {

		PlayerPrefs.DeleteAll ();

		minigame_manager.CleanUpLevel ();

		// enable hud
		Canvas canvas = hud.GetComponentInChildren<Canvas>();
		canvas.enabled = true;

		// reset camera & sprite position
		gameover = false;
		gameover_canvas.SetActive (false);
		camera.transform.position = last_camera_position;
		sprite.transform.position = last_player_position;

		gameover_canvas.GetComponent<Animator> ().SetTrigger ("exit-game-over");
	}
}