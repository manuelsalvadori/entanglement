using UnityEngine;
using System.Collections;

public class SmoothSwitch : MonoBehaviour {

    Vector3 target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 velocity2 = Vector3.zero;
    private Vector3 velocity3 = Vector3.zero;
    private Vector3 rot = Vector3.zero;
    private Vector3 l3d1, l3df, l2d, ltarget;
    private bool s1 = false, l = true, x = true, tred_mode = false;
    public float m_zDouble = -29f;
    public float m_zSingle = -20f;
    public float m_y3D = 20f;
    public float m_l1z = 10.5f;
    public float m_l1y = -5f;
    public Transform m_l1, m_l2;
    bool t = true, s = false;

    void Start()
    {
        target = transform.position;
        l2d = m_l1.transform.position;
        l3d1 = m_l1.position;
        l3d1.z = m_l1z;
        l3df = m_l1.position;
        l3df.z = m_l1z;
        l3df.y = -5.0f;
        ltarget = m_l1.position;

    }

    void Update()
    {
        if(Input.GetKeyDown("x") && !tred_mode)
        {
            if (x)
            {
                target.y = 0f;
                target.z = m_zDouble;
                x = false;
            }
            else
            {
                target.z = m_zSingle;
                target.y = l ? m_l1.position.y : m_l2.position.y;
                x = true;
            }
            s1 = true;
        }

        if (Input.GetKeyDown("c") && !tred_mode)
        {
            if (l)
            {
                target.y = m_l2.position.y;
                l = false;
            }
            else
            {
                target.y = m_l1.position.y;
                l = true;
            }
            target.z = m_zSingle;
            s1 = true;
        }

        if (Input.GetKeyDown("z"))
        {
            s = true;
            if (!tred_mode)
            {
                target.y = m_y3D;
                target.z = 0f;
                rot = new Vector3(30f, 90f, 0f);
                ltarget = l3d1;
                t = true;
            }
            else
            {
                target.y = 0f;
                target.z = m_zDouble;
                rot = Vector3.zero;
                ltarget = l2d;
            }
            s1 = true;
            tred_mode = !tred_mode;
        }
              
        if (s1)
        {
            if (Mathf.Approximately(m_l1.position.z , ltarget.z) && t && s)
            {
                ltarget = l3df;
                t = false;
                s = false;
            }
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime); 
            transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, rot, ref velocity2, smoothTime));
            m_l1.position = Vector3.SmoothDamp(m_l1.position, ltarget, ref velocity3, (smoothTime/2f));

            s1 = !transform.position.Equals(target);
        }
    }
}
