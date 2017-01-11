using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class downplatform : MonoBehaviour {

    public ActivateButton button1;
    public ActivateButton button2;
    public ActivateButton button3;

    bool isOpen = false;
    bool opened = true;

    void Update ()
    {
        if (!button1.m_isActive == true && !button2.m_isActive == true && button3.m_isActive == true && opened)
        {
            StartCoroutine(openClose());
            opened = false;
        }

    }

    private bool isOpening = false;
    private float durations = 1.75f;
    IEnumerator openClose()
    {
        if (isOpening)
        {
            yield break;
        }
        isOpening = true;


        yield return new WaitForSeconds(0.25f);

        Vector3 currentpos = transform.position;

        float counter = 0;
        while (counter < durations)
        {
            counter += Time.deltaTime;
            if (isOpen)
            {
                transform.position = Vector3.Lerp(currentpos, new Vector3(currentpos.x, 24.275f, currentpos.z), counter / durations);
            }
            else
            {
                transform.position = Vector3.Lerp(currentpos, new Vector3(currentpos.x, 24.275f, currentpos.z), counter / durations);

            }
            yield return null;
        }
        isOpen = !isOpen;
        yield return new WaitForSeconds(0.15f);

        isOpening = false;
    }
}
