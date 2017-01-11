using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Respawn")) {
            GetComponent<Rigidbody>().velocity += collision.gameObject.GetComponent<Rigidbody>().velocity;
            Debug.Log(GetComponent<Rigidbody>().velocity);
        }
    }
}
