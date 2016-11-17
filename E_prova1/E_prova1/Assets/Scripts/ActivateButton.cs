using UnityEngine;
using System.Collections;

public class ActivateButton : MonoBehaviour {


    public bool m_isActive = false;
    public bool m_isSwitch = false;

	void Update ()
    {
        
        if ((transform.position - (GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1]).transform.position).magnitude < 5f)
        {
            displayActive();

            if (Input.GetButtonDown("Interact"))
            {
                if (m_isSwitch)
                    m_isActive = !m_isActive;
                else
                    m_isActive = true;
            }
        }
	}

    void displayActive()
    {
        if (/*Input.GetJoystickNames()[0].Equals("Sony PLAYSTATION(R)3 Controller")*/  Input.GetJoystickNames().Length > 0)
        {
            Debug.Log("ps3");
        }
        else
        {
            Debug.Log("keyboard");

        }


    }
}
