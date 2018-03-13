using UnityEngine;

public class MiniGameMoveLogic : MonoBehaviour {

	public GameObject mini_game_manager_object;
	private MiniGameManager mgm;

	public GameObject freeze_fire;
	public GameObject current_freeze_fire;

	public Vector3 pos;

	private float speed = 20.0f;
	private float size = 7.0f;

	private Animator animator;

	enum Direction { Up, Down, Left, Right };
	private Direction current_direction;

	void Start() {
		mgm = mini_game_manager_object.GetComponent<MiniGameManager> ();
		animator = gameObject.GetComponent<Animator>();

		animator.enabled = false;
		pos = new Vector3 (1000, 1000);
	}

	void Update() {

		if (mgm.in_mini_game) {

			// move player toward next position
			transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);

			// user input
			float horizontalMovement = Input.GetAxis ("Horizontal");
			float verticalMovement = Input.GetAxis ("Vertical");

			// if this is the first user input, begin the game and the player animations
			if (!mgm.playing && ( horizontalMovement != 0 || verticalMovement != 0) ) {
				mgm.playing = true;
				animator.enabled = true;
			}

			// if the mini game is currently being played, run through the logic
			if (mgm.playing) {

				// freeze blast logic
				if (Input.GetKeyDown(KeyCode.Space)) {
					if (PlayerPrefs.GetInt ("freeze_blasts", PlayerPrefs.GetInt("freeze",0) + 1) > 0) {
						PlayerPrefs.SetInt ("freeze_blasts", PlayerPrefs.GetInt ("freeze_blasts", PlayerPrefs.GetInt("freeze",0) + 1) - 1);
						FreezeEnemies (transform.position, 5);
					}
				}

				// determine which direction the user has input
				if (horizontalMovement > 0) {
					current_direction = Direction.Right;
				} else if (horizontalMovement < 0) {
					current_direction = Direction.Left;
				} else if (verticalMovement > 0) {
					current_direction = Direction.Up;
				} else if (verticalMovement < 0) {
					current_direction = Direction.Down;
				}

				// set the position using the new direction and update the animation
				Vector3 newScale = transform.localScale;
				if (current_direction == Direction.Right && pos.x < mgm.right_bound && transform.position == pos) {
					pos += Vector3.right * size;

					//animator.SetTrigger ("side");
					animator.SetInteger ("direction", 3);
					newScale.x = Mathf.Abs (newScale.x) * -1;
					transform.localScale = newScale;
				} else if (current_direction == Direction.Left && pos.x > mgm.left_bound && transform.position == pos) {
					pos += Vector3.left * size;

					//animator.SetTrigger ("side");
					animator.SetInteger ("direction", 3);
					newScale.x = Mathf.Abs (newScale.x);
					transform.localScale = newScale;
				} else if (current_direction == Direction.Up && pos.y < mgm.upper_bound && transform.position == pos) {
					pos += Vector3.up * size;
					//animator.SetTrigger ("up");
					animator.SetInteger ("direction", 1);
				} else if (current_direction == Direction.Down && pos.y > mgm.lower_bound && transform.position == pos) {
					pos += Vector3.down * size;
					//animator.SetTrigger ("down");
					animator.SetInteger ("direction", 2);
				}

				// make sure the freeze blast follows us
				if (current_freeze_fire != null) {
					current_freeze_fire.transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
				}
			}
		}


	}

	void FreezeEnemies(Vector3 location, float radius)
	{
		// create new freeze blast and set its location on top of the player
		current_freeze_fire = (GameObject)Instantiate (freeze_fire);
		current_freeze_fire.transform.position = transform.position;
	}
}

