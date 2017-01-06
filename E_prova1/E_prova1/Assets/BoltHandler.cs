using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltHandler : MonoBehaviour {

    private GameObject Left;
    private GameObject Right;




    // Use this for initialization
    void Start () {
        Left = transform.GetChild(0).gameObject;
        Right = transform.GetChild(1).gameObject;
    }

	// Update is called once per frame
	void Update () {
        float tmp = Random.Range(0, 100);
        if(tmp < 2f)
        {
            float tmp2 = Random.Range(0, 100);
            if (tmp2 < 30)
            {
                GameObject[] gg = new GameObject[1];
                gg[0] = Left;
                StartCoroutine(zippa(gg));
            }
            else if(tmp2 >= 30 && tmp2 < 60)
            {
                GameObject[] gg = new GameObject[1];
                gg[0] = Right;
                StartCoroutine(zippa(gg));
            }
            else
            {
                GameObject[] gg = new GameObject[2];
                gg[0] = Right;
                gg[1] = Left;
                StartCoroutine(zippa(gg));
            }

        }
	}

    IEnumerator zippa(GameObject[] who)
    {
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject g in who) g.GetComponent<LineRenderer>().enabled = true;
        yield return new WaitForSeconds(Random.Range(0.2f, 0.8f));
        foreach (GameObject g in who) g.GetComponent<LineRenderer>().enabled = false;
    }

}
