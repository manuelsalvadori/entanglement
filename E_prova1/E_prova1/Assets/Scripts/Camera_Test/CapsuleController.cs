using UnityEngine;
using System.Collections;

public class CapsuleController : MonoBehaviour
{

    [Range(10f, 200f)]
    public float m_speed = 50f;

    [Range(10f, 200f)]
    public float m_jump_quantity = 50f;

    public GameObject end;
    public GameObject begin;

    public float percentage = 0;

    private float m_jump = 0f;

    private Rigidbody rb;

    public float getPercentage()
    {
        return percentage;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>() as Rigidbody;
        percentage = (transform.position - begin.transform.position).magnitude / (end.transform.position - begin.transform.position).magnitude;
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
        percentage = (transform.position - begin.transform.position).magnitude / (end.transform.position - begin.transform.position).magnitude;
        m_jump = 0f;

    }
}

