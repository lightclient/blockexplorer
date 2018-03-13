using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public Text lives;
	public Text completed_levels;
	public Text coins_collected;

	private float period = 0.0f;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {

		// update text every 10th of a second
		if (period > 0.1) {
			lives.text            = CreateTextLabel ( PlayerPrefs.GetInt ("lives", 3), 2 );
			completed_levels.text = CreateTextLabel ( PlayerPrefs.GetInt ("completed_levels", 0), 7 );
			coins_collected.text  = CreateTextLabel ( PlayerPrefs.GetInt ("coins_collected", 0 ), 7 );

			period = 0;
		}

		period += UnityEngine.Time.deltaTime;

	}

	string CreateTextLabel(int c, int p) {
		return c.ToString ().PadLeft (p, '0');
	}
}
