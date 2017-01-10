using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{

    public GameObject bottoni;
    public GameObject loading;
    public GameObject creditz;
    public Transform loadingbar;
    public GameObject em;
    public Text percento;

    public void sceneselection()
    {
        SceneManager.LoadScene("SelectionScene");
    }

    public void start()
    {
        bottoni.SetActive(false);
        loading.SetActive(true);
        DestroyImmediate(em);
        //SceneManager.LoadScene("Livello_0");
        StartCoroutine(loadAsync("introduction"));
    }

    public void credits()
    {
        creditz.SetActive(!creditz.activeInHierarchy);
    }

    public void exit()
    {
        Application.Quit();
    }

    private IEnumerator loadAsync(string levelName)
    {
        AsyncOperation operation = Application.LoadLevelAdditiveAsync(levelName);
        while(!operation.isDone) {
            yield return operation.isDone;
            loadingbar.GetComponent<Image>().fillAmount = operation.progress;
            percento.text = ((int)(operation.progress * 100f)).ToString() + "%";
            Debug.Log("loading progress: " + operation.progress);
        }
        loadingbar.GetComponent<Image>().fillAmount = 1f;
        yield return new WaitForSeconds(0.2f);
        SceneManager.UnloadScene("SelectionScene");
        Debug.Log("load done");


    }
}
