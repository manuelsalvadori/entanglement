using UnityEngine;
using System.Collections;

public class ActivateButton : MonoBehaviour {


    public bool m_isActive = false;
    public bool m_isSwitch = false;
    public GameObject square;

	void Update ()
    {
        
        if ((transform.position - (GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1]).transform.position).magnitude < 4f)
        {
            if(!(!m_isSwitch && m_isActive))
                displayActive();

            if (Input.GetButtonDown("Interact"))
            {
                if (m_isSwitch)
                    m_isActive = !m_isActive;
                else
                {
                    m_isActive = true;
                    displayDeactive();
                }
            }
        }
        else
        {
            square.SetActive(false);
        }

	}

    void displayActive()
    {
        if (/*Input.GetJoystickNames()[0].Equals("Sony PLAYSTATION(R)3 Controller")*/  Input.GetJoystickNames().Length > 0)
        {
            square.SetActive(true);
        }
        else
        {
            Debug.Log("keyboard");
        }
    }

    void displayDeactive()
    {
        if (/*Input.GetJoystickNames()[0].Equals("Sony PLAYSTATION(R)3 Controller")*/  Input.GetJoystickNames().Length > 0)
        {
            square.SetActive(false);
        }
        else
        {
            Debug.Log("keyboard");
        }
    }
}
