using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour {

	public Text lives_label, coins_label, armor_label, freeze_blast_label, item_pricing;

	//public string armor, freeze_blast, extra_life, teleport;

	public Button armor_upgrade_btn, freeze_blast_upgrade_btn, extra_life_btn, teleport_btn;

	public GameObject canvas;

	// Use this for initialization
	void Start () {
		UpdateItemList ();
	}
	
	// Update is called once per frame
	void Update () {

		int coins = PlayerPrefs.GetInt ("coins_collected", 0);

		// build lives , coins label
		lives_label.text = CreateTextLabel ( PlayerPrefs.GetInt ("lives", 3), 2 );
		coins_label.text = CreateTextLabel ( coins, 7 );
		armor_label.text = CreateTextLabel ( PlayerPrefs.GetInt ("armor", 0), 2 );
		freeze_blast_label.text = CreateTextLabel ( PlayerPrefs.GetInt ("freeze", 0), 2 );

		// check whether buttons should be active
		armor_upgrade_btn.interactable        = coins > ArmorCost();
		freeze_blast_upgrade_btn.interactable = coins > FreezeBlastCost();
		extra_life_btn.interactable           = coins > ExtraLifeCost();
		teleport_btn.interactable             = coins > TeleportCost();
			
	}

	string CreateTextLabel(int c, int p) {

		//string text = c.ToString ().PadLeft (7, '0');
		//string formatted = text.Substring(0,1) + "" + text.Substring(1,3) + "" + text.Substring(1,3);

		return c.ToString ().PadLeft (p, '0');
	}

	public void BuyArmorUpgrade() {

		// subtract cost from balance
		PlayerPrefs.SetInt ("coins_collected", PlayerPrefs.GetInt ("coins_collected", 0) - ArmorCost());

		// upgrade armor
		PlayerPrefs.SetInt ("armor", PlayerPrefs.GetInt("armor", 0) + 1);

	}

	public void BuyFreezeBlastUpgrade() {

		// subtract cost from balance
		PlayerPrefs.SetInt ("coins_collected", PlayerPrefs.GetInt ("coins_collected", 0) - FreezeBlastCost());

		// upgrade freeze blast
		PlayerPrefs.SetInt ("freeze", PlayerPrefs.GetInt("armor", 0) + 1);

		// update item list
		UpdateItemList();

	}

	public void BuyExtraLife() {

		// subtract cost from balance
		PlayerPrefs.SetInt ("coins_collected", PlayerPrefs.GetInt ("coins_collected", 0) - ExtraLifeCost());

		// add life to counter
		PlayerPrefs.SetInt ("lives", PlayerPrefs.GetInt("lives", 0) + 1);

		// add 1 to purchase count
		PlayerPrefs.SetInt ("lives_purchased", PlayerPrefs.GetInt ("lives_purchased", 0) + 1);

		// update item list
		UpdateItemList();

	}

	public void ShowStore() {
		Debug.Log ("hello");
		canvas.SetActive (true);
	}

	public void HideStore() {
		canvas.SetActive(false);
	}

	private void UpdateItemList() {
		item_pricing.text = ArmorString() + "\n" + FreezeBlastString() + "\n" + ExtraLifeString() + "\n" + TeleportString();
	}

	private string ArmorString() {
		string b = "Armor ";
		int c = ArmorCost ();

		if (c % 100000 != c) {
			return b + ". . . . . . . . . \\ " + c.ToString (); 
		} else if (c % 10000 != c) {
			return b + " . . . . . . . . . \\ " + c.ToString ();
		} else if (c % 1000 != c) {
			return b + ". . . . . . . . . . \\ " + c.ToString ();
		} else if (c % 100 != c) {
			return b + " . . . . . . . . . . \\ " + c.ToString ();
		} else {
			return b + ". . . . . . . . . . . \\ " + c.ToString ();
		}
	}

	private string FreezeBlastString() {
		string b = "Freeze Blast ";
		int c = FreezeBlastCost ();

		if (c % 100000 != c) {
			return b + " . . . . . \\ " + c.ToString (); 
		} else if (c % 10000 != c) {
			return b + ". . . . . . \\ " + c.ToString ();
		} else if (c % 1000 != c) {
			return b + " . . . . . . \\ " + c.ToString ();
		} else if (c % 100 != c) {
			return b + ". . . . . . . \\ " + c.ToString ();
		} else {
			return b + " . . . . . . . \\ " + c.ToString ();
		}
	}

	private string ExtraLifeString() {
		string b = "Extra Life  ";
		int c = ExtraLifeCost ();

		if (c % 100000 != c) {
			return b + " . . . . . . \\ " + c.ToString (); 
		} else if (c % 10000 != c) {
			return b + ". . . . . . . \\ " + c.ToString ();
		} else if (c % 1000 != c) {
			return b + " . . . . . . . \\ " + c.ToString ();
		} else if (c % 100 != c) {
			return b + ". . . . . . . . \\ " + c.ToString ();
		} else {
			return b + ". . . . . . . . \\ " + c.ToString ();
		}
	}

	private string TeleportString() {
		string b = "Teleportation ";
		int c = TeleportCost ();

		if (c % 100000 != c) {
			return b + ". . . . . \\ " + c.ToString (); 
		} else if (c % 10000 != c) {
			return b + " . . . . . \\ " + c.ToString ();
		} else if (c % 1000 != c) {
			return b + ". . . . . . \\ " + c.ToString ();
		} else if (c % 100 != c) {
			return b + " . . . . . . \\ " + c.ToString ();
		} else {
			return b + ". . . . . . . \\ " + c.ToString ();
		}
	}

	private int ArmorCost() {
		return 10 + (PlayerPrefs.GetInt ("armor", 0) == 0 ? 0 : (int)Mathf.Pow (5, PlayerPrefs.GetInt ("armor", 0)));
	}

	private int FreezeBlastCost() {
		return 10 + (PlayerPrefs.GetInt ("freeze", 0) == 0 ? 0 : (int)Mathf.Pow (5, PlayerPrefs.GetInt ("freeze", 0)));
	}

	private int ExtraLifeCost() {
		return 25 + (PlayerPrefs.GetInt ("lives_purchased", 0) == 0 ? 0 : (int)Mathf.Pow (5, PlayerPrefs.GetInt ("lives_purchased", 0)));
	}

	private int TeleportCost() {
		return 50 + (PlayerPrefs.GetInt ("teleports_purchased", 0) == 0 ? 0 : (int)Mathf.Pow (5, PlayerPrefs.GetInt ("teleports_purchased", 0)));
	}
}
