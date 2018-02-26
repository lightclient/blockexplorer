using UnityEngine;

public class MiniGameMoveLogic : MonoBehaviour {

	public GameObject mini_game_manager_object;
	private MiniGameManager mini_game_manager;

	void Start() {
		mini_game_manager = mini_game_manager_object.GetComponent<MiniGameManager> ();
	}

	void Update() {

		if (mini_game_manager.in_mini_game) {
			


		}
	}

	// collision detection 
//	void  OnCollisionEnter2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = true; jumpCount = 0; } }
//	void  OnCollisionStay2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = true; jumpCount = 0; } }
//	void  OnCollisionExit2D (Collision2D other) { if (other.collider.tag == "Ground") { isGrounded = false;} }
}

