using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class UIGameplayManager : MonoBehaviour {

    public static UIGameplayManager Instance = null;

    public string[] m_FadeInAnimation;
    public string[] m_FadeOutAnimation;

    //Struct to collect in GameManager elements of the GUI...trick to visualize it in the Inspector
    [System.Serializable]
    public struct NamedElements
    {
        public string name;
        public GameObject element;
    }

    public NamedElements[] m_UI_Elements;

    //Real implementation
    public Dictionary<string, GameObject> m_UI = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //Filling the pool
        foreach (NamedElements ne in m_UI_Elements)
        {
            m_UI.Add(ne.name, ne.element);
        }
    }



	// Update is called once per frame
	void Update () {

	}

    public void displayMessage(string message, GameObject o)
    {
        o.transform.GetChild(0).GetComponent<Text>().text = message;
    }

    public void displayThisWin(GameObject go)
    {
        string anim = null;
        if (!go.activeSelf) go.SetActive(true);
        if (go.GetComponent<Animation>().GetClip("General_FadeIn"))
            go.GetComponent<Animation>().Play("General_FadeIn");
        else if((anim = searchInAnimation(go, m_FadeInAnimation)) != null)
            go.GetComponent<Animation>().Play(anim);
        for (int i = 0; i < go.transform.childCount; i++)
            displayThisWin(go.transform.GetChild(i).gameObject);
    }

    public void hideThisWin(GameObject go)
    {
        string anim = null;
        if (!go.activeSelf) go.SetActive(true);
        if (go.GetComponent<Animation>().GetClip("General_FadeOut"))
            go.GetComponent<Animation>().Play("General_FadeOut");
        else if ((anim = searchInAnimation(go, m_FadeOutAnimation)) != null)
            go.GetComponent<Animation>().Play(anim);
        StartCoroutine(shutdown_thisWin(go));
        for (int i = 0; i < go.transform.childCount; i++)
            hideThisWin(go.transform.GetChild(i).gameObject);
    }

    IEnumerator shutdown_thisWin(GameObject go)
    {
        yield return new WaitUntil(() => !go.GetComponent<Animation>().isPlaying);
        go.SetActive(false);
        if (go.tag.Equals("Inventory"))
            GameManager.Instance.m_inventoryIsOpen = false;
    }

    public string searchInAnimation(GameObject go, string[] where)
    {

        foreach(string s in where)
        {
            if (go.GetComponent<Animation>().GetClip(s)) return s;
        }
        return null;
    }
}
