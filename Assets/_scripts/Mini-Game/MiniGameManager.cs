using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour {

	public GameObject player;
	public GameObject board;

	public GameObject game_manager_object;
	public GameManager game_manager;
	private ExhibitGenerator exhibit_generator;

	public bool in_mini_game = false;

	public GameObject coin;
	public GameObject coin_holder;

	public GameObject enemy;
	public GameObject enemy_holder;

	private Block current_block;
	private float[,] grid;

	private float speed = 20.0f;
	public Vector3 pos;
	public Vector3 pos2;
	private Transform tr;
	private float size = 7.0f;

	public float upper_bound = 156.0f;
	public float lower_bound = 94.0f;
	public float left_bound = -4.298332f;
	public float right_bound = 52.0f;

	public bool playing = false;

	public AudioSource source;
	public AudioClip clip1;
	public AudioClip clip2;

	public GameObject hud_vitals;
	public GameObject hud_block;

	public GameObject heart;
	public GameObject bitcoin;
	public GameObject freeze_blast;
	public GameObject armor;
	public GameObject armor_slider;
	public GameObject armor_text;

	// Use this for initialization
	void Start () {
		game_manager = game_manager_object.GetComponent<GameManager> ();

		// move items off screen ( lol didn't know what prefabs were )
		coin.transform.position = new Vector3 (250.0f, 250.0f);
		coin_holder = GameObject.Find("coin_holder");

		enemy.SetActive (false);
		enemy.transform.position = new Vector3 (250.0f, 250.0f);
		enemy_holder = GameObject.Find("enemy_holder");
		//////////////////////////////////////////////////////////////
	}

	// Update is called once per frame
	void Update () {

		// exit minigame if player wins (i.e. collects all coins)
		if (in_mini_game && coin_holder.transform.childCount <= 0) {
			ExitGame (true);
		}
	}

	public void ExitGame(bool completed) {
		source.Stop ();
		game_manager.exit_mini_game (completed);
	}

	public int height() {
		return current_block.height;
	}

	public void PrepareLevel(Block b) {

		current_block = b;

		// get difficulty -- between 8 (easiest) and 16 (latest)
		int difficulty = GetLeadingZeros (b);

		// play audio
		if (difficulty < 12) {
			source.clip = clip1;
		} else {
			source.clip = clip2;
		}

		// place player
		Vector3 mPos = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, 0));
		mPos.x -= 10 * size - (size / 2);
		mPos.y += 10 * size - (size / 2);
		player.transform.position = new Vector3( mPos.x, mPos.y);

		GameObject.Find("player").GetComponent<MiniGameMoveLogic> ().pos = player.transform.position;

		// set boundaries
		Vector3 p = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, 0));

		lower_bound = p.y + size;
		right_bound = p.x - size;

		p.x -= 10 * size;
		p.y += 10 * size;

		upper_bound = p.y - size;
		left_bound = p.x + size;

		// update hud
		MiniGameHUDManager hud_manager = GameObject.Find("MiniGameHUDManager").GetComponent<MiniGameHUDManager> ();
		hud_manager.SetHUD (b);

		// setup board
		Image current = board.GetComponent<Image>();
		//SpriteRenderer current = board.GetComponentInChildren<SpriteRenderer>();
		ExhibitGenerator eg = new ExhibitGenerator();
		current.sprite = eg.GenerateArt(b);

		// place board for any aspect ratio
		Vector3 board_position = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, 0));
		board_position.y += 35;
		board.transform.position = new Vector3( board_position.x, board_position.y);

		float aspect = (float)Screen.width / (float)Screen.height;
		Debug.Log ("aspect: " + aspect);

		// 16:9 or wider
		if (aspect > 1.7f) {
			RectTransform vitals = hud_vitals.GetComponent<RectTransform> ();

			vitals.anchoredPosition = new Vector2 (-0.56f, -2.981705f);
			vitals.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 919.13f);
			vitals.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 258.28f);

			RectTransform block = hud_block.GetComponent<RectTransform> ();

			block.anchoredPosition = new Vector2 (1.98f, 0.0f);
			block.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 50.11f);
			block.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 42.43f);

		} else if (aspect <= 1.7f && aspect > 1.5f) {
			//around 16:10
			RectTransform rt = hud_vitals.GetComponent<RectTransform> ();

			rt.anchoredPosition = new Vector2 (-0.56f, -2.981705f);
			rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 771.7f);
			rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 250.78f);

			RectTransform block = hud_block.GetComponent<RectTransform> ();

			block.anchoredPosition = new Vector2 (3.98f, 0.0f);
			block.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 46.14f);
			block.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 40.59f);

		} else {
			// yikes
			bitcoin.SetActive(false);
			hud_block.SetActive (false);

			RectTransform rt;

			rt = hud_vitals.GetComponent<RectTransform> ();
			rt.anchoredPosition = new Vector2 (3.13f, -2.981705f);
			rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 771.7f);
			rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 250.78f);

			rt = heart.GetComponent<RectTransform> ();
			rt.anchoredPosition = new Vector2 (232.0f, 42.0f);
			rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 100.0f);
			rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 100.0f);

			rt = freeze_blast.GetComponent<RectTransform> ();
			rt.anchoredPosition = new Vector2 (142.0f, -29.8f);
			rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 100.0f);
			rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 100.0f);

			rt = armor.GetComponent<RectTransform> ();
			rt.anchoredPosition = new Vector2 (-439.5f, -73.55f);
			rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, 100.0f);
			rt.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, 100.0f);

			armor_slider.SetActive (false);
			armor_text.SetActive (true);


		}

		// debug
		// grid = eg.GenerateGrid(b);
		// PrintGrid (grid);

		// place coins
		Random.InitState (b.height);

		// list of taken points
		List<int[]> points = new List<int[]> ();

		// add player starting point
		points.Add( new int[] {9, 9} );
		points.Add( new int[] {8, 9} );
		points.Add( new int[] {7, 9} );

		int n_coins = (int)Mathf.Floor (Random.Range (difficulty / 4, difficulty / 2));
		for (int i = 0; i < n_coins; i++) {
			GameObject new_coin = Instantiate(coin, new Vector3(0.0f, 0.0f), Quaternion.identity);
			new_coin.transform.localScale = new Vector3 (15.0f, 15.0f);
			new_coin.transform.parent = coin_holder.transform;

			int row = 0;
			int col = 0;

			// find an unoccupied area of the board
			do {
				row = (int)Mathf.Floor (Random.Range (0.0f, 10.0f));
				col = (int)Mathf.Floor (Random.Range (0.0f, 10.0f));
			} while (isCoinPlacementCollision (row, col, points));

			points.Add( new int[] {col, row} );

			// set coin position
			Vector3 cpos = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, 0));
			cpos.x -= (col + 1) * size - (size / 2);
			cpos.y += (row + 1) * size - (size / 2);

			new_coin.transform.position = new Vector3( cpos.x, cpos.y );
		}
			
		// place enemies
		int n_enemies = (int)Mathf.Floor (Random.Range (difficulty / 4, difficulty / 3));
		for (int i = 0; i < n_enemies; i++) {
			GameObject new_enemy = Instantiate(enemy, new Vector3(0.0f, 0.0f), Quaternion.identity);
			new_enemy.transform.localScale = new Vector3 (15.0f, 15.0f);
			new_enemy.transform.parent = enemy_holder.transform;

			int row = 0;
			int col = 0;

			// find an unoccupied area of the board
			do {
				row = (int)Mathf.Floor (Random.Range (0.0f, 10.0f));
				col = (int)Mathf.Floor (Random.Range (0.0f, 10.0f));
			} while (isCoinPlacementCollision (row, col, points));

			Debug.Log ("row: " + row + " col: " + col);

			points.Add( new int[] {col, row} );

			// set enemy attributes
			new_enemy.SetActive(true);

			EnemyLogic el = new_enemy.GetComponent<EnemyLogic> ();
			el.upper_bound = upper_bound;
			el.lower_bound = lower_bound;
			el.right_bound = right_bound;
			el.left_bound = left_bound;
			el.speed = 20.0f + Mathf.Floor (Random.Range (0.0f, difficulty / 8.0f));

			// set how powerful the enemy is based on the difficulty of the block
			if (difficulty <= 8) {
				el.savage_level = Random.Range (0.0f, 20.0f);
				el.wander_level = Random.Range (10.0f, 50.0f);
			} else if (difficulty < 12) {
				el.savage_level = Random.Range (20.0f, 60.0f);
				el.wander_level = Random.Range (30.0f, 80.0f);
			} else if (difficulty < 16) {
				el.savage_level = Random.Range (50.0f, 90.0f);
				el.wander_level = Random.Range (50.0f, 100.0f);
			} else {
				el.savage_level = Random.Range (80.0f, 100.0f);
				el.wander_level = Random.Range (70.0f, 100.0f);
			}

			// set enemy position
			Vector3 epos = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, 0));
			epos.x -= (col + 1) * size - (size / 2);
			epos.y += (row + 1) * size - (size / 2);

			new_enemy.transform.position = new Vector3( epos.x, epos.y );
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

		// reset hits
		PlayerPrefs.SetInt ("hits", 0);

		// reset allowed freeze blasts
		PlayerPrefs.DeleteKey("freeze_blasts");

		player.GetComponent<Animator> ().enabled = false;

		source.Stop ();
		playing = false;
		in_mini_game = false;
	}

	public void SetCurrentBlock(Block b) { current_block = b; }

	private bool isCoinPlacementCollision(int row, int col, List<int[]> points) {
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

	int GetLeadingZeros(Block b) {
		for (int i = 0; i < b.previous_block_hash.Length; i++)
			if (b.previous_block_hash[i].ToString() != "0".ToString())
				return i;

		return b.next_block_hash.Length;
	}
}
