using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeGUI : MonoBehaviour
{
    public Image guir, guib;
    public Sprite spred, spblue;

    void OnTriggerEnter()
    {
        guir.sprite = spred;
        guib.sprite = spblue;
        Destroy(this);
    }

}
