using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkingHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {

	}

    public bool locker = false;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT");
        if (transform.GetChild(0).GetComponent<ThirdPersonCharacterNostro>().m_IsGrounded && !collision.gameObject.tag.Equals("Ground") && !locker)
            locker = true;
    }

    private void OnCollisionStay(Collision collision)
    {

        if (transform.GetChild(0).GetComponent<ThirdPersonCharacterNostro>().m_IsGrounded && !collision.gameObject.tag.Equals("Ground") && locker)
        {
            Debug.Log("Change");
            if (transform.GetChild(0).gameObject.tag.Equals("Player1"))
            {
                GameManager.Instance.m_lockedPlayer = 1;
            }
            else
                GameManager.Instance.m_lockedPlayer = 0;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (transform.GetChild(0).GetComponent<ThirdPersonCharacterNostro>().m_IsGrounded && !collision.gameObject.tag.Equals("Ground") && locker)
            locker = false;
    }
}
