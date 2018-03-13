using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public Text lives;
	public Text completed_levels;
	public Text coins_collected;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		
		lives.text            = CreateTextLabel ( PlayerPrefs.GetInt ("lives", 3), 2 );
		completed_levels.text = CreateTextLabel ( PlayerPrefs.GetInt ("completed_levels", 0), 7 );
		coins_collected.text  = CreateTextLabel ( PlayerPrefs.GetInt ("coins_collected", 0 ), 7 );

	}

	string CreateTextLabel(int c, int p) {

		//string text = c.ToString ().PadLeft (7, '0');
		//string formatted = text.Substring(0,1) + "" + text.Substring(1,3) + "" + text.Substring(1,3);

		return c.ToString ().PadLeft (p, '0');
	}
}
