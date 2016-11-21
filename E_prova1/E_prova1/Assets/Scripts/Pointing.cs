﻿using UnityEngine;
using System.Collections;

public class Pointing : MonoBehaviour
{
    public float speed = 8f;
    public float[] pos_y = {10f, -2f};
    LineRenderer lr;
    public Material[] mat;
	
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        resetPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
        lr.SetPosition(0, transform.position);
    }

	void Update ()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0:1].transform.position);


        float va = Input.GetAxis("R_Vertical");
        float ha = Input.GetAxis("R_Horizontal");

        RaycastHit hit;
        Vector3 start = new Vector3(transform.position.x, pos_y[(GameManager.Instance.m_sel_pg) ? 0 : 1], transform.position.z);
        Ray raggio = new Ray(start, Vector3.down);
        Physics.Raycast(raggio, out hit);

        if (hit.collider != null)
        {
            transform.position = new Vector3(transform.position.x, (hit.collider.bounds.max.y + gameObject.GetComponent<MeshRenderer>().bounds.extents.y),transform.position.z);
        }

        transform.position += (new Vector3(ha, 0f, va).normalized * Time.deltaTime * speed);

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos = Camera.main.ViewportToWorldPoint(new Vector3(Mathf.Clamp(pos.x, 0.03f, 0.97f), pos.y, pos.z));

        transform.position = new Vector3(pos.x, transform.position.y, -4.6f);
	}

    public void resetPosition(Vector3 pos)
    {
        transform.position = pos + new Vector3(3f,0f,0f);
        lr.material = mat[GameManager.Instance.m_sel_pg ? 0:1];
        GetComponent<Renderer>().material = mat[GameManager.Instance.m_sel_pg ? 0:1];
    }
}
