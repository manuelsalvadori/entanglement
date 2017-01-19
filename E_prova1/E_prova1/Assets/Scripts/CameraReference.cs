using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReference : MonoBehaviour {

    private Vector3 velocity2 = Vector3.zero;

    public float smoothTime = 0.3F;

	// Update is called once per frame
	void Update () {

        transform.position = Camera.main.transform.position;
        if (GameManager.Instance.m_Current_State == (int)CoolCameraController.Stato.TreD)
            transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, new Vector3(28f, 90f, 0), ref velocity2, smoothTime));
        else
        {
            transform.rotation = Camera.main.transform.rotation;
        }
	}
}
