using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishLevel : MonoBehaviour
{
    public bool isMaster;
    public finishLevel l2;
    bool finished = false;

    void OnTriggerEnter(Collider o)
    {
        finished = true;
        if (isMaster)
            loadSelectionScene();
    }

    void loadSelectionScene()
    {
        if(finished && l2.finished)
            SceneManager.LoadScene("SelectionScene");
    }
}
