using UnityEngine;
using System.Collections;

public class SlaveController : MonoBehaviour
{
    public float m_v, m_h, m_force = 10f, m_jump = 10f, m_GravityMultiplier = 3f;
    public float m_Zfixed = -4.6f;
    public float smoothTime = 0.3F;                                     //Amount of smooth

    public float m_velocity_boundary = 10f;

    bool m_grounded = true;
    Camera cam;

    void Start ()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
        

    void FixedUpdate ()
    {

        m_v = Input.GetAxis("Horizontal");
        m_h = Input.GetAxis("Vertical");

        Quaternion m_look = transform.rotation;
        Vector3 move = new Vector3(m_v, 0f, m_h);
        if (move.magnitude > 1)
            move = move.normalized;
        Vector3 force = cam.transform.TransformDirection(move * m_force);
        force.y = 0f;
                  
        if (force.magnitude != 0)
            m_look = Quaternion.LookRotation(force.normalized);
        if(GameManager.Instance.m_3D_mode)
            StartCoroutine(rotatePlayer(m_look, 0.1f));
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
