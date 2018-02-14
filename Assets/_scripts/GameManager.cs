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
	public GameObject ml;
	public bool in_mini_game = false;
	public Vector3 last_camera_position;


	// privates
	// bool isGrounded = false; 
	// int jumpCount = 0;
	Rigidbody2D sprite;


	void Start() {
		sprite = player.transform.GetComponent<Rigidbody2D> ();
		Debug.Log ("********* " + ml);
	}
		
	void FixedUpdate() {

		if (!in_mini_game) {
			// if the sprite is at the boundary, move the camera with the sprite
			if (sprite.position.x > (leftBound + camera.transform.position.x)) {
				camera.transform.position = new Vector3 (sprite.position.x + rightBound, 0.0f, zBound);
				Debug.Log ("********* " + ml);
			} else if (sprite.position.x < (rightBound + camera.transform.position.x)) {
				camera.transform.position = new Vector3 (sprite.position.x + leftBound, 0.0f, zBound);
				enter_mini_game (5);
			}

			// clamp the sprite's position to the boundary triggers
			sprite.position = new Vector3 (
				Mathf.Clamp (sprite.position.x, rightBound + camera.transform.position.x, leftBound + camera.transform.position.x), 
				sprite.position.y, 
				0.0f
			);

		} else {
			
			// in mini game ...
			if (Input.GetKeyDown("a")) {
				exit_mini_game ();
			}
		}
	}

	public void enter_mini_game(int height) {

		// save the current camera position to come back to later
		last_camera_position = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z);

		Debug.Log (ml);
		Debug.Log ("********* " + ml);
		//MazeLoader ml = maze_loader.GetComponent<MazeLoader> ();
		//ml.generate(1);

		in_mini_game = true;

		camera.transform.position = new Vector3 (0.0f, 200.0f, -10.0f);
	}

	public void exit_mini_game() {
		in_mini_game = false;
		camera.transform.position = last_camera_position;
	}
}