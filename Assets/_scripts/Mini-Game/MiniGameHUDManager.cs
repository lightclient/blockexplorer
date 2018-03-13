using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameHUDManager : MonoBehaviour {

	// player info
	public Text lives, coins, freeze_blasts;

	// block info
	public Text height, timestamp, miner, transactions, size, bits, nonce;

	public Slider armor_bar;

	private float period = 0.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		// only update labels every 10th of a second
		if (period > 0.1) {
			lives.text = CreateTextLabel(PlayerPrefs.GetInt("lives", 3), 2);
			coins.text = CreateTextLabel(PlayerPrefs.GetInt ("coins_collected", 0 ), 6  );
			freeze_blasts.text = CreateTextLabel (PlayerPrefs.GetInt ("freeze_blasts", PlayerPrefs.GetInt("freeze",0) + 1), 2);

			armor_bar.value = (PlayerPrefs.GetInt ("armor", 0) + 2) - PlayerPrefs.GetInt ("hits", 0);

			period = 0;
		}

		period += UnityEngine.Time.deltaTime;
	}

	public void SetHUD(Block b) {
		height.text = "Height: " + b.height;
		timestamp.text = "Timestamp: " + b.timestamp;
		miner.text = "Miner: " + b.miner;
		transactions.text = "Transactions: " + b.transactions;
		size.text = "Size: " + b.size;
		bits.text = "Bits: " + b.bits;
		nonce.text = "Nonce: " + b.nonce;

		armor_bar.minValue = 0;
		armor_bar.maxValue = PlayerPrefs.GetInt ("armor", 0) + 2;
		armor_bar.value = armor_bar.maxValue;
	}

	string CreateTextLabel(int c, int p) {
		return c.ToString ().PadLeft (p, '0');
	}
}
