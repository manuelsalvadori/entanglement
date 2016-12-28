using UnityEngine;
using System.Collections;

public class ActivateButton : MonoBehaviour {

    public bool m_unlocked = false;
    public bool m_isActive = false;
    public bool m_isSwitch = false;
    public float min_distance = 5f;
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

        if (GameManager.Instance.m_Current_State != (int) CoolCameraController.Stato.TreD ? (transform.position - (GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1]).transform.position).magnitude < min_distance : (transform.position - (GameManager.Instance.m_players[0]).transform.position).magnitude < min_distance || (transform.position - (GameManager.Instance.m_players[1]).transform.position).magnitude < min_distance)
        {
            if (Input.GetButtonDown("Interact") && m_unlocked)
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

    public void unlock()
    {
        m_unlocked = true;
    }
}
