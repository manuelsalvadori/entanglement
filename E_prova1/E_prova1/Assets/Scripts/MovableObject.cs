using UnityEngine;
using System.Collections;

public class MovableObject : MonoBehaviour
{

    public bool m_isActive = false;

    void OnCollisionExit(Collision col)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void setActive ()
    {
        m_isActive = true;
        transform.GetChild(0).gameObject.SetActive(m_isActive);
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
}
