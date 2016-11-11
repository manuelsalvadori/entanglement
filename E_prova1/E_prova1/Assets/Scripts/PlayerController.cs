using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float m_v, m_h, m_force = 10f, m_jump = 10f, m_GravityMultiplier = 3f;
    public float m_Zfixed = -4.6f;
    Rigidbody m_rb;
    bool m_grounded = true;
    Camera cam;

	// Use this for initialization
	void Start ()
    {
        m_rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        m_grounded = m_rb.velocity.y == 0;

        m_v = Input.GetAxis("Horizontal");
        m_h = Input.GetAxis("Vertical");
           
        if (Mathf.Abs(m_rb.velocity.x) < 10f && Mathf.Abs(m_rb.velocity.y) < 10f)
        {
            float cam_rot = cam.transform.rotation.eulerAngles.y;
            cam_rot = (cam_rot < 0f) ? 360f + cam_rot : cam_rot;

            Debug.Log("cam_rot: " + cam_rot);
            Vector3 force = cam.transform.TransformDirection(new Vector3(m_v * m_force, 0f, m_h * m_force));
            force.y = 0f;
            m_rb.AddForce(force);
       
        }

        if (Input.GetKeyDown("b") && m_grounded)
        {
            m_rb.velocity = new Vector3(m_rb.velocity.x, m_jump, m_rb.velocity.z);
        }

        if (m_rb.velocity.y < 0)
        {
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_rb.AddForce(extraGravityForce);
        }
        Debug.Log(m_rb.velocity.x.ToString());
	}

    void LateUpdate()
    {
        if (!GameManager.Instance.m_3D_mode)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, m_Zfixed);
        }
    }
}
