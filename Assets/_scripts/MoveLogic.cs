using UnityEngine;

public class MoveLogic : MonoBehaviour {

	public GameObject camera;
	public GameObject player;

	// boundary triggers
	public float rightBound;
	public float leftBound;
	private float zBound = -10.0f;

	public float jumpPower;
	public float speed;

	// privates
	bool isGrounded = false; 
	int jumpCount = 0;
	Rigidbody2D sprite;


	void Start() {
		sprite = transform.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate() {


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


	// collision detection 
	void  OnCollisionEnter2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = true; jumpCount = 0; } }
	void  OnCollisionStay2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = true; jumpCount = 0; } }
	void  OnCollisionExit2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = false;} }
}

