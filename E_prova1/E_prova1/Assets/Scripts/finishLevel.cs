using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finishLevel : MonoBehaviour
{
    public bool isMaster;
    public bool isAlone;
    public finishLevel l2;
    public bool finished = false;

    void OnTriggerEnter(Collider o)
    {
        finished = true;
    }

    void Update()
    {
        if (isMaster)
            loadSelectionScene();
    }

    void loadSelectionScene()
    {
        if (isAlone && finished)
            SceneManager.LoadScene("SelectionScene");
        if(finished && l2.finished)
            SceneManager.LoadScene("SelectionScene");
    }
}
