using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour
{
    [Range (0,5)]
    public int level_to_load;

    void OnTriggerEnter(Collider o)
    {
        if (o.gameObject.tag.Equals("Player1"))
        {
            switch (level_to_load)
            {
                case 0:
                    SceneManager.LoadScene("LevelSelection");
                    break;
                case 1:
                    SceneManager.LoadScene("Level_1");
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
