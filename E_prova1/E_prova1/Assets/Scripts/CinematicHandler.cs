using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicHandler : MonoBehaviour {

    bool triggered = false;
    string[] messaggi;

    public int from;
    public int to;

    private int indice;
    private string currentString;

    private bool imVisible = false;
    private bool imWaitingToStop = false;

    public GameObject LeftTalker;
    public GameObject RightTalker;

    public Material LeftTalkerOff;
    public Material RightTalkerOff;

    private Material LeftTalkerOn;
    private Material RightTalkerOn;


    void Awake()
    {
        string[] tmp =
            {
                "A:stringa1",
                "B:stringa2",
                "A:stringa3",
                "A:stringa4",
                "B:stringa5",
                "A:stringa6",
                "B:stringa7",
                "A:stringa8",
                "B:stringa9",
                "B:stringa10"
            };
        messaggi = tmp;
        LeftTalkerOn = LeftTalker.GetComponentInChildren<Renderer>().material;
        RightTalkerOn = RightTalker.GetComponentInChildren<Renderer>().material;
    }

    void Start()
    {
        indice = from;
        currentString = messaggi[indice].Substring(2);

    }

    void OnTriggerEnter(Collider o)
    {
        //Debug.Log(o.gameObject.name);
        if (!triggered)
        {
            triggered = true;
            imVisible = true;
            StartCoroutine(DisplayCinematic());

        }
    }


    private void Update()
    {
        if (Input.GetButtonDown("Jump") && imVisible && GameManager.Instance.m_IsWindowOver)
        {
            if (indice < to)
            {
                indice++;
                currentString = messaggi[indice].Substring(2);
                if (messaggi[indice].ToCharArray()[0] == 'A')
                {
                    foreach(Renderer r in LeftTalker.GetComponentsInChildren<Renderer>()) r.material = LeftTalkerOn;
                    RightTalker.GetComponentInChildren<Animation>().Stop();
                    StartCoroutine(WaitForMaterials());

                }
                else
                {
                    RightTalker.GetComponentInChildren<Renderer>().material = RightTalkerOn;
                    //RightTalker.GetComponentInChildren<Animation>()["Shining_robot_Scanner"].enabled = true;
                    RightTalker.GetComponentInChildren<Animation>().Play();
                    foreach (Renderer r in LeftTalker.GetComponentsInChildren<Renderer>()) r.material = LeftTalkerOff;
                }
                StartCoroutine(UpdateWindow());
            }
        }
        if (indice == to && imVisible)
            if(!imWaitingToStop)
                StartCoroutine(spegniti());
    }

    IEnumerator spegniti()
    {
        imWaitingToStop = true;
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        imVisible = false;
        UIGameplayManager.Instance.hideThisWin(UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "Message" : "MessageRed"]);
        GameManager.Instance.m_IsWindowOver = false;

        foreach (SkinnedMeshRenderer mr in LeftTalker.GetComponentsInChildren<SkinnedMeshRenderer>()) mr.enabled = false;
        foreach (MeshRenderer mr in RightTalker.GetComponentsInChildren<MeshRenderer>()) mr.enabled = false;

    }

    IEnumerator UpdateWindow()
    {
        yield return new WaitForSeconds(0.1f);
        UIGameplayManager.Instance.displayMessage(currentString, UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "Message" : "MessageRed"]);

    }

    IEnumerator WaitForMaterials()
    {
        yield return new WaitForSeconds(0.1f);
        RightTalker.GetComponentInChildren<Renderer>().material = RightTalkerOff;
        RightTalker.GetComponentInChildren<Animation>().Play("Opacizza");
    }


    IEnumerator DisplayCinematic()
    {
        yield return new WaitForSeconds(1f);
        UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "Message" : "MessageRed"].SetActive(true);
        UIGameplayManager.Instance.displayMessage(currentString, UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "Message" : "MessageRed"]);
        UIGameplayManager.Instance.displayThisWin(UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "Message" : "MessageRed"]);



        if (messaggi[indice].ToCharArray()[0] == 'A')
        {
            foreach (Renderer r in LeftTalker.GetComponentsInChildren<Renderer>()) r.material = LeftTalkerOn;
            //RightTalker.GetComponentInChildren<Animation>()["Shining_robot_Scanner"].time = 0;
            RightTalker.GetComponentInChildren<Animation>().Stop();
            RightTalker.GetComponentInChildren<Renderer>().material = RightTalkerOff;
            RightTalker.GetComponentInChildren<Animation>().Play("Opacizza");

        }
        else
        {
            RightTalker.GetComponentInChildren<Renderer>().material = RightTalkerOn;
            RightTalker.GetComponentInChildren<Animation>().Play();
            foreach (Renderer r in LeftTalker.GetComponentsInChildren<Renderer>()) r.material = LeftTalkerOff;
        }
        foreach (SkinnedMeshRenderer mr in LeftTalker.GetComponentsInChildren<SkinnedMeshRenderer>()) mr.enabled = true;
        foreach (MeshRenderer mr in RightTalker.GetComponentsInChildren<MeshRenderer>()) mr.enabled = true;

        GameManager.Instance.m_IsWindowOver = true;
    }

}
