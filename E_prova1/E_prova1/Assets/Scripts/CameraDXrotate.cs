using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDXrotate : MonoBehaviour
{
    Vector3 velocity2 = Vector3.zero;
    Vector3 m_CameraRotation = Vector3.zero;
    public float smoothTime = 0.3f;

	void LateUpdate ()
    {
        if (Input.GetButtonDown("3Dmode"))
        {
            if (GameManager.Instance.m_3D_mode)
                m_CameraRotation = new Vector3(18f, 90f, 0f);
            else
                m_CameraRotation = new Vector3(0f, 0f, 0f);
        }
        transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, Camera.main.transform.rotation.eulerAngles, ref velocity2, smoothTime));
	}
}
