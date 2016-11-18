using UnityEngine;
using System.Collections;

public class MovingPlatform2 : MonoBehaviour {

    public Transform m_endPosition;
    public Transform m_initPosition;
    public ActivateButton m_button;
    private Vector3 endPos = Vector3.zero;
    private Vector3 startPos = Vector3.zero;
    private Vector3 velocity;
    private bool active = true, switched = true;
    public float lerpTime = 1f;
    float currentLerpTime= 0f;
    bool avanti = true;
    Transform m_parent;

    // Use this for initialization
    void Awake ()
    {
        startPos = m_initPosition.position;
        endPos = m_endPosition.position;
    }

    void FixedUpdate()
    {
        if (active = m_button.m_isActive)
        { 
            if (avanti)
            {
                currentLerpTime += Time.deltaTime;
                if (currentLerpTime > lerpTime)
                {
                    currentLerpTime = lerpTime;
                    avanti = false;
                }
            }
            else
            {
                currentLerpTime -= Time.deltaTime;
                if (currentLerpTime < 0f)
                {
                    currentLerpTime = 0f;
                    avanti = true;
                }
            }

            //lerp!
            float perc = currentLerpTime / lerpTime;
            perc = perc * perc * perc * (perc * (6f * perc - 15f) + 10f);
            transform.position = Vector3.Lerp(startPos, endPos, perc);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            m_parent = other.transform.parent;
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.transform.parent = transform;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log(other.transform.name);
            other.transform.parent = m_parent;
        }
    }
}
