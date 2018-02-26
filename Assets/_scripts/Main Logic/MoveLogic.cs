using UnityEngine;

public class MoveLogic : MonoBehaviour {

	public GameObject game_manager_object;
	private GameManager game_manager;

	// boundary triggers
	public float rightBound;
	public float leftBound;

	public float jumpPower;
	public float speed;
	public float maze_speed;

	// privates
	private bool isGrounded = false; 
	private int jumpCount = 0;
	private Rigidbody2D sprite;
	private Vector3 last_position;


	void Start() {
		sprite = transform.GetComponent<Rigidbody2D> ();
		game_manager = game_manager_object.GetComponent<GameManager> ();
	}

	void FixedUpdate() {

		if (!game_manager.in_mini_game) {
			// jump logic
			if (Input.GetKey (KeyCode.Space) && jumpCount < 2) {
				sprite.AddForce (Vector3.up * (jumpPower * sprite.mass * sprite.gravityScale * 20.0f));
				jumpCount += 1;
			}

			// flip sprite depending on the direction they're moving
			float horizontalMovement = Input.GetAxis ("Horizontal");
			Vector3 newScale = sprite.transform.localScale;
			if ((horizontalMovement < 0 && newScale.x > 0) || (horizontalMovement > 0 && newScale.x < 0)) {
				newScale.x *= -1;
				transform.localScale = newScale;
			}

			// add lateral velocity to the sprite
			Vector3 movement = new Vector3 (horizontalMovement * speed, sprite.velocity.y, 0.0f);
			sprite.velocity = movement;

		}
	}

	// collision detection 
	void  OnCollisionEnter2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = true; jumpCount = 0; } }
	void  OnCollisionStay2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = true; jumpCount = 0; } }
	void  OnCollisionExit2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = false;} }
}

