using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class block_deserializer {

	public int height;
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

}
