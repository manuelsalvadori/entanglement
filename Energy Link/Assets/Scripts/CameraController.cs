using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject m_pl1;
    public GameObject m_pl2;

    private Vector3 m_offset;
    private Vector3 distance;
    private Vector3 median;



    // Use this for initialization
    void Start () {
        median = new Vector3((m_pl1.transform.position.x + m_pl2.transform.position.x) / 2, m_pl1.transform.position.y, (m_pl1.transform.position.z + m_pl2.transform.position.z) / 2);
        m_offset = transform.position - median;
    }
	
	// Update is called once per frame
	void Update () {
        median = new Vector3((m_pl1.transform.position.x + m_pl2.transform.position.x) / 2, m_pl1.transform.position.y, (m_pl1.transform.position.z + m_pl2.transform.position.z) / 2);
        transform.position = median + m_offset;  
	}
}
