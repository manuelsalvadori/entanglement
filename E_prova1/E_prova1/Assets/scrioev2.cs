using UnityEngine;
using System.Collections;

public class scrioev2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(GameManager.Instance.m_players[0].transform.position.x, transform.position.y, transform.position.z);
	}
}
