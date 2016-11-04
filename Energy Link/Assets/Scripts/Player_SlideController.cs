using UnityEngine;
using System.Collections;

public class Player_SlideController : MonoBehaviour {

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
        rb.AddForce(new Vector3(0f, m_jump * m_speed, m_horizontal * m_speed));
        m_jump = 0f;
    }
}
