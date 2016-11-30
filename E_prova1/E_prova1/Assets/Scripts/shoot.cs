using UnityEngine;
using System.Collections;

public class shoot : MonoBehaviour
{
    public float m_speed = 20f;
    public RaycastHit hit;
    LineRenderer lr;

	void Start ()
    {
        resetShootPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
	}
	
    float curr_dir = 1.0f;
    bool right = true;

	void Update ()
    {
        transform.position = GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position;
        float m_h = Input.GetAxis("R_Horizontal") * Time.deltaTime * m_speed;
        float m_v = (right) ? Input.GetAxis("R_Vertical") * Time.deltaTime * m_speed : -Input.GetAxis("R_Vertical") * Time.deltaTime * m_speed;
        if (SwitchGadget.Instance.m_state == 0)
        {
            lr.enabled = true;
            if (GameManager.Instance.m_3D_mode)
            {
                transform.Rotate(new Vector3(0f,m_h, m_v));
            }
            else
            {
                transform.Rotate(new Vector3(0f, 0f, m_v));
            }
            float rotation_z = transform.rotation.eulerAngles.z; 
            float dir = (GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.rotation.eulerAngles.y >= 0f  && GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.rotation.eulerAngles.y < 180f)
                ?  1: -1f;
            if (curr_dir * dir < 0)
            {
                curr_dir = dir;
                rotation_z = 360-transform.rotation.eulerAngles.z;
                right = !right;
            }
            else
            {
                rotation_z = transform.rotation.eulerAngles.z;
            }

            float clamped_z = (GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.rotation.eulerAngles.y >= 0f  && GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.rotation.eulerAngles.y < 180f)
                ?  Mathf.Clamp(rotation_z, 65f, 170f): Mathf.Clamp(rotation_z, 190f, 295f);

            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, clamped_z));


            Vector3 start = GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position;
            Vector3 diff = transform.GetChild(0).position - start;
            Ray raggio = new Ray(start, diff);
            Physics.Raycast(raggio, out hit);
            if (!hit.Equals(null))
            {
                lr.SetPosition(0, transform.GetChild(0).transform.position);
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
        transform.rotation.eulerAngles.Set(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, dir * transform.rotation.eulerAngles.z);
        transform.position = posplayer;
    }
}
