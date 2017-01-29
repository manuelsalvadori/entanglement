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

    public void startbutton()
    {
        bottoni.SetActive(false);
        loading.SetActive(true);
        DestroyImmediate(em);
        StartCoroutine(loadAsync("introduction"));
    }

    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void exit()
    {
        Application.Quit();
    }

    public void livello1()
    {
        StartCoroutine(loadAsync("Livello_1"));
    }

    private IEnumerator loadAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
        while(!operation.isDone)
        {
            yield return operation.isDone;
            loadingbar.GetComponent<Image>().fillAmount = operation.progress;
            percento.text = ((int)(operation.progress * 100f)).ToString() + "%";
        }
        loadingbar.GetComponent<Image>().fillAmount = 1f;
        yield return new WaitForSeconds(0.2f);
    }

    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
