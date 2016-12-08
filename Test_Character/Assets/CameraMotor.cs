using UnityEngine;
using System.Collections;

public class CameraMotor : MonoBehaviour {

	public GameObject target;
	public float distance;

	// Update is called once per frame
	void Update () {
		transform.position = target.transform.position - new Vector3 (0, 0, distance);
	}
}
