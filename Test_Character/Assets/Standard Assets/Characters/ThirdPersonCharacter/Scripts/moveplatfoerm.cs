﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveplatfoerm : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().velocity += new Vector3(0, 0, Mathf.Sin(Time.timeSinceLevelLoad)/2f);

	}
}
