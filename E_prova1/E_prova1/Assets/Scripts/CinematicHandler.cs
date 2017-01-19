using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CinematicHandler : MonoBehaviour {

    bool triggered = false;
    string[] messaggi;

    public int from;
    public int to;

    private int indice;
    private string currentString;

    private bool imVisible = false;
    private bool imWaitingToStop = false;

    public GameObject EnemyToMove;
    public GameObject EnemyToHide;

    public Vector3 PosizioneFinale;

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
                "A:Ohh, what!? I am outside!",
                "B:... (noise)",
                "A:Who or what are you? And what have you done?",
                "B:I'm the one who'll destroy the two worlds, and i've entangled two worlds that are collapsing",
                "A:You do what? I won't let you do this",
                "A:\"it could be dangerous if it go away freely\"",
                "B:You have to catch me first",
                "A:What's happening to this world?",
				"B:The same thing that's happening in the other one. For now there are just some time and space distortions",
				"A:And what will happen next?",
				"B:The two worlds will collapse and they will no longer exist in the two different universe",
                "B:Now i have just to hide from you until the worlds end",
                "B:As you can see, i can pass through objects and i can fly, i seriously doubt you can catch me",
                "B:AHAHAHAHAHAH",
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

        Debug.Log(Camera.main.transform.position.x - GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.position.x);
    }

    IEnumerator spegniti()
    {
        imWaitingToStop = true;
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        imVisible = false;
        UIGameplayManager.Instance.hideThisWin(UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "CinematicBlue" : "CinematicRed"]);
        //GameManager.Instance.m_IsWindowOver = false;

        foreach (SkinnedMeshRenderer mr in LeftTalker.GetComponentsInChildren<SkinnedMeshRenderer>()) mr.enabled = false;
        foreach (MeshRenderer mr in RightTalker.GetComponentsInChildren<MeshRenderer>()) mr.enabled = false;

        if (EnemyToMove)
        {
            StartCoroutine(MoveEnemyToNextPosition());
        }

    }

    IEnumerator MoveEnemyToNextPosition()
    {
        yield return new WaitForSeconds(0.1f);

        EnemyToMove.SetActive(true);
        if(EnemyToHide)
            EnemyToHide.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        //EnemyToMove.GetComponent<SplineController>().FollowSpline();
        //EnemyToMove.GetComponent<AudioSource>().Play();
        EnemyToMove.transform.localPosition = PosizioneFinale;
        yield return new WaitForSeconds(0.2f);
        Camera.main.GetComponent<CoolCameraController>().followEnemy(EnemyToMove);
        Camera.main.GetComponent<MotionBlur>().enabled = true;
        yield return new WaitForSeconds(3f);
        Camera.main.GetComponent<CoolCameraController>().resetFollowing();
        GameManager.Instance.m_IsWindowOver = false;
        yield return new WaitUntil(() => Camera.main.transform.position.x - GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.position.x < 3.5f);
        Camera.main.GetComponent<MotionBlur>().enabled = false;
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
