using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour {

    void OnTriggerEnter(Collider o)
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
