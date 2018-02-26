using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour {

	public GameObject player;
	public GameObject board;
	public bool in_mini_game = false;


	private Block current_block;
	private float[,] grid;

	private float speed = 20.0f;
	public Vector3 pos;
	public Vector3 pos2;
	private Transform tr;
	private float size = 7.0f;

	private float upper_bound = 156.0f;
	private float lower_bound = 93.53827f;
	private float left_bound = -4.298332f;
	private float right_bound = 58.7f;

	// Use this for initialization
	void Start () {
		pos = player.transform.position;
		pos2 = player.transform.position;
		tr = player.transform;
	}

	// Update is called once per frame
	void Update () {

		float horizontalMovement = Input.GetAxis ("Horizontal");
		Vector3 newScale = player.transform.localScale;
		if ((horizontalMovement < 0 && newScale.x > 0) || (horizontalMovement > 0 && newScale.x < 0)) {
			newScale.x *= -1;
			player.transform.localScale = newScale;
		}

		float verticalMovement = Input.GetAxis ("Vertical");

		if (horizontalMovement > 0 && pos.x < right_bound && tr.position == pos) {
			pos2 = pos;
			pos += Vector3.right * size;
		}
		else if (horizontalMovement < 0 && pos.x > left_bound && tr.position == pos) {
			pos2 = pos;
			pos += Vector3.left * size;
		}
		else if (verticalMovement > 0 && pos.y < upper_bound && tr.position == pos) {
			pos2 = pos;
			pos += Vector3.up * size;
		}
		else if (verticalMovement < 0 && pos.y > lower_bound && tr.position == pos) {
			pos2 = pos;
			pos += Vector3.down * size;
		}
			
		tr.position = Vector3.MoveTowards(tr.position, pos, Time.deltaTime * speed);
	}

	public void PrepareLevel(Block b) {

		// update hud
		HUDManager hud_manager = GameObject.Find("HUDManager").GetComponent<HUDManager> ();
		hud_manager.UpdateHUD (b);

		// setup board
		SpriteRenderer current = board.GetComponentInChildren<SpriteRenderer>();
		ExhibitGenerator eg = new ExhibitGenerator();
		current.sprite = eg.GenerateArt(b);

		board.transform.localScale = new Vector3 (700, 700, 10);

		// get grid
		grid = eg.GenerateGrid(b);

		PrintGrid (grid);

		in_mini_game = true;
	}


	public void SetCurrentBlock(Block b) { current_block = b; }

	void PrintGrid(float[,] grid) {
		string g = "";
		for(int r = 0; r < 10; r++) {
			string row = "";
			
			for(int c = 0; c < 10; c++) {
				row += grid[c,r];
				row += " ";
			}
			
			row += "\n";
			g += row;
					

		}

		Debug.Log (g);
	}

}
