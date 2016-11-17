using UnityEngine;
using System.Collections;

//This script controls Y Motion of a gameobject that is controlled by another one by a Joint.
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
        //Jump
        if ((!Input.GetButton("L2") && Input.GetButtonDown("Jump")) && m_grounded)
        {
            m_rb.velocity = new Vector3(m_rb.velocity.x, m_jump, m_rb.velocity.z);
        }

        //Increase gravity in order to fall faster
        if (m_rb.velocity.y < 0)
        {
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_rb.AddForce(extraGravityForce);
        }
    }


    
    public void OnCollisionStay(Collision other)
    {
        //If this gameobject is touching the "Ground" it can jump
        if (other.gameObject.tag.Equals("Ground"))
            m_grounded = true;
    }

    
    public void OnCollisionExit(Collision other)
    {
        //If this gameobject is not touching anymore the "Ground" it can't jump
        if (other.gameObject.tag.Equals("Ground"))
            m_grounded = false;
    }
}
