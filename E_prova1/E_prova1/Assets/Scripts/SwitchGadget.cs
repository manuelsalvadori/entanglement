using UnityEngine;
using System.Collections;

public class SwitchGadget : MonoBehaviour
{

    public int m_state = 1;
    bool m_in;

    void Start()
    {
        resetState();
        initSelectionUI();
    }

	void Update ()
    {
        if (Input.GetButtonDown("Next") && !transform.GetChild(0).GetComponent<Animation>().isPlaying)
        {
            m_state++;
            m_state = (m_state > 2) ? 0 : m_state;
            if(m_state != 1)
                switchMirino(false);

            transform.GetChild(m_state).GetComponent<Animation>()["RigthToCenter"].speed = 1f;
            transform.GetChild(m_state).GetComponent<Animation>()["RigthToCenter"].time = 0f;
            transform.GetChild(m_state).GetComponent<Animation>().Play("RigthToCenter");

            transform.GetChild((m_state + 2) % 3).GetComponent<Animation>()["CenterToLeft"].speed = 1f;
            transform.GetChild((m_state + 2) % 3).GetComponent<Animation>()["CenterToLeft"].time = 0f;
            transform.GetChild((m_state + 2) % 3).GetComponent<Animation>().Play("CenterToLeft");

            transform.GetChild((m_state + 1) % 3).GetComponent<Animation>()["LeftToRigth"].speed = 1f;
            transform.GetChild((m_state + 1) % 3).GetComponent<Animation>()["LeftToRigth"].time = 0f;
            transform.GetChild((m_state + 1) % 3).GetComponent<Animation>().Play("LeftToRigth");
        }

        if (Input.GetButtonDown("Previous") && !transform.GetChild(0).GetComponent<Animation>().isPlaying)
        {
            m_state--;
            m_state = (m_state < 0) ? 2 : m_state;
            if(m_state != 1)
                switchMirino(false);
            
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

    private void resetState()
    {
        transform.GetChild(m_state).GetComponent<RectTransform>().localScale = new Vector3(1f ,1f, 1f);
        transform.GetChild(m_state).GetComponent<RectTransform>().anchoredPosition = new Vector2(-50f, 50f);

        transform.GetChild((m_state + 1) % 3).GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
        transform.GetChild((m_state + 1) % 3).GetComponent<RectTransform>().anchoredPosition = new Vector2(30f, 40f);

        transform.GetChild((m_state + 2) % 3).GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
        transform.GetChild((m_state + 2) % 3).GetComponent<RectTransform>().anchoredPosition = new Vector2(-130f, 44f);
    }

    private void initSelectionUI()
    {
        if (gameObject.name.Equals("GadgetSelection_1"))
        {
            if (GameManager.Instance.m_sel_pg)
            {
                GetComponent<Animation>().Play("gs1_in");
                m_in = true;
            }
            else
            {
                m_in = false;
            }
        }
        
        if (gameObject.name.Equals("GadgetSelection_2"))
        {
            if (!GameManager.Instance.m_sel_pg)
            {
                GetComponent<Animation>().Play("gs2_in");
                m_in = true;

            }
            else
            {
                m_in = false;
            }
        }
    }

    public void switchSelectionUI()
    {
        if (gameObject.name.Equals("GadgetSelection_1"))
        {
            if (m_in)
            {
                GetComponent<Animation>().Play("gs1_out");
            }
            else
            {
                GetComponent<Animation>().Play("gs1_in");
            }
        }
        else
        {
            if (m_in)
            {
                GetComponent<Animation>().Play("gs2_out");
            }
            else
            {
                GetComponent<Animation>().Play("gs2_in");
            }
        }
        m_in = !m_in;
    }

    public void hideSelectionUI()
    {
        if (m_in)
        {
            if (gameObject.name.Equals("GadgetSelection_1"))
                GetComponent<Animation>().Play("gs1_out");
            else
                GetComponent<Animation>().Play("gs2_out");
        }
    }

    public void unhideSelectionUI()
    {
        if (m_in)
        {
            if (gameObject.name.Equals("GadgetSelection_1"))
                GetComponent<Animation>().Play("gs1_in");
            else
                GetComponent<Animation>().Play("gs2_in");
        }
    }

    void switchMirino(bool enabled)
    {
        GameManager.Instance.mirino.SetActive(enabled);
        if(enabled)
            GameManager.Instance.mirino.GetComponent<Pointing>().resetPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
    }
}
