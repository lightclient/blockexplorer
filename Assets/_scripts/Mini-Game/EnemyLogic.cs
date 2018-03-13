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

	private float speed = 20.0f;
	private float size = 7.0f;
	private float upper_bound = 156.0f;
	private float lower_bound = 94.0f;
	private float left_bound = -4.298332f;
	private float right_bound = 52.0f;

	// Use this for initialization
	void Start () {
		pos = transform.position;
		current_direction = Direction.Down;

		mini_game_manager = mini_game_manager_object.GetComponent<MiniGameManager> ();
		animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!frozen && mini_game_manager.playing) {
			if (transform.position == pos && Random.Range (0.0f, 100.0f) < wander_level) {

				if (Random.Range (0.0f, 100.0f) < savage_level) {
					MoveTowardPlayer ();
				} else {
					ChooseNewDirection ();
				}
			}

			// choose new direction if we hit a wall
			if(	( current_direction == Direction.Right && pos.x >= right_bound ) || 
				( current_direction == Direction.Left  && pos.x <= left_bound  ) || 
				( current_direction == Direction.Down  && pos.y <= lower_bound ) ||
				( current_direction == Direction.Up    && pos.y >= upper_bound ) ) {

				ChooseNewDirection ();

			}

			Vector3 newScale = transform.localScale;

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
				
			transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
		}

	}


	public void freeze(int waitTime) {
		StartCoroutine(_freeze(waitTime));
	}

	private IEnumerator _freeze(int waitTime)
	{
		frozen = true;
		yield return new WaitForSeconds(waitTime);
		frozen = false;
	}

	void MoveTowardPlayer() {
		float x = Mathf.Abs (mini_game_manager.player.transform.position.x - transform.position.x);
		float y = Mathf.Abs (mini_game_manager.player.transform.position.y - transform.position.y);

		if (y < x) {
			current_direction = (transform.position.x < mini_game_manager.player.transform.position.x) ? Direction.Right : Direction.Left;
		} else {
			current_direction = (transform.position.y < mini_game_manager.player.transform.position.y) ? Direction.Up : Direction.Down;
		}
	}

	void ChooseNewDirection() {

		Direction new_dir;
		do {
			new_dir = (Direction)(int)Mathf.Floor (Random.Range (0.0f, 4.0f));
		} while ( new_dir == current_direction || new_dir == opposite(current_direction) );

			
		current_direction = new_dir;
	}

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

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			// Debug.Log ("die bitch");

			// trying to avoid a double death
			if (mini_game_manager.in_mini_game) {
				mini_game_manager.ExitGame (false);
			}
		} else if (other.tag == "Enemy") {
			// Debug.Log ("encountered another enemy");
		} else if (other.tag == "Freeze") {
			Debug.Log ("gg mate");
			freeze (5);
		} else {
			Debug.Log ("da fuq dis : " + other.tag);
		}


	}

}
