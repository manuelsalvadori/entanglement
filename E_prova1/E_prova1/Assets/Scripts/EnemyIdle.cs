using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : MonoBehaviour {

    public float speed = 1f;
    float init_y;
    float rnd;

    public GameObject scudo;

    private GameObject futureEnemy;

    public float amount = 3f;


    bool isSounding = false;

    void Start()
    {
        init_y = transform.localPosition.y;
        rnd = Random.Range(0.1f, 0.9f);
    }

    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, init_y + Mathf.Sin(Time.time * speed + rnd) / amount, transform.localPosition.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Checkpoint"))
        {

            if (other.gameObject.tag.Equals("Cinematic"))
            {
                futureEnemy = other.gameObject.GetComponent<CinematicHandler>().EnemyToMove;
                StartCoroutine(deactivateFollowing());
            }
            else
            {
                if (other.gameObject.tag.Equals("Ending"))
                    StartCoroutine(deactivateFollowing(false, 0.6f));
                scudo.GetComponent<MeshRenderer>().enabled = true;
                if (!isSounding)
                {
                    isSounding = true;
                    GetComponent<AudioSource>().Play();
                    StartCoroutine(shutUP());
                }

                StartCoroutine(deactivateShield());
            }
        }

    }

    IEnumerator shutUP()
    {
        yield return new WaitForSeconds(1f);
        isSounding = false;
    }

    IEnumerator deactivateFollowing(bool activate = true, float time = 2.5f )
    {
        yield return new WaitForSeconds(time);
        Camera.main.GetComponent<CoolCameraController>().resetFollowing();
        if(activate)
            futureEnemy.SetActive(true);
        GameManager.Instance.m_IsWindowOver = false;
    }

    IEnumerator deactivateShield()
    {
        yield return new WaitForSeconds(1f);
        scudo.GetComponent<MeshRenderer>().enabled = false;
    }
}
