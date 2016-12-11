using UnityEngine;
using System.Collections;

public class MovableObject : MonoBehaviour
{

    public bool m_isActive = false;
    public float deltay = -14f;

    void OnCollisionExit(Collision col)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (m_isActive)
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    public void setActive ()
    {
        m_isActive = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ |RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        GetComponent<Rigidbody>().MovePosition(transform.position + new Vector3(0f, deltay,0f));
    }

    void OnCollisionStay(Collision o)
    {   
        if (Mathf.Abs(o.collider.bounds.min.y - transform.GetComponent<Collider>().bounds.max.y) < 0.02f)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
