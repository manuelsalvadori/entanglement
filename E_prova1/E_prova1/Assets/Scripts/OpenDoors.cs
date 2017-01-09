using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoors : MonoBehaviour
{
    public Transform door_left, door_right;
    public ActivateButton button;
    bool isOpen = false;
    bool preActive = true;

	void Update ()
    {
        if (button.m_isActive == preActive)
        {
            StartCoroutine(openClose());
            preActive = !preActive;
        }
		
	}

    private bool isOpening = false;
    private float durations = 0.15f;
    IEnumerator openClose()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
        if (isOpening)
        {
            yield break;
        }
        isOpening = true;

        Vector3 currentpos = door_right.position;
        Vector3 currentpos2 = door_left.position;

        float counter = 0;
        while (counter < durations)
        {
            counter += Time.deltaTime;
            if (isOpen)
            {
                door_right.position = Vector3.Lerp(currentpos, currentpos + new Vector3(0f, 0f, 1.85f), counter / durations);
                door_left.position = Vector3.Lerp(currentpos2, currentpos2 - new Vector3(0f, 0f, 2.15f), counter / durations);
            }
            else
            {
                door_right.position = Vector3.Lerp(currentpos, currentpos - new Vector3(0f, 0f, 1.85f), counter / durations);
                door_left.position = Vector3.Lerp(currentpos2, currentpos2 + new Vector3(0f,0f, 2.15f), counter / durations);

            }
            yield return null;
        }
        isOpen = !isOpen;
        yield return new WaitForSeconds(0.15f);

        isOpening = false;
    }
}
