using UnityEngine;
using System.Collections;

public class JumpController : MonoBehaviour
{
    public float m_jump = 20f, m_GravityMultiplier = 3f;
    Rigidbody m_rb;

    public float m_velocity_boundary = 10f;

    bool m_grounded = true;
    Camera cam;

    void Start ()
    {
        m_rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
        
    void FixedUpdate ()
    {
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
