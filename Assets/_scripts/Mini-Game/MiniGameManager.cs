using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour {

	public GameObject player;
	public GameObject board;
	public GameObject coin;

	public bool in_mini_game = false;

	private GameObject coin_holder;

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

		coin.transform.position = new Vector3 (250.0f, 250.0f);
		coin_holder = GameObject.Find("coin_holder");
	}

	// Update is called once per frame
	void Update () {
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

		// debug
		// grid = eg.GenerateGrid(b);
		// PrintGrid (grid);

		// place coins
		Random.InitState (b.height);

		for (int i = 0; i < 3; i++) {
			GameObject new_coin = Instantiate(coin, new Vector3(0.0f, 0.0f), Quaternion.identity);
			new_coin.transform.localScale = new Vector3 (0.2f, 0.2f);
			new_coin.transform.parent = coin_holder.transform;

			float row = Mathf.Floor (Random.Range (0.0f, 10.0f));
			float col = Mathf.Floor (Random.Range (0.0f, 10.0f));

			new_coin.transform.localPosition = new Vector3 ((float)(col * size), (float)(row * size), 0.0f);
		}

		in_mini_game = true;
	}

	public void CleanUpLevel() {
		foreach (Transform child in coin_holder.transform) {
			GameObject.Destroy(child.gameObject);
		}
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
