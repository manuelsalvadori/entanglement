using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour
{
    public GameObject m_l1;
    public GameObject m_l2;
    bool m_selLevel= true;
    private bool s1 = false;
    private bool s2 = false;
    Vector3 end;
    Vector3 start;
    public float m_speed = 5f;
    private float start_time;
    private float length;



	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("x"))
        {
            SwitchDouble(m_selLevel);

        }

        if (Input.GetKeyDown("c"))
        {
            SwitchOne(m_selLevel);
        }
        if (s2)
        {
            float currentPos = (Time.time - start_time) * m_speed;
            float fract = currentPos / length;
            transform.position = Vector3.Lerp(start, end, fract);
            Debug.Log(transform.position.ToString() + " currentpos " + currentPos + " fract " + fract + " lenght " + length.ToString());
            if (transform.position.Equals(end))
            {
                m_selLevel = !m_selLevel;
                s2 = false;
            }
        }
	}

    void SwitchDouble(bool selected)
    {
        
    }

    void SwitchOne(bool selected)
    {
        start = transform.position;
        start_time = Time.time;
        end = transform.position;
        if (selected)
        {
            start = new Vector3(start.x, m_l1.transform.position.y, start.z);
            Debug.Log("prima transform = "+start.ToString() + " end=" + end.ToString());

            end = new Vector3(end.x, m_l2.transform.position.y, end.z);
        }
        else
        {
            start = new Vector3(start.x, m_l2.transform.position.y, start.z);
            end = new Vector3(end.x, m_l1.transform.position.y, end.z);
        }

        //length = Vector3.Distance(start.position, end.position);
        Debug.Log("mezz transform = "+start.ToString() + " end=" + end.ToString() + "length = "+length);

        Vector3 dist = start - end;
        length = dist.magnitude;
        Debug.Log("dopo transform = "+start.ToString() + " end=" + end.ToString() + "length = "+length);
        s2 = true;
    }
}
