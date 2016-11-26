using UnityEngine;
using System.Collections;

public class shoot : MonoBehaviour
{
    public float m_speed = 6f;
    public RaycastHit hit;
    LineRenderer lr;

	void Start ()
    {
        resetShootPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
	}
	
	void Update ()
    {
        if (SwitchGadget.Instance.m_state == 0)
        {
            lr.enabled = true;
            if (GameManager.Instance.m_3D_mode)
            {
                Debug.Log(Input.GetAxis("R_Vertical"));
                transform.position = transform.position + new Vector3(0f, Input.GetAxis("R_Vertical") * Time.deltaTime * m_speed, -Input.GetAxis("R_Horizontal") * Time.deltaTime * m_speed);
            }
            else
            {
                transform.position = transform.position + new Vector3(0f, Input.GetAxis("R_Vertical") * Time.deltaTime * m_speed, 0f);
            }
            float clamped_y = Mathf.Clamp(transform.position.y, GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.y - 0.5f, GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.y + 5f);
            float dir = (GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.rotation.eulerAngles.y >= 0f  && GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.rotation.eulerAngles.y < 180f) ? GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.position.x + 1.0f : GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.position.x - 1.0f;
           

            transform.position = new Vector3(dir, clamped_y, transform.position.z);

            Vector3 start = GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position;
            Vector3 diff = transform.position - start;
            Ray raggio = new Ray(start, diff);
            Physics.Raycast(raggio, out hit);
            if (!hit.Equals(null))
            {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, hit.point);
            }
        }
        else
        {
            lr.enabled = false;
        }

	}

    public void resetShootPosition(Vector3 posplayer)
    {
        float dir = (GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.rotation.eulerAngles.y >= 0f && GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.rotation.eulerAngles.y < 180f) ? 1.0f : -1.0f;
        transform.position = posplayer + new Vector3(dir,0f,0f);
    }
}
