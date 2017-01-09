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
                "to reach some unreachable places you can use the teleporter gadget. select using r1/r2 and press square to use it",
                "press x to jump",
                "press square to activate the switches",
                "press triangle to open the inventory. select an item with right stick and press square to use it or circle to transfer it in the other player inventory",
                "press down to select the second dimension, up to return in this one",
                "to cross the energy barrier, select the dash gadget with r1/r2 and press circle to use it",
                "The red mark shows the position of your alter ego in the secon dimension. When you are over the red mark you can get entangled with the alter ego by pressing R2",
                "to move the platforms to the other dimension select the transfer gun gadget using r1/r2 and press square to use it",
                "You obtain the TRANSFER gadget!\nFor transfer an item from red inventory to the blue one or viceversa, select him and press CIRCLE",
                "You obtain the DASH gadget!\nSelect him with L1/R1 and press CIRCLE to cross energy barrier or to jump longer",
                "You obtain the GUN gadget!\nSelect him with L1/R1 and press CIRCLE to transfer the platform  in the other dimension",
                "You obtain the TELEPORT gadget!\nSelect him with L1/R1 and press CIRCLE to get teleported! Use right stick for change the destination"
            };
        messaggi = tmp;
    }

    void OnTriggerEnter(Collider o)
    {
        //Debug.Log(o.gameObject.name);
        if (!triggered)
        {
            triggered = true;
            UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "MessageBlue" : "MessageRed"].SetActive(true);
            UIGameplayManager.Instance.displayMessage(messaggi[indice], UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "MessageBlue" : "MessageRed"]);
            UIGameplayManager.Instance.displayThisWin(UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "MessageBlue" : "MessageRed"]);
            GameManager.Instance.m_IsWindowOver = true;
            StartCoroutine(spegniti());
        }
    }

    IEnumerator spegniti()
    {
        yield return new WaitForSeconds(1.5f);
        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
        UIGameplayManager.Instance.hideThisWin(UIGameplayManager.Instance.m_UI[GameManager.Instance.m_sel_pg ? "MessageBlue" : "MessageRed"]);
        GameManager.Instance.m_IsWindowOver = false;

    }
}
