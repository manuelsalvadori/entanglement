using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    public Transform m_endPosition;
    public Transform m_initPosition;
    public ActivateButton m_button;
    private Vector3 target = Vector3.zero;
    private Vector3 velocity;
    private bool active = true, switched = true;
    public float smoothTime = 1f;

	// Use this for initialization
	void Awake ()
    {
        target = m_endPosition.position;
	}

	void FixedUpdate ()
    {
<<<<<<< HEAD
        active = m_button.m_isActive;
		if (m_button == null)
            active = true;
	    else
			active = m_button.m_isActive;
		
=======
        if (m_button == null)
            active = m_button.m_isActive;
        else
            active = true;
        
>>>>>>> f0e9785139520979caa8b155468c6707127d9808
        if ((transform.position - target).magnitude < 0.3f)
        {
            if (switched)
            {
                target = m_initPosition.position;
                switched = !switched;
            }
            else
            {
                target = m_endPosition.position;
                switched = !switched;
            }
        }

        if(active)
            this.gameObject.GetComponent<Rigidbody>().MovePosition(Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime));
	}
}
