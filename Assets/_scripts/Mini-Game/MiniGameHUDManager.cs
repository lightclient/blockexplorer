using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MiniGameHUDManager : MonoBehaviour {

	// player info
	public Text lives, coins, freeze_blasts, armor;

	// block info
	public Text height, date, time, miner, transactions, size, nonce;

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
			armor.text = CreateTextLabel ((PlayerPrefs.GetInt ("armor", 0) + 2) - PlayerPrefs.GetInt ("hits", 0), 2);

			period = 0;
		}

		period += UnityEngine.Time.deltaTime;
	}

	public void SetHUD(Block b) {
		height.text = "Height: " + b.height;
		date.text = "Date: " + TimestampToDate(b.timestamp);
		time.text = "Time: " + TimestampToTime (b.timestamp);
		miner.text = "Miner: " + b.miner;
		transactions.text = "Transactions: " + b.transactions;
		size.text = "Size: " + b.size;
		nonce.text = "Nonce: " + b.nonce;

		armor_bar.minValue = 0;
		armor_bar.maxValue = PlayerPrefs.GetInt ("armor", 0) + 2;
		armor_bar.value = armor_bar.maxValue;
	}

	string CreateTextLabel(int c, int p) {
		return c.ToString ().PadLeft (p, '0');
	}

	string TimestampToDate(int ts) {
		return epoch.AddSeconds(ts).ToShortDateString();
	}

	string TimestampToTime(int ts) {
		return epoch.AddSeconds (ts).ToShortTimeString ();
	}

	private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}
