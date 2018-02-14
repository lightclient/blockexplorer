using UnityEngine;
using System.Collections;

public class ProceduralNumberGenerator {
	public static int currentPosition = 0;
	public const string key = "123424123342421432233144441212334432121223344";

	public ProceduralNumberGenerator (int seed) {
		Random.InitState (seed);
	}

	public int GetNextNumber() {
		/*
		string currentNum = key.Substring(currentPosition++ % key.Length, 1);
		return int.Parse (currentNum);
		*/
		return (int)Mathf.Floor (Random.Range (1.0f, 5.0f));
	}
}