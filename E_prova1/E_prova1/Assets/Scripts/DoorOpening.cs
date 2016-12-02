using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    public int m_door_number;
	public Transform m_endPosition;
	public Transform m_initPosition;
    public ActivateButton[] m_button;
	private Vector3 target = Vector3.zero;
	private Vector3 velocity;
	public bool active = true;
	public float smoothTime = 1f;

	// Use this for initialization
	void Awake ()
	{
		target = m_endPosition.position;
	}

	void FixedUpdate ()
	{
        switch (m_door_number)
        {
            case 1:
                active = m_button[1].m_isActive || m_button[3].m_isActive;
                break;
            case 2:
                active = m_button[2].m_isActive || m_button[3].m_isActive || m_button[6].m_isActive;
                break;
            case 3:
                active = m_button[4].m_isActive || m_button[5].m_isActive;
                break;
            case 4:
                active = m_button[2].m_isActive || m_button[4].m_isActive;
                break;
            case 5:
                active = m_button[1].m_isActive || m_button[5].m_isActive;
                break;
            case 6:
                active = m_button[1].m_isActive || m_button[2].m_isActive || m_button[3].m_isActive || m_button[6].m_isActive;
                break;
            default:
                return;
        }

		if ((transform.position - target).magnitude < 0.3f)
		{
			if (active ==false)
			{
				target = m_initPosition.position;
			}
			else
			{
				target = m_endPosition.position;
			}
		}

		if(!transform.position.Equals(target))
			this.gameObject.GetComponent<Rigidbody>().MovePosition(Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime));
	}
}
