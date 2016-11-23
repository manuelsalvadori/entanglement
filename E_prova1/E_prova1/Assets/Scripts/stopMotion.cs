using UnityEngine;
using System.Collections;

public class stopMotion : MonoBehaviour {

    void OnCollisionExit(Collision col)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
