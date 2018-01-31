using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class exhibits : MonoBehaviour {

	public GameObject camera;
	public GameObject frame;

	public float leftExhibitCacheArea;
	public float rightExhibitCacheArea;

	private GameObject[] frames = new GameObject[40];

	private float frameSize = 30.0f;
	private float start_x = 0.0f;
	private float start_y = 7.0f;
	private float last;

	private int rightMost;
	private int leftMost;
	private block_deserializer latestBlock;

	// Use this for initialization
	void Start () {

		// construct and send request
		StartCoroutine(GetBlockData());
	}

	IEnumerator GetBlockData() {
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

			latestBlock = JsonUtility.FromJson<block_deserializer> (www.downloadHandler.text);

			frame.transform.position = new Vector3 (0, 100, 0);

			for (int i = 0; i < frames.Length; i++) {
				frames [i] = Instantiate (frame, new Vector3 ((start_x - i * frameSize), start_y, 0), Quaternion.identity);

				frames [i].GetComponent<block> ().height = latestBlock.height - i;
				Debug.Log (frames [i].GetComponent<block> ().height);

				rightMost = 0;
				leftMost = frames.Length - 1;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
		float diff = camera.transform.position.x - last;

		// only move terrian if the camera is moving
		// diff < 0 means moving left
		// diff > 0 means moving right
		if (diff < 0 && frames[rightMost].transform.position.x > (camera.transform.position.x + rightExhibitCacheArea + frameSize) ) {

			// moving left
			float new_x = frames[leftMost].transform.position.x - frameSize;
			float new_y = frames[leftMost].transform.position.y;
			float new_z = frames[leftMost].transform.position.z;

			frames[rightMost].transform.position = new Vector3(new_x, new_y, new_z);

			// update block number
			frames[rightMost].GetComponent<block>().height = frames[leftMost].GetComponent<block>().height - 1;
			frames[rightMost].GetComponent<block> ().initialized = false;

			leftMost = rightMost;
			rightMost += 1;

		} else if (diff > 0 && frames[leftMost].transform.position.x < (camera.transform.position.x + leftExhibitCacheArea - frameSize) && 
			latestBlock.height != frames[rightMost].GetComponent<block>().height ) {

			// moving right
			float new_x = frames[rightMost].transform.position.x + frameSize;
			float new_y = frames[rightMost].transform.position.y;
			float new_z = frames[rightMost].transform.position.z;

			frames[leftMost].transform.position = new Vector3(new_x, new_y, new_z);

			// update block number
			frames[leftMost].GetComponent<block>().height = frames[rightMost].GetComponent<block>().height + 1;
			frames[leftMost].GetComponent<block> ().initialized = false;

			rightMost = leftMost;
			leftMost -= 1;
		}


		if (leftMost < 0) {
			leftMost = frames.Length - 1;
		}

		if (rightMost >= frames.Length) {
			rightMost = 0;
		}


		last = camera.transform.position.x;

	}
}