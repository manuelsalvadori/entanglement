using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTexture : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = CreateTexture();
	}

    public Texture2D CreateTexture()
    {
        int width = 100;
        int height = 100;

        Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex.filterMode = FilterMode.Point;

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++)
            {
                tex.SetPixel(j, tex.height-1-i, Random.ColorHSV());
            }
        }
        tex.Apply();
        return tex;
    }
     
}
