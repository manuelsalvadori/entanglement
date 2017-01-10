using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductionHandler : MonoBehaviour {

    public GameObject Talkers;
    public GameObject Explosion;
    public GameObject Message;
    public GameObject blank;

    public GameObject LeftTalker;
    public GameObject RightTalker;


    public Material LeftTalkerOff;
    public Material RightTalkerOff;

    private Material LeftTalkerOn;
    private Material RightTalkerOn;

    string[] messaggi;

    public int from;
    public int to;
    public int indice;

    private bool isStarted = false;
    private bool secondPart = false;
    private bool imWaitingToStop = false;

    public AudioClip digit;
    public AudioClip explosion;
    public AudioClip background;
    public AudioClip robot;

    string currentString;

    // Use this for initialization
    void Start () {
        string[] tmp =
            {
                "This time i can't fail! This experiment will be a success!",
                "A:WHAT HAPPENED!? And, where am I!?",
                "B:... (strange rumors)",
                "A:It seems that I have been thrown into a space-time distortion!",
                "B:... (strange rumors)",
                "A:Oh my God, my experiment has been a complete failure!",
                "B:~~Ecaps Noitrorsid!~~",
                "A:Oh No! Again!",
            };
        messaggi = tmp;
        indice = from;
        LeftTalkerOn = LeftTalker.GetComponentInChildren<Renderer>().material;
        RightTalkerOn = RightTalker.GetComponentInChildren<Renderer>().material;
        GetComponent<AudioSource>().PlayOneShot(background);
    }

	// Update is called once per frame
	void Update () {
        if (!isStarted && Input.GetButtonDown("Jump"))
        {
            isStarted = true;
            StartCoroutine(go());
        }

        if(isStarted && secondPart && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Ciao");
            if (indice < to)
            {
                indice++;
                currentString = messaggi[indice].Substring(2);
                if (messaggi[indice].ToCharArray()[0] == 'A')
                {
                    foreach (Renderer r in LeftTalker.GetComponentsInChildren<Renderer>()) r.material = LeftTalkerOn;
                    RightTalker.GetComponentInChildren<Renderer>().material = RightTalkerOff;

                }
                else
                {
                    RightTalker.GetComponentInChildren<Renderer>().material = RightTalkerOn;

                    foreach (Renderer r in LeftTalker.GetComponentsInChildren<Renderer>()) r.material = LeftTalkerOff;
                }
                StartCoroutine(UpdateWindow());
            }
            if (indice == to)
                if (!imWaitingToStop)
                    StartCoroutine(spegniti());
        }

	}

    IEnumerator spegniti()
    {
        imWaitingToStop = true;
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        blank.GetComponent<Animation>().Play("ExplosionBlank");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Livello_0");
        //Load  First Scene
    }

    IEnumerator WaitForMaterials()
    {
        yield return new WaitForSeconds(0.1f);

        RightTalker.GetComponentInChildren<Animation>().Play("Opacizza");
    }

    IEnumerator go()
    {
        yield return new WaitForSeconds(0.1f);
        currentString = messaggi[indice];
        UIGameplayManager.Instance.displayMessage(currentString, Message);
        UIGameplayManager.Instance.displayThisWin(Message);
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        indice++;
        UIGameplayManager.Instance.hideThisWin(Message);
        yield return new WaitForSeconds(1f);
        Explosion.GetComponent<AudioSource>().Play();
        Explosion.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(1.2f);
        blank.GetComponent<Animation>().Play("ExplosionBlank");
        yield return new WaitForSeconds(0.6f);
        Camera.main.transform.position = new Vector3(0f, Camera.main.transform.position.y, Camera.main.transform.position.z);
        blank.GetComponent<Animation>().Stop();
        blank.GetComponent<Animation>().Play("FadeBlank");
        yield return new WaitForSeconds(0.2f);
        if (messaggi[indice].ToCharArray()[0] == 'A')
        {
            foreach (Renderer r in LeftTalker.GetComponentsInChildren<Renderer>()) r.material = LeftTalkerOn;

            RightTalker.GetComponentInChildren<Renderer>().material = RightTalkerOff;
        }
        else
        {
            RightTalker.GetComponentInChildren<Renderer>().material = RightTalkerOn;

            foreach (Renderer r in LeftTalker.GetComponentsInChildren<Renderer>()) r.material = LeftTalkerOff;
        }

        currentString = messaggi[indice].Substring(2);
        UIGameplayManager.Instance.displayMessage(currentString, Message);
        UIGameplayManager.Instance.displayThisWin(Message);
        Talkers.SetActive(true);
        secondPart = true;
        GetComponent<AudioSource>().Stop();
        RightTalker.GetComponent<AudioSource>().Play();
    }

    IEnumerator UpdateWindow()
    {
        yield return new WaitForSeconds(0.1f);
        UIGameplayManager.Instance.displayMessage(currentString, Message);

    }


}
