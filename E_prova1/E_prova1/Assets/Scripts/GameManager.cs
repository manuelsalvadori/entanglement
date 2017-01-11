using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int m_frameRate = 60;
    public static GameManager Instance = null;
    public bool m_camIsMoving = false;
    public bool m_levelIsMoving = false;

    public bool m_IsWindowOver = false;
    public bool m_IsFading = false;

    public Transform[] m_checkpoints;
    private Transform[] m_current_checkpoint;
    [Range (0,10)]
    public int select_checkpoint;

    public int m_Current_State = (int)CoolCameraController.Stato.First_Player;

    public bool m_3D_mode = false;
    public bool m_double_mode = false;
    public bool m_single_mode = true;
    public bool m_sel_pg = true;

	public int m_lockedPlayer = 0;

    public bool[] m_UpgradesActive = {false, false, false, false};
    public bool m_inventoryIsOpen = false;


    enum Levels { Zero, One, Two, Three, Four, Final};
    public bool m_playerswicth = false;
    public SwitchGadget[] m_gadgetSelection;

    public GameObject mirino;
    public GameObject pistola;
    public GameObject[] m_players;
    public Inventory[] m_inventory;
    public GameObject m_level1;
    public GameObject m_level2;
    public int m_currentLevel;
    public float m_ZLevel;

    public Sprite[] m_itemInvetoryView;

    public GameObject m_gameplay_UI_Canvas;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Application.targetFrameRate = m_frameRate;
        m_current_checkpoint = new Transform[2];
    }

    void Start()
    {
        m_current_checkpoint[0] = m_checkpoints[select_checkpoint];
        m_current_checkpoint[1] = m_checkpoints[select_checkpoint];
        resetPlayersPosition();
        m_ZLevel = m_current_checkpoint[0].position.z;
    }

    void Update()
    {
        m_3D_mode = m_Current_State == (int)CoolCameraController.Stato.TreD;

        //Change player selected on Double Mode
        if ((Input.GetKeyDown(KeyCode.Tab) || (Input.GetButton("L2") && Input.GetButtonDown("X"))) && m_Current_State == (int)CoolCameraController.Stato.Doppia)
        {
            GameManager.Instance.m_sel_pg = !GameManager.Instance.m_sel_pg;
            Camera.main.GetComponent<CoolCameraController>().changeOldState(GameManager.Instance.m_sel_pg ? 0 : 1);
            m_playerswicth = !m_playerswicth;
            GameObject.Find("GadgetSelection_1").GetComponent<SwitchGadget>().switchSelectionUI();
            GameObject.Find("GadgetSelection_2").GetComponent<SwitchGadget>().switchSelectionUI();
            mirino.GetComponent<Pointing>().resetPosition(m_players[(m_sel_pg) ? 0 : 1].transform.position);
            pistola.GetComponent<shoot>().resetShootPosition(m_players[(m_sel_pg) ? 0 : 1].transform.position);

        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("SelectionScene");
        }

        if ((Input.GetButtonDown("Use") && !Input.GetButton("L2")) && !m_inventoryIsOpen && !GameManager.Instance.m_IsWindowOver && GameManager.Instance.m_Current_State != (int) CoolCameraController.Stato.TreD)
        {
            if (hasUpgrade(m_gadgetSelection[(m_sel_pg) ? 0 : 1].m_state)) {
                m_players[(m_sel_pg) ? 0 : 1].GetComponent<PlayerController>().useGadget(m_gadgetSelection[(m_sel_pg) ? 0 : 1].m_state);
            }
            else
            {
                Debug.Log("NO UPGRADE ");
            }
        }

        if ((!Input.GetButton("L2") && Input.GetButtonDown("Inventory")) && !m_3D_mode && !GameManager.Instance.m_IsWindowOver && !m_IsFading) {
            if (!m_inventory[GameManager.Instance.m_sel_pg ? 0 : 1].gameObject.activeSelf && !m_inventoryIsOpen)
            {
                m_inventory[GameManager.Instance.m_sel_pg ? 0 : 1].GetComponent<Inventory>().updateView();
                displayInventory(GameManager.Instance.m_sel_pg ? 0 : 1);
                m_inventoryIsOpen = true;
            }
            else
            {
                hideInventory(GameManager.Instance.m_sel_pg ? 0 : 1);
            }
        }

        if (!m_3D_mode)
            GameManager.Instance.m_players[1].transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.zero);

        /*
        if(m_inventory[0].gameObject.activeSelf || m_inventory[1].gameObject.activeSelf)
        {
            m_inventoryIsOpen = true;
        }
        */

    }

    //Check if the two players are in the "same" x
    public bool isPlayersInline()
    {
        return (Mathf.Abs(m_players[0].transform.position.x - m_players[1].transform.position.x) < 2f);
    }


    public static IEnumerator alinePlayers()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !GameManager.Instance.m_levelIsMoving);
        GameManager.Instance.m_players[(!GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position = (new Vector3(
                GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.x,
                GameManager.Instance.m_players[(!GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.y,
                GameManager.Instance.m_players[(!GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.z));
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
        //GameManager.Instance.m_players[1].GetComponent<MeshRenderer>().enabled = false;
        GameManager.Instance.m_players[1].GetComponent<PlayerController>().enabled = false;
        GameManager.Instance.m_players[1].GetComponent<JumpController>().enabled = true;
    }

    //Function to end the syncronized movement of players
    public static void deactivateChildMode()
    {
        Destroy(GameManager.Instance.m_players[0].GetComponent<ConfigurableJoint>());
        //GameManager.Instance.m_players[1].GetComponent<MeshRenderer>().enabled = true;
        GameManager.Instance.m_players[1].GetComponent<PlayerController>().enabled = true;
        GameManager.Instance.m_players[1].GetComponent<JumpController>().enabled = false;
        GameManager.Instance.m_players[1].transform.rotation = GameManager.Instance.m_players[1].transform.GetChild(0).localRotation;
        GameManager.Instance.m_players[1].transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.zero);
        //Debug.Log(GameManager.Instance.m_players[1].transform.GetChild(0).localRotation.eulerAngles);
    }

    public static bool isPickble(GameObject go)
    {
        return go.GetComponent<PickableObject>() != null;
    }

    public int whoAmI(string name)
    {
        return m_players[0].gameObject.name.Equals(name) ? 0 : m_players[1].gameObject.name.Equals(name) ? 1 : 3;
    }


    public void displayInventory(int which, float delay = 0f)
    {
        if (delay > 0)
        {
            StartCoroutine(delayedDisplay(which, delay));
        }
        else
        {

            m_inventory[which].gameObject.SetActive(true);
            m_inventory[which].gameObject.GetComponent<Inventory>().updateView();
            m_inventory[which].gameObject.GetComponent<Animation>().Play("General_FadeIn");

            foreach (Animation a in m_inventory[which].gameObject.GetComponentsInChildren<Animation>())
            {
                if (a.gameObject.GetComponent<Text>())
                    a.Play("General_Text_FadeIn");
                else
                {
                    if (a.gameObject.GetComponent<Image>().sprite != null)
                        a.Play("General_FadeIn");

                }
            }
            m_inventoryIsOpen = true;
        }
    }

    IEnumerator delayedDisplay(int which, float time)
    {

        yield return new WaitForSeconds(time);
        displayInventory(which, 0f);

    }

    public void hideInventory(int which)
    {
        if (!m_inventory[which].gameObject.GetComponent<Animation>().isPlaying)
        {
            m_inventory[which].gameObject.GetComponent<Animation>().Play("General_FadeOut");

            foreach (Animation a in m_inventory[which].gameObject.GetComponentsInChildren<Animation>())
            {
                if (a.gameObject.GetComponent<Text>())
                    a.Play("General_Text_FadeOut");
                else
                {
                    if (a.gameObject.GetComponent<Image>().sprite != null)
                        a.Play("General_FadeOut");
                }
            }
            m_inventoryIsOpen = false;
            StartCoroutine(shutdown_thisWin(m_inventory[which].gameObject));
        }
    }

    public IEnumerator shutdown_thisWin(GameObject go)
    {
        yield return new WaitUntil(() => !go.GetComponent<Animation>().isPlaying);
        go.SetActive(false);
        /*
        if(go.tag.Equals("Inventory"))
            m_inventoryIsOpen = false;
            */
    }

    public int whichLevelItIs()
    {
        return m_currentLevel;
    }

    public Sprite getSprite(int n)
    {
        return m_itemInvetoryView[n];
    }

    public void resetPlayersPosition()
    {
        m_players[0].transform.position = m_current_checkpoint[0].position;
        m_players[1].transform.position = m_current_checkpoint[0].position + new Vector3(0f, -65f,0f);
    }

    public void onDeathPlayer(int sel_pg)
    {
        if (!m_3D_mode)
            m_players[sel_pg].transform.position = m_current_checkpoint[sel_pg].position + new Vector3(0f, -65f * sel_pg, 0f);
        else
        {
            m_players[0].transform.position = m_current_checkpoint[0].position;
            m_players[1].transform.position = m_current_checkpoint[0].position + new Vector3(0f, -65f, 0f);
        }
    }

    public void updateCheckpoint(Transform cp)
    {
        m_current_checkpoint[m_sel_pg ? 0 : 1] = cp;
    }

    public bool[] getUpgrades() { return m_UpgradesActive; }


    public void gainUpgrade(int n)
    {
        if(n < m_UpgradesActive.Length && !m_UpgradesActive[n])
            m_UpgradesActive[n] = true;
    }

    public bool hasUpgrade(int n)
    {
        if(n < m_UpgradesActive.Length)
        {
            return m_UpgradesActive[n];
        }
        return false;
    }
}
