using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour {

	public GameObject player;
	public GameObject board;

	public GameObject game_manager_object;
	public GameManager game_manager;

	public bool in_mini_game = false;

	public GameObject coin;
	private GameObject coin_holder;

	public GameObject enemy;
	private GameObject enemy_holder;

	private Block current_block;
	private float[,] grid;

	private float speed = 20.0f;
	public Vector3 pos;
	public Vector3 pos2;
	private Transform tr;
	private float size = 7.0f;

	private float upper_bound = 156.0f;
	private float lower_bound = 94.0f;
	private float left_bound = -4.298332f;
	private float right_bound = 52.0f;

	// Use this for initialization
	void Start () {
		pos = player.transform.position;
		pos2 = player.transform.position;
		tr = player.transform;

		game_manager = game_manager_object.GetComponent<GameManager> ();

		coin.transform.position = new Vector3 (250.0f, 250.0f);
		coin_holder = GameObject.Find("coin_holder");

		enemy.SetActive (false);
		enemy.transform.position = new Vector3 (250.0f, 250.0f);
		enemy_holder = GameObject.Find("enemy_holder");
	}

	// Update is called once per frame
	void Update () {
		if (in_mini_game && coin_holder.transform.childCount <= 0) {
			game_manager.exit_mini_game (true);
		}
	}

	public int height() {
		return current_block.height;
	}

	public void PrepareLevel(Block b) {

		current_block = b;

		// update hud
		MiniGameHUDManager hud_manager = GameObject.Find("MiniGameHUDManager").GetComponent<MiniGameHUDManager> ();
		hud_manager.UpdateHUD (b);

		// setup board
		SpriteRenderer current = board.GetComponentInChildren<SpriteRenderer>();
		ExhibitGenerator eg = new ExhibitGenerator();
		current.sprite = eg.GenerateArt(b);

		board.transform.localScale = new Vector3 (700, 700, 10);

		// debug
		// grid = eg.GenerateGrid(b);
		// PrintGrid (grid);

		// place coins
		Random.InitState (b.height);

		List<float[]> points = new List<float[]> ();
		for (int i = 0; i < 3; i++) {
			GameObject new_coin = Instantiate(coin, new Vector3(0.0f, 0.0f), Quaternion.identity);
			new_coin.transform.localScale = new Vector3 (15.0f, 15.0f);
			new_coin.transform.parent = coin_holder.transform;

			float row = 0.0f;
			float col = 0.0f;

			do {
				row = Mathf.Floor (Random.Range (0.0f, 10.0f));
				col = Mathf.Floor (Random.Range (0.0f, 10.0f));
			} while (isCoinPlacementCollision (row, col, points));

			points.Add( new float[] {col, row} );

			new_coin.transform.localPosition = new Vector3 ((float)(col * size), (float)(row * size), 0.0f);
		}
			
		for (int i = 0; i < 3; i++) {
			GameObject new_enemy = Instantiate(enemy, new Vector3(0.0f, 0.0f), Quaternion.identity);
			new_enemy.transform.localScale = new Vector3 (15.0f, 15.0f);
			new_enemy.transform.parent = enemy_holder.transform;

			float row = 0.0f;
			float col = 0.0f;

			do {
				row = Mathf.Floor (Random.Range (0.0f, 10.0f));
				col = Mathf.Floor (Random.Range (0.0f, 10.0f));
			} while (isCoinPlacementCollision (row, col, points));

			points.Add( new float[] {col, row} );

			new_enemy.SetActive(true);
			new_enemy.transform.localPosition = new Vector3 ((float)(col * size), (float)(row * size), 0.0f);
		}

		in_mini_game = true;
	}

	public void CleanUpLevel() {
		foreach (Transform child in coin_holder.transform) {
			GameObject.Destroy(child.gameObject);
		}

		foreach (Transform child in enemy_holder.transform) {
			GameObject.Destroy(child.gameObject);
		}

		in_mini_game = false;
	}

	public void SetCurrentBlock(Block b) { current_block = b; }

	private bool isCoinPlacementCollision(float row, float col, List<float[]> points) {
		foreach (var point in points) {
			if(point[0] == col && point[1] == row) {
				Debug.Log ("COLLISION");
				return true;
			}
		}

		return false;
	}

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
