using UnityEngine;
using System.Collections;

public class SwitchGadget : MonoBehaviour
{

    int m_state = 2;

	void Update ()
    {
        if (Input.GetButtonDown("Next") && !transform.GetChild(0).GetComponent<Animation>().isPlaying)
        {
            m_state++;
            m_state = (m_state > 2) ? 0 : m_state;
            transform.GetChild(m_state).GetComponent<Animation>()["RigthToCenter"].speed = 1f;
            transform.GetChild(m_state).GetComponent<Animation>()["RigthToCenter"].time = 0f;
            transform.GetChild((m_state + 2) % 3).GetComponent<Animation>()["CenterToLeft"].speed = 1f;
            transform.GetChild((m_state + 2) % 3).GetComponent<Animation>()["CenterToLeft"].time = 0f;
            transform.GetChild((m_state + 1) % 3).GetComponent<Animation>()["LeftToRigth"].speed = 1f;
            transform.GetChild((m_state + 1) % 3).GetComponent<Animation>()["LeftToRigth"].time = 0f;
            transform.GetChild(m_state).GetComponent<Animation>().Play("RigthToCenter");
            transform.GetChild((m_state + 2) % 3).GetComponent<Animation>().Play("CenterToLeft");
            transform.GetChild((m_state + 1) % 3).GetComponent<Animation>().Play("LeftToRigth");
        }

        if (Input.GetButtonDown("Previous") && !transform.GetChild(0).GetComponent<Animation>().isPlaying)
        {
            m_state--;
            m_state = (m_state < 0) ? 2 : m_state;
            int m_state1 = (m_state - 1 < 0) ? m_state + 2 : m_state -1;
            int m_state2 = (m_state - 2 < 0) ? ((m_state - 2 == -1)? 2 : 1) : m_state - 2;

            transform.GetChild(m_state2).GetComponent<Animation>()["RigthToCenter"].speed = -1f;
            transform.GetChild(m_state2).GetComponent<Animation>()["RigthToCenter"].time = transform.GetChild(0).GetComponent<Animation>()["RigthToCenter"].length;
            transform.GetChild(m_state2).GetComponent<Animation>().Play("RigthToCenter");

            transform.GetChild(m_state).GetComponent<Animation>()["CenterToLeft"].speed = -1f;
            transform.GetChild(m_state).GetComponent<Animation>()["CenterToLeft"].time = transform.GetChild(1).GetComponent<Animation>()["CenterToLeft"].length;
            transform.GetChild(m_state).GetComponent<Animation>().Play("CenterToLeft");

            transform.GetChild(m_state1).GetComponent<Animation>()["LeftToRigth"].speed = -1f;
            transform.GetChild(m_state1).GetComponent<Animation>()["LeftToRigth"].time = transform.GetChild(2).GetComponent<Animation>()["LeftToRigth"].length;
            transform.GetChild(m_state1).GetComponent<Animation>().Play("LeftToRigth");
        }
            
	}
}
