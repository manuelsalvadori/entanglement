﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drone : MonoBehaviour
{
    public float speed = 1f;
    float init_y;
    float rnd;
	
    void Start()
    {
        init_y = transform.localPosition.y;
        rnd = Random.Range(0.1f,0.9f);
    }

	void Update ()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, init_y + Mathf.Sin(Time.time*speed + rnd)/3f, transform.localPosition.z);
	}
}
