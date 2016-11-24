using UnityEngine;
using System.Collections;

public class stopMotion : MonoBehaviour {

    void OnCollisionExit(Collision col)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void Update ()
    {

        if ((transform.position - (GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1]).transform.position).magnitude < 4f)
        {
            
            if (Input.GetButtonDown("Interact"))
            {
                Debug.Log(transform.GetChild(0).gameObject.name);
                transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeInHierarchy);
            }

        }

    }
}
