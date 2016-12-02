using UnityEngine;
using System.Collections;

public class StopObject : MonoBehaviour
{

    public bool m_isActive = false;

    void OnCollisionExit(Collision col)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (m_isActive)
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
}
