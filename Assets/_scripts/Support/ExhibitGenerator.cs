using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExhibitGenerator {
	
	private Color[] one   = new Color[2] { Color.black, Color.gray };
	private Color[] two   = new Color[4] { Color.black, Color.white, Color.gray, Color.blue };
	private Color[] three = new Color[8] { Color.black, Color.white, Color.blue, Color.green, Color.red, Color.yellow, Color.magenta, Color.cyan };
	private Color[] four  = new Color[16] { Color.black, Color.white, Color.blue, Color.green, Color.red, Color.yellow, Color.magenta, Color.cyan,
			   			    new Color(0.0f, 0.9f, 0.5f), new Color(0.5f, 0.1f, 0.9f), new Color(0.6f, 0.0f, 0.0f), new Color(1.0f, 1.0f, 0.0f),
			   			    new Color(1.0f, 0.6f, 1.0f), new Color(0.8f, 0.9f, 0.5f), new Color(0.0f, 0.9f, 0.5f), new Color(0.0f, 0.9f, 0.5f) };
	
	public Sprite GenerateArt(Block b) {
		Texture2D tex = new Texture2D(10,10);

		Vector3 point00 = new Vector3(-0.5f,-0.5f);
		Vector3 point10 = new Vector3( 0.5f,-0.5f);
		Vector3 point01 = new Vector3(-0.5f, 0.5f);
		Vector3 point11 = new Vector3( 0.5f, 0.5f);

		float resolution = 10;

		float stepSize = 1f / resolution;

		int first = 1;
		int second = 1;
		if (b.miner != "unknown") {
			Random.InitState(b.miner.GetHashCode());
			first = (int)Mathf.Floor (Random.Range (0.0f, 3.0f));
			second = (int)Mathf.Floor (Random.Range (0.0f, 3.0f));
		}

		int bits_of_color = Mathf.Clamp( (GetLeadingZeros (b) - 8) + (b.transactions == 1 ? 0 : 1) , 1, 8);

		// Debug.Log (height + " " + previous_block_hash + " bits -> " + bits_of_color);

		Random.InitState (b.height);

		for (int y = 0; y < resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
			for (int x = 0; x < resolution; x++) {
				Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);

				Color pixel_color = Color.black;

				if(bits_of_color > 3) {

					float[] c = new float[3];

					int c1 = ( (int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow(2, bits_of_color)) ) << (8 - bits_of_color) );
					int c2 = ( (int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow(2, bits_of_color)) ) << (8 - bits_of_color) );
					int c3 = ( (int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow(2, bits_of_color)) ) << (8 - bits_of_color) );

					c[0] = c1 / 255.0f;
					c[1] = c2 / 255.0f;
					c[2] = c3 / 255.0f;

					c[first] = c[second];

					pixel_color = new Color (c [0], c [1], c [2]);
				} else {

					switch(bits_of_color) {

					case 1:
						pixel_color = one [(int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow (2, bits_of_color)))];
						break;
					case 2:
						pixel_color = two [(int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow (2, bits_of_color)))];
						break;
					case 3:
						pixel_color = three [(int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow (2, bits_of_color)))];
						break;
					case 4:
						pixel_color = four [(int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow (2, bits_of_color)))];
						break;
					}
				}

				tex.SetPixel(x, y, pixel_color);
			}
		}

		tex.Apply();

		tex.filterMode = FilterMode.Point;

		//current.drawMode = SpriteDrawMode.Tiled;
		Sprite art = Sprite.Create (tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect);

		return art;
	}

	public float[,] GenerateGrid(Block b) {

		float [,] grid = new float[10,10];

		Texture2D tex = new Texture2D(10,10);

		Vector3 point00 = new Vector3(-0.5f,-0.5f);
		Vector3 point10 = new Vector3( 0.5f,-0.5f);
		Vector3 point01 = new Vector3(-0.5f, 0.5f);
		Vector3 point11 = new Vector3( 0.5f, 0.5f);

		float resolution = 10;

		float stepSize = 1f / resolution;

		int first = 1;
		int second = 1;
		if (b.miner != "unknown") {
			Random.InitState(b.miner.GetHashCode());
			first = (int)Mathf.Floor (Random.Range (0.0f, 3.0f));
			second = (int)Mathf.Floor (Random.Range (0.0f, 3.0f));
		}

		int bits_of_color = Mathf.Clamp( (GetLeadingZeros (b) - 8) + (b.transactions == 1 ? 0 : 1) , 1, 8);

		// Debug.Log (height + " " + previous_block_hash + " bits -> " + bits_of_color);

		Random.InitState (b.height);

		for (int y = 0; y < resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
			for (int x = 0; x < resolution; x++) {
				Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);

				Color pixel_color = Color.black;

				if(bits_of_color > 3) {

					float[] c = new float[3];

					int c1 = ( (int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow(2, bits_of_color)) ) << (8 - bits_of_color) );
					int c2 = ( (int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow(2, bits_of_color)) ) << (8 - bits_of_color) );
					int c3 = ( (int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow(2, bits_of_color)) ) << (8 - bits_of_color) );

					c[0] = c1 / 255.0f;
					c[1] = c2 / 255.0f;
					c[2] = c3 / 255.0f;

					c[first] = c[second];

					pixel_color = new Color (c [0], c [1], c [2]);
				} else {

					switch(bits_of_color) {

					case 1:
						pixel_color = one [(int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow (2, bits_of_color)))];
						break;
					case 2:
						pixel_color = two [(int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow (2, bits_of_color)))];
						break;
					case 3:
						pixel_color = three [(int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow (2, bits_of_color)))];
						break;
					case 4:
						pixel_color = four [(int)Mathf.Floor (Random.Range (0.0f, Mathf.Pow (2, bits_of_color)))];
						break;
					}
				}

				grid [x, y] = pixel_color.r + pixel_color.g + pixel_color.b;
			}
		}

		return grid;
	}


	public int GetLeadingZeros(Block b) {
		for (int i = 0; i < b.previous_block_hash.Length; i++)
			if (b.previous_block_hash[i].ToString() != "0".ToString())
				return i;

		return b.next_block_hash.Length;
	}
}
