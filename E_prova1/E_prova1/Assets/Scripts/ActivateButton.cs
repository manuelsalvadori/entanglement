using UnityEngine;
using System.Collections;

public class ActivateButton : MonoBehaviour {


    public bool m_isActive = false;
    public bool m_isSwitch = false;
    public GameObject square;
    public GameObject tastoi;
    public Material red, blue;
    public GameObject screen;


    void Start()
    {
        screen.GetComponent<Renderer>().material = red;
    }

	void Update ()
    {
        
        if ((transform.position - (GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1]).transform.position).magnitude < 3f)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (m_isSwitch)
                    m_isActive = !m_isActive;
                else
                {
                    m_isActive = true;
                    //displayDeactive();
                }
                if (m_isActive)
                    screen.GetComponent<Renderer>().material = blue;
                else
                    screen.GetComponent<Renderer>().material = red;
                
            }
        }
        else
        {
            //displayDeactive();
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
            tastoi.SetActive(true);
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
            tastoi.SetActive(false);
        }
    }
}
