using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedPlayer : MonoBehaviour {

    public Rigidbody[] p;

    public int sel_pg;

    private Vector3 velocity;

    public float m_Smooth_Time = 2f;

	// Use this for initialization
	void Start () {
        sel_pg = 0;
	}



	// Update is called once per frame
	void LateUpdate () {
        float deltaX = p[0].position.x - p[1].position.x;
        float deltaZ = p[0].position.z - p[1].position.z;

        Debug.Log(p[0].velocity + " " + p[1].velocity + " Delta: "  + deltaX + " " + deltaZ + " Player " + (sel_pg + 1));
        if(Mathf.Abs(deltaX) > 3f || Mathf.Abs(deltaX) < 2.6f )
        {
            p[0].velocity = new Vector3(0f, p[0].velocity.y, p[0].velocity.z);
            p[1].velocity = new Vector3(0f, p[1].velocity.y, p[1].velocity.z);

            p[1 - sel_pg].MovePosition(Vector3.SmoothDamp(p[1 - sel_pg].position, new Vector3(p[sel_pg].position.x + 2.8f * (sel_pg == 0 ? 1 : -1), p[1 - sel_pg].position.y, p[1 - sel_pg].position.z), ref velocity, m_Smooth_Time));

        }
        if (Mathf.Abs(deltaZ) > 0.4f)
        {
            p[0].velocity = new Vector3(p[0].velocity.x, p[0].velocity.y, 0f);
            p[1].velocity = new Vector3(p[1].velocity.x, p[1].velocity.y, 0f);
            p[1 - sel_pg].MovePosition(Vector3.SmoothDamp(p[1 - sel_pg].position, new Vector3(p[1 - sel_pg].position.x, p[1 - sel_pg].position.y, p[sel_pg].position.z), ref velocity, m_Smooth_Time));
        }
    }

}
