using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Exhibits : MonoBehaviour {

	public GameObject camera;
	public GameObject frame;

	public float leftExhibitCacheArea;
	public float rightExhibitCacheArea;

	private GameObject[] frames = new GameObject[40];

	public int block_rate;

	private float frameSize = 30.0f;
	private float start_x = -10.0f;
	private float start_y = 20.0f;
	private float last;

	private int rightMost;
	private int leftMost;
	public Block latestBlock;

	private GameObject exhibit_holder;

	private GameManager game_manager;
	private Terrain terrain_manager;

	// Use this for initialization
	void Start () {

		game_manager    = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		terrain_manager = GameObject.Find ("GameManager").GetComponent<Terrain> ();

		exhibit_holder = new GameObject ();
		exhibit_holder.name = "Exhibits";

		// construct and send request
		StartCoroutine(GetBlockData(0));
	}

	public void teleport(int height) {


		foreach (Transform t in exhibit_holder.transform) {
			GameObject.Destroy (t.gameObject);
		}

		// determine which frames will be the left and right most after teleport
		int leftMostHeight = ((height - (frames.Length / 2) * block_rate) < 0) ? -1 : (height - (frames.Length / 2) * block_rate);
		int rightMostHeight = ((height + (frames.Length / 2) * block_rate) > latestBlock.height) ? latestBlock.height : (height  + (frames.Length / 2) * block_rate);

		// if we're at the latest block, then the left most block will need to be updated
		leftMostHeight = rightMostHeight == latestBlock.height ? (leftMostHeight - frames.Length / 2 * block_rate) : leftMostHeight;

		leftMostHeight += 1;

		Vector3 position_to_jump_to = new Vector3 (0.0f, 0.0f);

		frame.transform.position = new Vector3 (0, 90, 0);

		for (int i = 0; i < frames.Length; i++) {
			frames [i] = Instantiate (frame, new Vector3 ((start_x + i * frameSize), start_y, 0), Quaternion.identity);

			frames [i].GetComponentInChildren<BlockManager>().height = (i * block_rate + leftMostHeight);
			frames [i].transform.parent = exhibit_holder.transform;

			Debug.Log ("curr = " + (i * block_rate + leftMostHeight) + " looking for: " + height);

			if ((i * block_rate + leftMostHeight) == height) {
				position_to_jump_to = frames [i].transform.position;
			}

			if (PlayerPrefs.GetInt ((i * block_rate).ToString ()) == 1) {
				//frame
			}

			leftMost = 0;
			rightMost = frames.Length - 1;
		}

		position_to_jump_to.x += frameSize / 2 - frameSize / 4;
		terrain_manager.TeleportTerrain (position_to_jump_to);
		game_manager.player.transform.position = position_to_jump_to;

		Vector3 new_camera_pos = camera.transform.position;
		new_camera_pos.x = position_to_jump_to.x;
		camera.transform.position = new_camera_pos;

		/*
		for (int i = 0; i < frames.Length; i++) {
			//frames [i] = Instantiate (frame, new Vector3 ((start_x + i * frameSize), start_y, 0), Quaternion.identity);

			frames [(leftMost + i) % frames.Length].GetComponentInChildren<BlockManager>().height = (i-closest_i) * block_rate + height;
			frames [(leftMost + i) % frames.Length].GetComponentInChildren<BlockManager>().initialized = false;
			frames [(leftMost + i) % frames.Length].transform.parent = exhibit_holder.transform;

			if (PlayerPrefs.GetInt ((i * block_rate).ToString ()) == 1) {
				//frame
			}

			Debug.Log (frames [(leftMost + i) % frames.Length].GetComponentInChildren<BlockManager> ().height);
		}
		*/
	}

	IEnumerator GetBlockData(int start) {
		UnityWebRequest www = UnityWebRequest.Get("https://bitaps.com/api/block/latest");
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError) {
			Debug.Log (www.error);
		} else {
			// determine if there was an error retrieving the data
			if (www.isNetworkError || www.isHttpError) {
				Debug.Log (www.error);
				Application.Quit ();
			}

			latestBlock = JsonUtility.FromJson<Block> (www.downloadHandler.text);

			frame.transform.position = new Vector3 (0, 90, 0);

			for (int i = 0; i < frames.Length; i++) {
				frames [i] = Instantiate (frame, new Vector3 ((start_x + i * frameSize), start_y, 0), Quaternion.identity);

				frames [i].GetComponentInChildren<BlockManager>().height = i * block_rate + start;
				frames [i].transform.parent = exhibit_holder.transform;

				if (PlayerPrefs.GetInt ((i * block_rate).ToString ()) == 1) {
					//frame
				}

				Debug.Log (frames [i].GetComponentInChildren<BlockManager> ().height);

				leftMost = 0;
				rightMost = frames.Length - 1;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
		float diff = camera.transform.position.x - last;

		/*
		 if (diff != last) {
			Debug.Log ("diff: " + diff);
			Debug.Log ("right most pos: " + frames [rightMost].transform.position.x);
			Debug.Log ("right cache pos: " + (camera.transform.position.x + rightExhibitCacheArea + frameSize));

			Debug.Log ("left most pos: " + frames [leftMost].transform.position.x);
			Debug.Log ("left cache pos: " + (camera.transform.position.x + leftExhibitCacheArea - frameSize));
		}
		*/

		// only move terrian if the camera is moving
		// diff < 0 means moving left
		// diff > 0 means moving right
		if (diff < 0 && frames[rightMost].transform.position.x > (camera.transform.position.x + rightExhibitCacheArea + frameSize) && 
			0 <= frames[leftMost].GetComponent<BlockManager>().height - block_rate ) {

			// moving left
			float new_x = frames[leftMost].transform.position.x - frameSize;
			float new_y = frames[leftMost].transform.position.y;
			float new_z = frames[leftMost].transform.position.z;

			frames[rightMost].transform.position = new Vector3(new_x, new_y, new_z);

			Debug.Log ("new left block: " + (frames [leftMost].GetComponent<BlockManager> ().height - block_rate));

			// update block number
			frames[rightMost].GetComponent<BlockManager>().height = frames[leftMost].GetComponent<BlockManager>().height - block_rate;
			frames[rightMost].GetComponent<BlockManager>().initialized = false;

			leftMost = rightMost;
			rightMost -= 1;

		} else if (diff > 0 && frames[leftMost].transform.position.x < (camera.transform.position.x + leftExhibitCacheArea - frameSize) && 
			latestBlock.height >= frames[rightMost].GetComponent<BlockManager>().height + block_rate ) {

			// moving right
			float new_x = frames[rightMost].transform.position.x + frameSize;
			float new_y = frames[rightMost].transform.position.y;
			float new_z = frames[rightMost].transform.position.z;

			frames[leftMost].transform.position = new Vector3(new_x, new_y, new_z);

			// update block number
			frames[leftMost].GetComponent<BlockManager>().height = frames[rightMost].GetComponent<BlockManager>().height + block_rate;
			frames[leftMost].GetComponent<BlockManager>().initialized = false;

			Debug.Log ("new right block: " + (frames [leftMost].GetComponent<BlockManager> ().height - block_rate));

			rightMost = leftMost;
			leftMost += 1;
		}


		if (leftMost >= frames.Length) {
			leftMost = 0;
		}

		if (rightMost < 0) {
			rightMost = frames.Length - 1;
		}


		last = camera.transform.position.x;

	}
}