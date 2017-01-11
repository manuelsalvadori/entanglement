using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicAlternative : MonoBehaviour
{

    bool triggered = false;
    string[] messaggi;

    public int from;
    public int to;
    public int final;

    private int indice;
    private string currentString;

    private bool imVisible = false;
    private bool imWaitingToStop = false;
    private bool imChanging = false;

    public GameObject EnemyToMove;
    public GameObject EnemyToHide;

    public GameObject LeftTalker;
    public GameObject RightTalker;


    public GameObject LeftTalker2;
    public GameObject RightTalker2;

    public Material LeftTalkerOff;
    public Material RightTalkerOff;

    public Material LeftTalkerOff2;
    public Material RightTalkerOff2;

    private Material LeftTalkerOn;
    private Material RightTalkerOn;


    private bool firstpart = false;
    private bool secondpart = false;

    void Awake()
    {
        string[] tmp =
            {
                "A:So, are you hiding in this lab!?",
                "B:If you are alone, you'll never get me!",
                "B:You and your dopplegenger will be defeated!",
                "A:Oh no, It seems that there is another world affected by this pain!",
                "B:Muahauha, you and your world are doomed!",
                "B:You'll never catch me!",
                "A:I have to do something! Quickly!",
                "You can change player with L2",
                "B:stringa13"
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
        if (o.gameObject.tag.Equals("Player1") || o.gameObject.tag.Equals("Player2"))
        {
            //Debug.Log(o.gameObject.name);
            if (!triggered)
            {
                triggered = true;
                imVisible = true;
                StartCoroutine(DisplayCinematic());

            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && imVisible && GameManager.Instance.m_IsWindowOver && !firstpart)
        {
            if (indice < to)
            {
                indice++;
                currentString = messaggi[indice].Substring(2);
                if (messaggi[indice].ToCharArray()[0] == 'A')
                {
                    foreach (Renderer r in LeftTalker.GetComponentsInChildren<Renderer>()) r.material = LeftTalkerOn;
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
            if (!imChanging)
                StartCoroutine(changePlayer());
        if (secondpart && Input.GetButtonDown("Jump") && imVisible && GameManager.Instance.m_IsWindowOver)
        {
            if (indice < final)
            {
                indice++;
                currentString = messaggi[indice].Substring(2);
                if (messaggi[indice].ToCharArray()[0] == 'A')
                {
                    foreach (Renderer r in LeftTalker.GetComponentsInChildren<Renderer>()) r.material = LeftTalkerOn;
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
        if (indice == final && imVisible)
        {
            if (!imWaitingToStop)
                StartCoroutine(spegniti());
        }
    }

    IEnumerator changePlayer()
    {
        firstpart = true;
        imChanging = true;
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        UIGameplayManager.Instance.hideThisWin(UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "CinematicBlue" : "CinematicRed"]);
        foreach (SkinnedMeshRenderer mr in LeftTalker.GetComponentsInChildren<SkinnedMeshRenderer>()) mr.enabled = false;
        foreach (MeshRenderer mr in RightTalker.GetComponentsInChildren<MeshRenderer>()) mr.enabled = false;
        yield return new WaitForSeconds(0.5f);
        EnemyToMove.GetComponent<EnemyIdle>().AttivaScudo();
        yield return new WaitForSeconds(1.2f);
        EnemyToMove.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Camera.main.GetComponent<CoolCameraController>().select_singleView(1);
        yield return new WaitForSeconds(0.7f);
        RightTalker = RightTalker2;
        RightTalkerOn = RightTalker.GetComponentInChildren<Renderer>().material;
        LeftTalker = LeftTalker2;
        LeftTalkerOn = LeftTalker.GetComponentInChildren<Renderer>().material;
        RightTalkerOff = RightTalkerOff2;
        LeftTalkerOff = LeftTalkerOff2;
        StartCoroutine(DisplayCinematic());
        yield return new WaitForSeconds(0.2f);
        secondpart = true;

    }

    IEnumerator spegniti()
    {
        imWaitingToStop = true;
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));

        UIGameplayManager.Instance.hideThisWin(UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "CinematicBlue" : "CinematicRed"]);
        //GameManager.Instance.m_IsWindowOver = false;

        foreach (SkinnedMeshRenderer mr in LeftTalker.GetComponentsInChildren<SkinnedMeshRenderer>()) mr.enabled = false;
        foreach (MeshRenderer mr in RightTalker.GetComponentsInChildren<MeshRenderer>()) mr.enabled = false;

        yield return new WaitForSeconds(0.5f);
        UIGameplayManager.Instance.m_UI["MessageRed"].SetActive(true);
        currentString = messaggi[7];
        UIGameplayManager.Instance.displayMessage(currentString, UIGameplayManager.Instance.m_UI["MessageRed"]);
        UIGameplayManager.Instance.displayThisWin(UIGameplayManager.Instance.m_UI["MessageRed"]);
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        UIGameplayManager.Instance.hideThisWin(UIGameplayManager.Instance.m_UI["MessageRed"]);
        GameManager.Instance.m_IsWindowOver = false;
        imVisible = false;
    }

    IEnumerator UpdateWindow()
    {
        yield return new WaitForSeconds(0.1f);
        UIGameplayManager.Instance.displayMessage(currentString, UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "CinematicBlue" : "CinematicRed"]);

    }

    IEnumerator WaitForMaterials()
    {
        yield return new WaitForSeconds(0.1f);
        RightTalker.GetComponentInChildren<Renderer>().material = RightTalkerOff;
        RightTalker.GetComponentInChildren<Animation>().Play("Opacizza");
    }


    IEnumerator DisplayCinematic()
    {
        yield return new WaitForSeconds(0.3f);
        UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "CinematicBlue" : "CinematicRed"].SetActive(true);
        UIGameplayManager.Instance.displayMessage(currentString, UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "CinematicBlue" : "CinematicRed"]);
        UIGameplayManager.Instance.displayThisWin(UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "CinematicBlue" : "CinematicRed"]);



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
