using UnityEngine;
using System.Collections;

public class FreezeOnContact : MonoBehaviour
{

    void OnCollisionEnter(Collision o)
    {
        Debug.Log(GetComponentInParent<Rigidbody>().velocity.ToString());
        GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
    }
}
