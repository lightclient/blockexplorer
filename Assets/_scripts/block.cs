using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class block : MonoBehaviour {

	public GameObject frame;

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
				TextMesh text = GetComponentInParent<TextMesh>();
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
				SpriteRenderer current = GetComponentInChildren<SpriteRenderer>();

				Texture2D tex = new Texture2D(10,10);

				Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
				Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
				Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
				Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));

				float resolution = 10;

				float stepSize = 1f / resolution;

				int first = 1;
				int second = 1;
				if (miner != "unknown") {
					Random.InitState(miner.GetHashCode());
					first = (int)Mathf.Floor (Random.Range (0.0f, 3.0f));
					second = (int)Mathf.Floor (Random.Range (0.0f, 3.0f));
				}


				Random.InitState (height);

				for (int y = 0; y < resolution; y++) {
					Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
					Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
					for (int x = 0; x < resolution; x++) {
						Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);

						float[] c = new float[3];

						c[0] = Random.Range (0.0f, 1.0f);
						c[1] = Random.Range (0.0f, 1.0f);
						c[2] = Random.Range (0.0f, 1.0f);

						c[first] = c[second];

						tex.SetPixel(x, y, new Color(c[0], c[1], c[2]));
					}
				}
					
				tex.Apply();

				tex.filterMode = FilterMode.Point;

				//current.drawMode = SpriteDrawMode.Tiled;
				Sprite art = Sprite.Create (tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect);

				current.sprite = art;

				frame.transform.localScale = new Vector3 (100, 100, 10);
			}

		}

	}

}
