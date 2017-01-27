using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotCam : MonoBehaviour
{
    public Vector3 velocity;
    public float speed = 1f;
    public float smoothTime = 0.3f;
    public Transform doc;

	void Update ()
    {
        transform.position = Vector3.SmoothDamp(transform.position, doc.transform.position, ref velocity, smoothTime);

        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(Input.GetAxis("R_Vertical") * Time.deltaTime * speed, Input.GetAxis("R_Horizontal") * Time.deltaTime * speed, 0f));

	}
}
