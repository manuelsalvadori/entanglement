using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openOutdoor : MonoBehaviour
{
    public ActivateButton button;
    bool isOpen = false;
    bool opened = true;

    void Update ()
    {
        if (button.m_isActive == true && opened)
        {
            StartCoroutine(openClose());
            opened = false;
        }

    }

    private bool isOpening = false;
    private float durations = 0.35f;
    IEnumerator openClose()
    {
        if (isOpening)
        {
            yield break;
        }
        isOpening = true;

        Vector3 currentpos = transform.position;

        float counter = 0;
        while (counter < durations)
        {
            counter += Time.deltaTime;
            if (isOpen)
            {
                transform.position = Vector3.Lerp(currentpos, currentpos + new Vector3(0f, 6.3f, 0f), counter / durations);
            }
            else
            {
                transform.position = Vector3.Lerp(currentpos, currentpos - new Vector3(0f, 6.3f, 0f), counter / durations);

            }
            yield return null;
        }
        isOpen = !isOpen;
        yield return new WaitForSeconds(0.15f);

        isOpening = false;
    }
}
