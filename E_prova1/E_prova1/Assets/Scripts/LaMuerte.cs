using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaMuerte : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

    public Texture2D [] frames;
    float framesPerSecond = 120f;

    // Update is called once per frame
    void Update () {
        int index = (int)(Time.time * framesPerSecond) % frames.Length;

        GetComponent<Renderer>().material.mainTexture = frames[index];
    }
}
