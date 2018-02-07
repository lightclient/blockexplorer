using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maze_generator : MonoBehaviour {

	[System.Serializable]
	public class Cell {
		public bool visited;
		public GameObject north;
		public GameObject east;
		public GameObject south;
		public GameObject west;
	}

	public GameObject wall;
	public float wall_length = 10.0f;

	public int x_size;
	public int y_size;

	public Cell[] cells;
	public int current_cell = 0;

	private GameObject wall_holder;

	// Use this for initialization
	void Start () {
		generate_grid();
	}

	void generate_grid() {
		
		wall_holder = new GameObject ();
		wall_holder.name = "Maze";

		GameObject temp_wall;



		// for x
		Vector3 initial_position = new Vector3 (-55.5f, 170.0f);
		for (int i = 0; i < y_size; i++) {
			for (int j = 0; j <= x_size; j++) {
				temp_wall = Instantiate (wall, new Vector3 (initial_position.x + j * wall_length, initial_position.y + i * wall_length), Quaternion.Euler(0.0f, 0.0f, 90.0f)) as GameObject;
				temp_wall.transform.parent = wall_holder.transform;
			}
		}


		// for y
		initial_position = new Vector3 (-50.4f, 165.51f);
		for (int i = 0; i <= y_size; i++) {
			for (int j = 0; j < x_size; j++) {
				temp_wall = Instantiate (wall, new Vector3(initial_position.x + j * wall_length, initial_position.y + i * wall_length), Quaternion.identity) as GameObject;
				temp_wall.transform.parent = wall_holder.transform;
			}
		}

		create_cells();

	}

	void create_cells() {
		int n_children = wall_holder.transform.childCount;
		GameObject[] walls = new GameObject[n_children];

		cells = new Cell[x_size * y_size];

		int east_west_process = 0;
		int child_process = 0;
		int term_count = 0;

		// get all walls
		for(int i = 0; i < n_children; i++) {
			walls[i] = wall_holder.transform.GetChild(i).gameObject;
		}


		for (int cell_process = 0; cell_process < cells.Length; cell_process++) {
			cells [cell_process] = new Cell ();
			cells [cell_process].west = walls [east_west_process];
			cells [cell_process].south = walls [child_process + (x_size + 1) * y_size];

			if (term_count == x_size) {
				east_west_process += 2;
				term_count = 0;
			} else {
				east_west_process++;
			}

			term_count++;
			child_process++;

			cells [cell_process].east = walls [east_west_process];
			cells [cell_process].north = walls [ (child_process + (x_size + 1) * y_size) + x_size - 1];
		}

		create_maze ();
	}

	void create_maze() {
		get_neighbor ();

	}

	void get_neighbor() {
		int length = 0;
		int[] neighbor = new int[4];
		int check = (current_cell + 1) / x_size;

		check -= 1;
		check *= x_size;
		check += x_size;

		// current_cell = Random.

		// get west wall
		if (current_cell + 1 < cells.Length && (current_cell + 1) != check) {
			if (cells [current_cell + 1].visited == false) {
				neighbor [length] = current_cell + 1;
				length++;
			}
		}

		// get east wall
		if (current_cell - 1 >= 0 && current_cell != check) {
			if (cells [current_cell - 1].visited == false) {
				neighbor [length] = current_cell - 1;
				length++;
			}
		}

		// get north wall
		if (current_cell + x_size < cells.Length) {
			if (cells [current_cell + 1].visited == false) {
				neighbor [length] = current_cell + x_size;
				length++;
			}
		}

		// get south wall
		if (current_cell - x_size >= 0) {
			if (cells [current_cell - 1].visited == false) {
				neighbor [length] = current_cell - x_size;
				length++;
			}
		}


		for(int i = 0; i < length; i++)
			Debug.Log (neighbor [i]);
			
	}

	// Update is called once per frame
	void Update () {
		
	}
}
