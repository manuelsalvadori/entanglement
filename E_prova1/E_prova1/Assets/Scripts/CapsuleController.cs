using UnityEngine;
using System.Collections;

public class CapsuleController : MonoBehaviour
{

    [Range(10f, 200f)]
    public float m_speed = 50f;

    [Range(10f, 200f)]
    public float m_jump_quantity = 50f;

    private float m_jump = 0f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>() as Rigidbody;
    }


    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_jump = m_jump_quantity;
        }

        float m_horizontal = Input.GetAxis("Horizontal");
        float m_vertical = Input.GetAxis("Vertical");
        rb.AddForce(new Vector3(-m_horizontal * m_speed, m_jump * m_speed, -m_vertical * m_speed));
        m_jump = 0f;
    }
}

