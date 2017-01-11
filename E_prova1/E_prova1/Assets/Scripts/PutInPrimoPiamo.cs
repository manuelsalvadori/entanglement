using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutInPrimoPiamo : MonoBehaviour {


    Vector3 velocity2 = Vector3.zero;

    public float smoothTime = 0.05f;


    public Vector3 where;

	// Update is called once per frame
	void Update () {
        transform.position = Vector3.SmoothDamp(transform.position, Camera.main.transform.position + where, ref velocity2, smoothTime);
	}

    public void SetPosition(Vector3 here)
    {
        where = here;
    }
}


