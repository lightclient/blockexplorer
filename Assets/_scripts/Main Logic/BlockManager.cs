using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlockManager : MonoBehaviour {

	public GameObject frame;
	public GameObject check;

	public int height = -1;
	public Block block;

	public bool initialized = false;

	private Color[] one;
	private Color[] two;
	private Color[] three;
	private Color[] four;

	private TextMesh text;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!initialized && height != -1) {
			text.text = "# " + height;
			StartCoroutine (GetBlockData ());
			initialized = true;
		}
	}

	public void completeBlock() {
		SpriteRenderer sr = check.GetComponent<SpriteRenderer> ();
		if (PlayerPrefs.GetInt (block.height.ToString ()) == 1) {
			sr.enabled = true;
		} else {
			sr.enabled = false;
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
				block = JsonUtility.FromJson<Block>(www.text);

				completeBlock ();

				// generate art
				SpriteRenderer current = GetComponentInChildren<SpriteRenderer>();

				ExhibitGenerator eg = new ExhibitGenerator();
				current.sprite = eg.GenerateArt(block);

				current.color = Color.white;

				frame.GetComponent<Animator> ().enabled = false;
				frame.transform.localScale = new Vector3 (100, 100, 10);
			}

		}

	}
}
