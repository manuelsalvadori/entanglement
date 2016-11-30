using UnityEngine;
using System.Collections;

public class MovableObject : MonoBehaviour
{

    public bool m_isActive = false;

    void OnCollisionExit(Collision col)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (m_isActive)
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    public void setActive ()
    {
        m_isActive = true;
        transform.GetChild(0).gameObject.SetActive(m_isActive);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    void OnCollisionStay(Collision o)
    {   
        if (Mathf.Abs(o.collider.bounds.min.y - transform.GetComponent<Collider>().bounds.max.y) < 0.02f)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
