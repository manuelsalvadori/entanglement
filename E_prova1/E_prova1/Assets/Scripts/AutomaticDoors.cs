﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoors : MonoBehaviour
{
    public Transform door_left, door_right, door_key;
    public Transform player;
    public float distance = 2f;
    bool isclosed = true;

    void Update ()
    {
        if (Mathf.Abs((player.position - transform.position).magnitude) < distance && isclosed)
        {
            StartCoroutine(openClose(true));
            isclosed = false;
        }

        if (Mathf.Abs((player.position - transform.position).magnitude) >= distance && !isclosed)
        {
            StartCoroutine(openClose(false));
        }

    }

    private bool isOpening = false;
    private float durations = 0.15f;
    IEnumerator openClose(bool isOpen)
    {
        if (isOpening)
        {
            yield break;
        }
        isOpening = true;

        Vector3 currentpos = door_right.localPosition;
        Vector3 currentpos2 = door_left.localPosition;
		Vector3 currentpos3 = door_key.localPosition;

        float counter = 0;
        while (counter < durations)
        {
            counter += Time.deltaTime;
            if (isOpen)
            {
                door_right.localPosition = Vector3.Lerp(currentpos, currentpos + new Vector3(1.85f, 0f, 0f), counter / durations);
                door_left.localPosition = Vector3.Lerp(currentpos2, currentpos2 - new Vector3(1.85f, 0f, 0f), counter / durations);
				door_key.localPosition = Vector3.Lerp(currentpos3, currentpos3 - new Vector3(1.85f, 0f, 0f), counter / durations);
            }
            else
            {
                door_right.localPosition = Vector3.Lerp(currentpos, currentpos - new Vector3(1.85f, 0f, 0f), counter / durations);
                door_left.localPosition = Vector3.Lerp(currentpos2, currentpos2 + new Vector3(1.85f, 0f, 0f), counter / durations);
				door_key.localPosition = Vector3.Lerp(currentpos3, currentpos3 + new Vector3(1.85f, 0f, 0f), counter / durations);
            }
            yield return null;
        }
        isOpen = !isOpen;
        yield return new WaitForSeconds(0.15f);

        isOpening = false;
        if(isOpen)
            isclosed = true;

    }
}
