using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushBoxes : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionStay(Collision o)
    {
        if (o.gameObject.CompareTag("Player1"))
        {
            Debug.Log(o.gameObject.name);
        }
    }
}
