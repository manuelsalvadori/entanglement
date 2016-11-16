using UnityEngine;
using System.Collections;

public class ActivateButton : MonoBehaviour {


    public bool m_isActive = false;

	void Update ()
    {
        if ((transform.position - (GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1]).transform.position).magnitude < 5f)
        {
            displayActive();

            if (Input.GetButtonDown("Interact"))
            {
                m_isActive = true;
            }
        }
	}

    void displayActive()
    {
        //facciamo apparire una UI
    }
}
