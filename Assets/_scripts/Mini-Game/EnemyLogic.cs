using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour {

	public GameObject mini_game_manager_object;
	private MiniGameManager mini_game_manager;

	public Vector3 pos;

	public float wander_level;   // likelihood of changing directions
	public float savage_level; // likelihood of changing directions toward player

	public bool frozen = false;

	private Animator animator;

	enum Direction { Up, Down, Left, Right };
	private Direction current_direction;

	public float upper_bound = 156.0f;
	public float lower_bound = 94.0f;
	public float left_bound = -4.298332f;
	public float right_bound = 52.0f;

	public float speed = 20.0f;

	private float size = 7.0f;

	public AudioSource audio;
	public AudioClip hit;

	// Use this for initialization
	void Start () {
		pos = transform.position;
		current_direction = Direction.Down;

		mini_game_manager = mini_game_manager_object.GetComponent<MiniGameManager> ();
		animator = gameObject.GetComponent<Animator>();

		animator.enabled = false;

		audio.clip = hit;
	}
	
	// Update is called once per frame
	void Update () {

		if (!frozen && mini_game_manager.playing) {

			// after enemies start moving, their animations should begin looping
			if (!animator.enabled) {
				animator.enabled = true;
			}

			// determine if the enemy will change course this block
			if (transform.position == pos && Random.Range (0.0f, 100.0f) < wander_level) {

				// determine if the enemy will chase after the player
				if (Random.Range (0.0f, 100.0f) < savage_level) {
					MoveTowardPlayer ();
				} else {
					ChooseNewDirection ();
				}
			}

			// choose new direction if enemy hit a wall
			if(	( current_direction == Direction.Right && pos.x >= right_bound ) || 
				( current_direction == Direction.Left  && pos.x <= left_bound  ) || 
				( current_direction == Direction.Down  && pos.y <= lower_bound ) ||
				( current_direction == Direction.Up    && pos.y >= upper_bound ) ) {

				ChooseNewDirection ();

			}

			Vector3 newScale = transform.localScale;

			// set new position vector , animation based on new direction
			if (current_direction == Direction.Right && pos.x < right_bound && transform.position == pos) {
				pos += Vector3.right * size;

				animator.SetInteger ("direction", 3);
				newScale.x = Mathf.Abs (newScale.x) * -1;
				transform.localScale = newScale;

			} else if (current_direction == Direction.Left && pos.x > left_bound && transform.position == pos) {
				pos += Vector3.left * size;

				animator.SetInteger ("direction", 3);
				newScale.x = Mathf.Abs (newScale.x);
				transform.localScale = newScale;

			} else if (current_direction == Direction.Up && pos.y < upper_bound && transform.position == pos) {
				pos += Vector3.up * size;
				animator.SetInteger ("direction", 1);
			} else if (current_direction == Direction.Down && pos.y > lower_bound && transform.position == pos) {
				pos += Vector3.down * size;
				animator.SetInteger ("direction", 2);
			}
				
			// move towards the new position target
			transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
		}

	}


	// public method to call freezer coroutine
	public void freeze(int waitTime) {
		StartCoroutine(_freeze(waitTime));
	}

	// freezer coroutine ... stops enemy from moving
	private IEnumerator _freeze(int waitTime)
	{
		frozen = true;
		animator.enabled = false;
		yield return new WaitForSeconds(waitTime);
		frozen = false;
	}

	// determine the optimal direction towards the player
	void MoveTowardPlayer() {
		float x = Mathf.Abs (mini_game_manager.player.transform.position.x - transform.position.x);
		float y = Mathf.Abs (mini_game_manager.player.transform.position.y - transform.position.y);

		if (y < x) {
			current_direction = (transform.position.x < mini_game_manager.player.transform.position.x) ? Direction.Right : Direction.Left;
		} else {
			current_direction = (transform.position.y < mini_game_manager.player.transform.position.y) ? Direction.Up : Direction.Down;
		}
	}

	// choose a random new direction, but don't go in the opposite direction enemy is currently moving
	void ChooseNewDirection() {

		Direction new_dir;
		do {
			new_dir = (Direction)(int)Mathf.Floor (Random.Range (0.0f, 4.0f));
		} while ( new_dir == current_direction || new_dir == opposite(current_direction) );

			
		current_direction = new_dir;
	}

	// determine what the opposite of a given direction is
	Direction opposite(Direction dir) {

		if (dir == Direction.Right) {
			return Direction.Left;
		} else if (dir == Direction.Left) {
			return Direction.Right;
		} else if (dir == Direction.Up) {
			return Direction.Down;
		} else if (dir == Direction.Down) {
			return Direction.Up;
		}

		// oops
		return Direction.Down;
	}

	// collision logic
	void OnTriggerEnter2D(Collider2D other) {

		// determine what collided with the enemy
		if (other.tag == "Player" && mini_game_manager.playing) {
			// player collides with enemy

			audio.Play ();

			if (PlayerPrefs.GetInt ("hits", 0) < (PlayerPrefs.GetInt ("armor", 0) + 1) && mini_game_manager.in_mini_game) {
				// add hit
				PlayerPrefs.SetInt ("hits", PlayerPrefs.GetInt ("hits", 0) + 1);
			} else if (mini_game_manager.in_mini_game) {
				// die
				PlayerPrefs.SetInt ("hits", PlayerPrefs.GetInt ("hits", 0) + 1);
				mini_game_manager.ExitGame (false);
			}

		
		
		} else if (other.tag == "Enemy") {
			// enemy collides with another enemy



		
		} else if (other.tag == "Freeze") {
			// enemy collides with freeze blast

			freeze (5);
		}

	}

}
