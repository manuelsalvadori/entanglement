using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnIfFall : MonoBehaviour {

    private Vector3 birth_Position;
    public GameObject respawn;

	// Use this for initialization
	void Start () {
        birth_Position = transform.position;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetInstanceID() == respawn.GetInstanceID())
        {
            transform.position = birth_Position;
        }
    }
}
