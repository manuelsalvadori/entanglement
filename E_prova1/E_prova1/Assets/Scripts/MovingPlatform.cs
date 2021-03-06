﻿using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    public Transform m_endPosition;
    public Transform m_initPosition;
    public ActivateButton m_button;
    private Vector3 target = Vector3.zero;
    private Vector3 velocity;
    public bool active = true;
    private bool switched = true;
    public float smoothTime = 1f;

	// Use this for initialization
	void Awake ()
    {
        target = m_endPosition.position;
	}

	void Update ()
    {
        if (m_button != null)
        {
            //Debug.Log("bottone!");
            active = m_button.m_isActive;
        }

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
           transform.position = (Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime));
	}
}
