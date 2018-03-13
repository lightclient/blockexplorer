using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour {

	public Text lives_label, coins_label, armor_label, freeze_blast_label, item_pricing, teleport_confirm;

	//public string armor, freeze_blast, extra_life, teleport;

	public Button armor_upgrade_btn, freeze_blast_upgrade_btn, extra_life_btn, teleport_btn;

	public GameObject canvas, teleport_menu;

	public InputField input_block;

	private Exhibits exhibits;

	private float period = 0.0f;

	// Use this for initialization
	void Start () {
		exhibits = GameObject.Find("GameManager").GetComponent<Exhibits>();
		UpdateItemList ();
	}
	
	// Update is called once per frame
	void Update () {


		// update text every 10th of a second
		if (period > 0.1) {
			
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

			UpdateItemList ();
		}

		period += UnityEngine.Time.deltaTime;
			
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

		// update item list
		UpdateItemList();

	}

	public void BuyFreezeBlastUpgrade() {

		// subtract cost from balance
		PlayerPrefs.SetInt ("coins_collected", PlayerPrefs.GetInt ("coins_collected", 0) - FreezeBlastCost());

		// upgrade freeze blast
		PlayerPrefs.SetInt ("freeze", PlayerPrefs.GetInt("freeze", 0) + 1);

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

	public void BuyTeleport() {
		// validate input
		// TODO

		int block = 0;

		if (int.TryParse(input_block.text, out block) && ( 0 <= block && block <= exhibits.latestBlock.height)) {
			// you know that the parsing attempt
			// was successful
				Debug.Log("success");
		} else {
			Debug.Log("fail");
		}
			

		Debug.Log (input_block.text);

		// subtract cost from balance
		//PlayerPrefs.SetInt ("coins_collected", PlayerPrefs.GetInt ("coins_collected", 0) - TeleportCost());

		// add 1 to purchase count
		PlayerPrefs.SetInt ("teleports_purchased", PlayerPrefs.GetInt ("teleports_purchased", 0) + 1);

		exhibits.teleport (block);

		HideTeleportMenu ();
		HideStore ();
	}

	public void ShowTeleportMenu() {
		teleport_confirm.text = "This operation cannot be undone. In order to perform another teleportation, more \\itcoin will need to be aquired. Please enter the block you would like to teleport to in box below. The latest block is " + exhibits.latestBlock.height + ".";
		teleport_menu.SetActive(true);
	}

	public void HideTeleportMenu() {
		teleport_menu.SetActive (false);
	}

	public void ShowStore() {
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
			return b + ". . . . . . \\ " + c.ToString (); 
		} else if (c % 10000 != c) {
			return b + " . . . . . . \\ " + c.ToString ();
		} else if (c % 1000 != c) {
			return b + ". . . . . . . \\ " + c.ToString ();
		} else if (c % 100 != c) {
			return b + " . . . . . . . \\ " + c.ToString ();
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
