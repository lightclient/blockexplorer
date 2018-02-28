using UnityEngine;

public class MiniGameMoveLogic : MonoBehaviour {

	public GameObject mini_game_manager_object;
	private MiniGameManager mini_game_manager;

	public Vector3 pos;
	public Vector3 pos2;

	private float speed = 20.0f;
	private float size = 7.0f;

	private float upper_bound = 156.0f;
	private float lower_bound = 94.0f;
	private float left_bound = -4.298332f;
	private float right_bound = 52.0f;

	void Start() {
		pos = transform.position;
		pos2 = transform.position;
		mini_game_manager = mini_game_manager_object.GetComponent<MiniGameManager> ();
	}

	void Update() {

		if (mini_game_manager.in_mini_game) {
			
			float horizontalMovement = Input.GetAxis ("Horizontal");
			Vector3 newScale = transform.localScale;
			if ((horizontalMovement < 0 && newScale.x > 0) || (horizontalMovement > 0 && newScale.x < 0)) {
				newScale.x *= -1;
				transform.localScale = newScale;
			}

			float verticalMovement = Input.GetAxis ("Vertical");

			if(horizontalMovement == 0 && verticalMovement == 0) {
				pos2 = pos;
			}

			if (horizontalMovement > 0 && pos.x < right_bound && transform.position == pos) {
				pos2 = pos;
				pos += Vector3.right * size;
			}
			else if (horizontalMovement < 0 && pos.x > left_bound && transform.position == pos) {
				pos2 = pos;
				pos += Vector3.left * size;
			}
			else if (verticalMovement > 0 && pos.y < upper_bound && transform.position == pos) {
				pos2 = pos;
				pos += Vector3.up * size;
			}
			else if (verticalMovement < 0 && pos.y > lower_bound && transform.position == pos) {
				pos2 = pos;
				pos += Vector3.down * size;
			}

			transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);

		}
	}

	// collision detection 
//	void  OnCollisionEnter2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = true; jumpCount = 0; } }
//	void  OnCollisionStay2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = true; jumpCount = 0; } }
//	void  OnCollisionExit2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = false;} }
}

