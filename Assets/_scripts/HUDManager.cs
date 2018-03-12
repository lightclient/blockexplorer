using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public GameObject heart1;
	public GameObject heart2;
	public GameObject heart3;

	public Text completed_levels;
	public Text coins_collected;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

		if (PlayerPrefs.GetInt ("lives", 3) == 3) {
			heart1.active = true;
			heart2.active = true;
			heart3.active = true;
		} else if (PlayerPrefs.GetInt ("lives", 3) == 3) {
			heart1.active = true;
			heart2.active = true;
		} else if (PlayerPrefs.GetInt ("lives", 3) == 3) {
			heart1.active = true;
		} else {
		}

		completed_levels.text = CreateTextLabel ( PlayerPrefs.GetInt ("completed_levels", 0) );
		coins_collected.text  = CreateTextLabel ( PlayerPrefs.GetInt ("coins_collected", 0 ) );

	}

	string CreateTextLabel(int c) {

		//string text = c.ToString ().PadLeft (7, '0');
		//string formatted = text.Substring(0,1) + "" + text.Substring(1,3) + "" + text.Substring(1,3);

		return c.ToString ().PadLeft (7, '0');
	}
}
