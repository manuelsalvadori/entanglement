using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class exit : MonoBehaviour
{
    bool p1 = false, p2 = false;
    [Range (0,5)]
    public int level_to_load;
    public bool isSelectLevel = true;
    public GameObject canvas;
    public Transform loadingbar;
    public Text percento;
    public Camera cam;

    void OnTriggerEnter(Collider o)
    {
        if (!o.gameObject.tag.Equals("enemy"))
        {
            if (o.gameObject.tag.Equals("Player1"))
                p1 = true;

            if (o.gameObject.tag.Equals("Player2"))
                p2 = true;

            if ((p1 && p2) || isSelectLevel)
            {
                switch (level_to_load)
                {
                    case 0:
                        SceneManager.LoadScene("LevelSelection");
                        break;
                    case 1:
                        canvas.SetActive(true);
                        Camera.main.enabled = false;

                        cam.enabled = true;
                        StartCoroutine(loadAsync("Livello_1"));
                        break;
                    case 2:
                        SceneManager.LoadScene("Level_2");
                        break;
                    case 3:
                        SceneManager.LoadScene("Level_3");
                        break;
                    case 4:
                        SceneManager.LoadScene("Level_4");
                        break;
                    case 5:
                        SceneManager.LoadScene("Level_5");
                        break;
                    default:
                        return;
                }
            }
        }
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
}
