using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiocollisionbox : MonoBehaviour
{
    void OnCollisionEnter(Collision o)
    {
        
        if (o.gameObject.tag == "Player1")
        {
            Debug.Log("pg2 muore");
            GetComponent<AudioSource>().Play();
        }
    }

    void OnCollisionExit(Collision o)
    {
        if(o.gameObject.tag == "Player1")
            GetComponent<AudioSource>().Stop();
    }

}
