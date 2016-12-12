using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePopUp : MonoBehaviour
{
    bool triggered = false;
    string[] messaggi;

    public int indice;

    void Awake()
    {
        string [] tmp =
            {
                "to reach some unreachable places you can use the teleporter gadget. select using r1/r2 and press square to use it"
            };
        messaggi = tmp;
    }

    void OnTriggerEnter(Collider o)
    {
        if (!triggered)
        {
            Debug.Log(indice);
            triggered = true;
            UIGameplayManager.Instance.m_UI["Message"].SetActive(true);
            UIGameplayManager.Instance.displayMessage(messaggi[indice]);
            UIGameplayManager.Instance.displayThisWin(UIGameplayManager.Instance.m_UI["Message"]);
            GameManager.Instance.m_IsWindowOver = true;
            StartCoroutine(spegniti());
        }
    }

    IEnumerator spegniti()
    {
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        UIGameplayManager.Instance.hideThisWin(UIGameplayManager.Instance.m_UI["Message"]);
        GameManager.Instance.m_IsWindowOver = false;

    }
}
