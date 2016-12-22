using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drone : MonoBehaviour
{
    public float speed = 1f;
    float init_y;
	
    void Start()
    {
        init_y = transform.position.y;
    }

	void Update ()
    {
        transform.position = new Vector3(transform.position.x, init_y + Mathf.Sin(Time.time*speed)/3f, transform.position.z);
	}
}
