using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrain : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		tile = GameObject.Find("ground");

		for (int i = 0; i < floor.Length; i++) {
			floor[i] = Instantiate(tile, new Vector3( (start_x - i * blockSize), start_y, 0), Quaternion.identity);

			// flip every other tile on its x & y to spice it up
			if (i % 2 == 0) {
				Vector3 newScale = floor[i].transform.localScale;
				newScale.y *= -1;
				newScale.x *= -1;
				floor[i].transform.localScale = newScale;
			}

			rightMost = 0;
			leftMost = floor.Length-1;
		}
	}
	
	// Update is called once per frame
	void Update () {

		float diff = camera.transform.position.x - last;

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
			rightMost += 1;
		} else if (diff > 0 && floor[leftMost].transform.position.x < (camera.transform.position.x + leftScreenBound - blockSize) ) {
			
			// moving right
			float new_x = floor[rightMost].transform.position.x + blockSize;
			float new_y = floor[rightMost].transform.position.y;
			float new_z = floor[rightMost].transform.position.z;

			floor[leftMost].transform.position = new Vector3(new_x, new_y, new_z);

			rightMost = leftMost;
			leftMost -= 1;
		}


		if (leftMost < 0) {
			leftMost = floor.Length - 1;
		}

		if (rightMost >= floor.Length) {
			rightMost = 0;
		}


		last = camera.transform.position.x;
	}
}
