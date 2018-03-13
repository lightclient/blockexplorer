using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour {

	public GameObject camera;
	public GameObject tile;

	public float leftScreenBound;
	public float rightScreenBound;


	private GameObject[] floor = new GameObject[20];

	public float blockSize = 10.0f;
	private float start_x = 71.9F;
	private float start_y = -30.0F;
	private float last;

	private int rightMost;
	private int leftMost;

	private GameObject terrain_holder;

	// Use this for initialization
	void Start () {
		terrain_holder = new GameObject ();
		terrain_holder.name = "Terrain";

		tile = GameObject.Find("ground");

		for (int i = 0; i < floor.Length; i++) {
			floor[i] = Instantiate(tile, new Vector3( (start_x - i * blockSize), start_y, 0), Quaternion.identity);
			floor[i].transform.parent = terrain_holder.transform;

			// flip every other tile on its x & y to spice it up
			if (i % 2 == 0) {
				Vector3 newScale = floor[i].transform.localScale;
				newScale.y *= -1;
				newScale.x *= -1;
				floor[i].transform.localScale = newScale;
			}
		}

		rightMost = floor.Length-1;
		leftMost = 0;

		TeleportTerrain (camera.transform.position);
	}

	public void TeleportTerrain(Vector3 pos) {
		float leftMostX = pos.x - (floor.Length / 2 * blockSize);

		for (int i = 0; i < floor.Length; i++) {
			Vector3 new_pos = floor [(leftMost + i) % floor.Length].transform.position;
			new_pos.x = leftMostX + i * blockSize;
			floor [(leftMost + i) % floor.Length].transform.position = new_pos;
		}
	}

	// Update is called once per frame
	void Update () {

		float diff = camera.transform.position.x - last;

		/*
		if (diff != 0) {
			Debug.Log ("diff: " + diff);
			Debug.Log ("camera: " + camera.transform.position.x);
			Debug.Log ("last: " + last);
			Debug.Log ("right most pos: " + floor [rightMost].transform.position.x);
			//Debug.Log ("right cache pos: " + (camera.transform.position.x + rightScreenBound + blockSize));

			Debug.Log ("left most pos: " + floor [leftMost].transform.position.x);
			//Debug.Log ("left cache pos: " + (camera.transform.position.x + leftScreenBound - blockSize));
		}
		*/

		// only move terrian if the camera is moving
		// diff < 0 means moving left
		// diff > 0 means moving right
		if (diff < 0 && floor[rightMost].transform.position.x > (camera.transform.position.x + rightScreenBound + blockSize) ) {

			// moving left
			float new_x = floor[leftMost].transform.position.x - blockSize;
			float new_y = floor[leftMost].transform.position.y;
			float new_z = floor[leftMost].transform.position.z;

			floor[rightMost].transform.position = new Vector3(new_x, new_y, new_z);

			leftMost = rightMost;
			rightMost -= 1;

		} else if (diff > 0 && floor[leftMost].transform.position.x < (camera.transform.position.x + leftScreenBound - blockSize) ) {
			
//			// moving right
			float new_x = floor[rightMost].transform.position.x + blockSize;
			float new_y = floor[rightMost].transform.position.y;
			float new_z = floor[rightMost].transform.position.z;

			floor[leftMost].transform.position = new Vector3(new_x, new_y, new_z);

			rightMost = leftMost;
			leftMost += 1;
		}


		if (leftMost >= floor.Length) {
			leftMost = 0;
		}

		if (rightMost < 0) {
			rightMost = floor.Length - 1;
		}


		last = camera.transform.position.x;
	}
}
