using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransporterUltimateActivationPawa : MonoBehaviour
{
    public GameObject mightyLightOfBluishPowa;
    public GameObject mightyLightOfReddishPowa;

    void OnTriggerEnter(Collider o)
    {
        if (o.CompareTag("Player1"))
        {
            mightyLightOfBluishPowa.SetActive(true);
        }

        if (o.CompareTag("Player2"))
        {
            mightyLightOfReddishPowa.SetActive(true);
        }
    }

    void OnTriggerExit(Collider o)
    {
        if (o.CompareTag("Player1"))
        {
            mightyLightOfBluishPowa.SetActive(false);
        }

        if (o.CompareTag("Player2"))
        {
            mightyLightOfReddishPowa.SetActive(false);
        }
    }
}
