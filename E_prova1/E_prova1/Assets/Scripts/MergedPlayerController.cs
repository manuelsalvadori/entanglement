using UnityEngine;
using System.Collections;

public class MergedPlayerController : MonoBehaviour
{
    public float m_v, m_h, m_force = 10f, m_jump = 10f, m_GravityMultiplier = 3f;
    public float m_Zfixed = -4.6f;
    public float smoothTime = 0.3F;                                     //Amount of smooth
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
        
                m_grounded = (Mathf.Abs(m_rb.velocity.y) < 0.005f);
                m_v = Input.GetAxis("Horizontal");
                m_h = Input.GetAxis("Vertical");

                if (GameManager.Instance.m_camIsMoving) //Durante la transizione da una modalità di camera all'altra, i movimenti sono disabilitati
                    m_h = m_v = 0f;

                Quaternion m_look = transform.rotation;
                Vector3 move = new Vector3(m_v, 0f, m_h);
                if (move.magnitude > 1)
                    move = move.normalized;
                Vector3 force = cam.transform.TransformDirection(move * m_force);
                force.y = 0f;
                m_rb.AddForce(force);
            
                if (Mathf.Abs(m_rb.velocity.z) > m_velocity_boundary)
                    m_rb.velocity = new Vector3(m_rb.velocity.x, m_rb.velocity.y, Mathf.Clamp(m_rb.velocity.z, -(m_velocity_boundary), m_velocity_boundary));

                if (Mathf.Abs(m_rb.velocity.x) > m_velocity_boundary)
                    m_rb.velocity = new Vector3(Mathf.Clamp(m_rb.velocity.x, -(m_velocity_boundary), m_velocity_boundary), m_rb.velocity.y, m_rb.velocity.z);

                if ((!Input.GetButton("L2") && Input.GetButtonDown("Jump")) && m_grounded)
                {
                    m_rb.velocity = new Vector3(m_rb.velocity.x, m_jump, m_rb.velocity.z);
                }

                if (m_rb.velocity.y < 0)
                {
                    Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
                    m_rb.AddForce(extraGravityForce);
                }
            

        if (!GameManager.Instance.m_3D_mode)
        {
            m_rb.Sleep();
        }
        else
        {
            m_rb.WakeUp();
        }
	}

    void LateUpdate()
    {
        if (!GameManager.Instance.m_3D_mode && !GameManager.Instance.m_camIsMoving)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, m_Zfixed);
        }
    }

}
