﻿using UnityEngine;
using System.Collections;

public class PlayerController2 : MonoBehaviour
{
    public float m_v, m_h, m_force = 10f, m_jump = 10f, m_GravityMultiplier = 3f;
    public float m_Zfixed = -4.6f;
    public float smoothTime = 0.3F;                                     //Amount of smooth
    Rigidbody m_rb;
    bool m_grounded = true;
    Camera cam;

    void Start ()
    {
        m_rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	

    void FixedUpdate ()
    {
        if (!GameManager.Instance.m_sel_pg || GameManager.Instance.m_3D_mode || GameManager.Instance.m_double_mode)
        {
            m_grounded = (Mathf.Abs(m_rb.velocity.y) < 0.001f);
            m_v = Input.GetAxis("Horizontal");
            m_h = Input.GetAxis("Vertical");

            if (GameManager.Instance.m_camIsMoving) //Durante la transizione da una modalità di camera all'altra, i movimenti sono disabilitati
            m_h = m_v = 0f;
        
            if (Mathf.Abs(m_rb.velocity.x) < 10f && Mathf.Abs(m_rb.velocity.y) < 10f) //se la velocity è troppo alta smettiamo di aggiungere forza
            {
                Quaternion m_look = transform.rotation;
                Vector3 move = new Vector3(m_v, 0f, m_h);
                if (move.magnitude > 1)
                    move = move.normalized;
                Vector3 force = cam.transform.TransformDirection(move * m_force);
                force.y = 0f;
                m_rb.AddForce(force);

                if (force.magnitude != 0)
                    m_look = Quaternion.LookRotation(force.normalized);
            
                StartCoroutine(rotatePlayer(m_look, 0.1f));
            }

            if ((!Input.GetButton("L2") && Input.GetButtonDown("Jump")) && m_grounded)
            {
                m_rb.velocity = new Vector3(m_rb.velocity.x, m_jump, m_rb.velocity.z);
            }

            if (m_rb.velocity.y < 0)
            {
                Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
                m_rb.AddForce(extraGravityForce);
            }
        }
	}

    void LateUpdate()
    {
        if (!GameManager.Instance.m_3D_mode && !GameManager.Instance.m_camIsMoving)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, m_Zfixed);
        }
    }

    private bool rotating = false;
    IEnumerator rotatePlayer(Quaternion newRot, float duration)
    {
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Quaternion currentRot = transform.rotation;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
        rotating = false;
    }
}