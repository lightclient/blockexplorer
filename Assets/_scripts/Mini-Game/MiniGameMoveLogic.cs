using UnityEngine;

public class MiniGameMoveLogic : MonoBehaviour {

	public GameObject mini_game_manager_object;
	private MiniGameManager mini_game_manager;

	public GameObject freeze_fire;
	public GameObject current_freeze_fire;

	public Vector3 pos;

	private float speed = 20.0f;
	private float size = 7.0f;

	private float upper_bound = 156.0f;
	private float lower_bound = 94.0f;
	private float left_bound = -4.298332f;
	private float right_bound = 52.0f;

	private Animator animator;

	enum Direction { Up, Down, Left, Right };
	private Direction current_direction;

	void Start() {
		pos = transform.position;

		mini_game_manager = mini_game_manager_object.GetComponent<MiniGameManager> ();
		animator = gameObject.GetComponent<Animator>();
	}

	void Update() {

		if (mini_game_manager.in_mini_game) {


			if (Input.GetKeyDown(KeyCode.Space)) {
				FreezeEnemies (transform.position, 5);
			}

			float horizontalMovement = Input.GetAxis ("Horizontal");
			Vector3 newScale = transform.localScale;

			float verticalMovement = Input.GetAxis ("Vertical");


			if (horizontalMovement > 0) {
				current_direction = Direction.Right;
			}
			else if (horizontalMovement < 0) {
				current_direction = Direction.Left;
			}
			else if (verticalMovement > 0) {
				current_direction = Direction.Up;
			}
			else if (verticalMovement < 0) {
				current_direction = Direction.Down;
			}


			if (current_direction == Direction.Right && pos.x < right_bound && transform.position == pos) {
				pos += Vector3.right * size;

				animator.SetTrigger ("side");
				newScale.x = Mathf.Abs(newScale.x) * -1;
				transform.localScale = newScale;
			}
			else if (current_direction == Direction.Left && pos.x > left_bound && transform.position == pos) {
				pos += Vector3.left * size;

				animator.SetTrigger ("side");
				newScale.x = Mathf.Abs(newScale.x);
				transform.localScale = newScale;
			}
			else if (current_direction == Direction.Up && pos.y < upper_bound && transform.position == pos) {
				pos += Vector3.up * size;
				animator.SetTrigger ("up");
			}
			else if (current_direction == Direction.Down && pos.y > lower_bound && transform.position == pos) {
				pos += Vector3.down * size;
				animator.SetTrigger ("down");
			}

			transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);

			if (current_freeze_fire != null) {
				current_freeze_fire.transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
			}

		}
	}

	void FreezeEnemies(Vector3 location, float radius)
	{
		current_freeze_fire = (GameObject)Instantiate (freeze_fire);
		current_freeze_fire.transform.position = transform.position;
		Debug.Log ("play explosion");

//		Debug.Log (location);
//		UnityEngine.Collider2D[] objectsInRange = Physics2D.OverlapCircleAll (new Vector2 (location.x, location.y), radius);
//		foreach (UnityEngine.Collider2D col in objectsInRange)
//		{
//			Debug.Log ("contact");
//			EnemyLogic enemy = col.GetComponent<EnemyLogic>();
//
//			if (enemy != null)
//			{
//				// linear falloff of effect
//				float proximity = (location - enemy.transform.position).magnitude;
//				float effect = 1 - (proximity / radius);
//
//				enemy.freeze(5);
//			}
//		}

	}

	// collision detection 
	//	void  OnCollisionEnter2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = true; jumpCount = 0; } }
	//	void  OnCollisionStay2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = true; jumpCount = 0; } }
	//	void  OnCollisionExit2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = false;} }
}

