using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class block : MonoBehaviour {

	public int height = -1;
	public string hash;
	public string previous_block_hash;
	public string next_block_hash;
	public string merkleroot;
	public string coinbase;
	public string miner;
	public int timestamp;
	public int version;
	public int transactions;
	public int size;
	public int bits;
	public int nonce;

	public bool initialized = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {


		if (!initialized && height > 0) {
			StartCoroutine (GetBlockData ());
			initialized = true;
		}
	}

	IEnumerator GetBlockData() {
		Debug.Log ("getting block " + height);

		string address = "https://bitaps.com/api/block/" + height;
		using (WWW www = new WWW(address)) {

			// yield until the data is received
			yield return www;

			// deinitialize block to redo load if there is an error
			if (!string.IsNullOrEmpty(www.error)) {
				Debug.Log(www.error);
				TextMesh text = GetComponentInChildren<TextMesh>();
				text.text = "ERROR";
				initialized = false;

			} else {
				Debug.Log (www.text);

				// deserialize data from api into blok class
				block_deserializer b = JsonUtility.FromJson<block_deserializer>(www.text);

				// copy values into this class
				height = b.height;
				hash = b.hash;
				previous_block_hash = b.previous_block_hash;
				next_block_hash = b.next_block_hash;
				merkleroot = b.merkleroot;
				coinbase = b.coinbase;
				miner = b.miner;
				timestamp = b.timestamp;
				version = b.version;
				transactions = b.transactions;
				size = b.size;
				bits = b.bits;
				nonce = b.nonce;

				// update the text mesh
				TextMesh text = GetComponentInChildren<TextMesh>();
				text.text = "# " + height;

				// generate art
//				GameObject myLine = new GameObject();
//
//				myLine.AddComponent<LineRenderer>();
//				LineRenderer lr = myLine.GetComponent<LineRenderer>();
//				lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
//				lr.startColor (Color.green);
//				lr.SetWidth(0.1f, 0.1f);
//
//				GameObject.Destroy(myLine, 0.2f);
			}

		}

	}

}
