using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointActivation : MonoBehaviour
{
    public Material lightOnBlue, lightOnRed;
    public GameObject lb1,lb2, lr1, lr2;
    public float duration = 0.4f;
    bool activeBlue = false;
    bool activeRed = false;

    void OnTriggerEnter(Collider o)
    {
        if (GameManager.Instance.m_3D_mode)
        {
            GetComponent<AudioSource>().Play();
            lb1.GetComponent<Renderer>().material = lightOnBlue;
            lb2.GetComponent<Renderer>().material = lightOnBlue;
            lr1.GetComponent<Renderer>().material = lightOnRed;
            lr2.GetComponent<Renderer>().material = lightOnRed;
            StartCoroutine(rotate3d());
        }
        else
        {
            if (GameManager.Instance.m_sel_pg && !activeBlue)
            {
                GetComponent<AudioSource>().Play();
                lb1.GetComponent<Renderer>().material = lightOnBlue;
                lb2.GetComponent<Renderer>().material = lightOnBlue;
                StartCoroutine(rotateActive(GameManager.Instance.m_sel_pg));
            }
            if (!GameManager.Instance.m_sel_pg && !activeRed)
            {
                GetComponent<AudioSource>().Play();
                lr1.GetComponent<Renderer>().material = lightOnRed;
                lr2.GetComponent<Renderer>().material = lightOnRed;
                StartCoroutine(rotateActive(GameManager.Instance.m_sel_pg));
            }
        }
    }

    IEnumerator rotateActive(bool pg)
    {
        GameObject go;
        if (pg)
            go = lb1;
        else
            go = lr1;
        
        float startRotation = go.transform.eulerAngles.x;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            go.transform.localRotation = Quaternion.Euler(new Vector3(yRotation, 90f, 90f));

            yield return null;
        }
        if (pg)
            activeBlue = true;
        else
            activeRed = true;

        if (activeRed && activeBlue)
            Destroy(this);
    }

    IEnumerator rotate3d()
    {
        float startRotation = lb1.transform.eulerAngles.x;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            lb1.transform.localRotation = Quaternion.Euler(new Vector3(yRotation, 90f, 90f));
            lr1.transform.localRotation = Quaternion.Euler(new Vector3(yRotation, 90f, 90f));

            yield return null;
        }
            Destroy(this);
    }
}
