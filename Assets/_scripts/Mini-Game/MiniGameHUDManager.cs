using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameHUDManager : MonoBehaviour {

	public TextMesh height;
	public TextMesh timestamp;
	public TextMesh miner;
	public TextMesh transactions;
	public TextMesh size;
	public TextMesh nonce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateHUD(Block b) {
		height.text = "Height: " + b.height;
		timestamp.text = "Timestamp: " + b.timestamp;
		miner.text = "Miner: " + b.miner;
		transactions.text = "Transactions: " + b.transactions;
		size.text = "Size: " + b.size;
		nonce.text = "Nonce: " + b.nonce;
	}
}
