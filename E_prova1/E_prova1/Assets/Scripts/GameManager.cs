using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;
    public bool m_camIsMoving = false;
    public bool m_3D_mode = false;
    public bool m_double_mode = false;
    public bool m_single_mode = true;
    public bool m_sel_pg = true;
    public SwitchGadget[] m_gadgetSelection;

    public GameObject[] m_players;
    public Inventory[] m_inventory;
    public GameObject m_level1;
    public GameObject m_level2;

    public GameObject m_gameplay_UI_Canvas;

    //Struct to collect in GameManager elements of the GUI...trick to visualize it in the Inspector
    [System.Serializable]
    public struct NamedElements
    {
        public string name;
        public GameObject element;
    }

    public NamedElements[] m_UI_Elements;

    //Real implementation
    public Dictionary<string, GameObject> m_UI = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //Filling the pool
        foreach (NamedElements ne in m_UI_Elements)
        {
            m_UI.Add(ne.name, ne.element);
        }
    }

    void Update()
    {
        //Change player selected on Double Mode
        if ((Input.GetKeyDown(KeyCode.Tab) || (Input.GetButton("L2") && Input.GetButtonDown("X"))) && m_double_mode)
        {
            GameManager.Instance.m_sel_pg = !GameManager.Instance.m_sel_pg;
            GameObject.Find("GadgetSelection_1").GetComponent<SwitchGadget>().switchSelectionUI();
            GameObject.Find("GadgetSelection_2").GetComponent<SwitchGadget>().switchSelectionUI();
        }

        if (Input.GetButtonDown("Use"))
        {
            if (m_inventory[(m_sel_pg) ? 0 : 1].hasUpgrade(m_gadgetSelection[(m_sel_pg) ? 0 : 1].m_state))
            {
                m_players[(m_sel_pg) ? 0 : 1].GetComponent<PlayerController>().useGadget(m_gadgetSelection[(m_sel_pg) ? 0 : 1].m_state);
            }
            else
            {
                Debug.Log("NO UPGRADE ");
            }
        }

        if(Input.GetKeyDown(KeyCode.O) || Input.GetButtonDown("Triangle")){
            if(!m_inventory[GameManager.Instance.m_sel_pg ? 0 : 1].gameObject.activeSelf)
                displayInventory(GameManager.Instance.m_sel_pg ? 0 : 1);
            else
            {
                hideInventory(GameManager.Instance.m_sel_pg ? 0 : 1);
            }
        }

        if (!m_3D_mode)
            GameManager.Instance.m_players[1].transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.zero);

    }
        
    //Check if the two players are in the "same" x
    public bool isPlayersInline()
    {
        return (Mathf.Abs(m_players[0].transform.position.x - m_players[1].transform.position.x) < 2f);
    }

    //Function to start the syncronized movement of players
    //Create the configurable Joint between players and set the other gameobject properties in order to follow player 1 movement
    public static IEnumerator activateChildMode()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !GameManager.Instance.m_camIsMoving);
        GameManager.Instance.m_players[0].AddComponent<ConfigurableJoint>();
        GameManager.Instance.m_players[0].GetComponent<ConfigurableJoint>().connectedBody = GameManager.Instance.m_players[1].GetComponent<Rigidbody>();
        //Leave the player free to move in Y Motion
        GameManager.Instance.m_players[0].GetComponent<ConfigurableJoint>().xMotion = ConfigurableJointMotion.Locked;
        GameManager.Instance.m_players[0].GetComponent<ConfigurableJoint>().zMotion = ConfigurableJointMotion.Locked;
        GameManager.Instance.m_players[1].GetComponent<MeshRenderer>().enabled = false;
        GameManager.Instance.m_players[1].GetComponent<PlayerController>().enabled = false;
        GameManager.Instance.m_players[1].GetComponent<JumpController>().enabled = true;
    }

    //Function to end the syncronized movement of players
    public static void deactivateChildMode()
    {
        Destroy(GameManager.Instance.m_players[0].GetComponent<ConfigurableJoint>());
        GameManager.Instance.m_players[1].GetComponent<MeshRenderer>().enabled = true;
        GameManager.Instance.m_players[1].GetComponent<PlayerController>().enabled = true;
        GameManager.Instance.m_players[1].GetComponent<JumpController>().enabled = false;
        GameManager.Instance.m_players[1].transform.rotation = GameManager.Instance.m_players[1].transform.GetChild(0).localRotation;
        GameManager.Instance.m_players[1].transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.zero);
        Debug.Log(GameManager.Instance.m_players[1].transform.GetChild(0).localRotation.eulerAngles);
    }

    public static bool isPickble(GameObject go)
    {
        return go.GetComponent<PickableObject>() != null;
    }

    public int whoAmI(string name)
    {
        return m_players[0].gameObject.name.Equals(name) ? 0 : m_players[1].gameObject.name.Equals(name) ? 1 : 3;
    }


    public void displayInventory(int which)
    {
        m_inventory[which].gameObject.SetActive(true);
        m_inventory[which].gameObject.GetComponent<Animation>().Play("General_FadeIn");
        
        foreach (Animation a in m_inventory[which].gameObject.GetComponentsInChildren<Animation>())
        {
            if (a.gameObject.GetComponent<Text>())
                a.Play("General_Text_FadeIn");
            else
            {
                a.Play("General_FadeIn");
            }
        }
    }

    public void hideInventory(int which)
    {
        m_inventory[which].gameObject.GetComponent<Animation>().Play("General_FadeOut");
        
        foreach (Animation a in m_inventory[which].gameObject.GetComponentsInChildren<Animation>())
        {
            if (a.gameObject.GetComponent<Text>())
                a.Play("General_Text_FadeOut");
            else
            {
                a.Play("General_FadeOut");
            }
        }
        StartCoroutine(shutdown_thisWin(m_inventory[which].gameObject));
    }

    IEnumerator shutdown_thisWin(GameObject go)
    {
        yield return new WaitUntil(() => !go.GetComponent<Animation>().isPlaying);
        go.SetActive(false);
    }

}
