using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    private static bool[] m_upgrades = new bool[4] /*{ false, false, false, false }*/{true, true, true, true};
    private List<Item> m_items = new List<Item>();
    private static int[] m_ncollectables = new int[6] { 0, 0, 0, 0, 0, 0 };

    public static readonly int MAX_CAPACITY = 8;

    /*
     * Gadget 1: Exchange object between players
     * Gadget 2: Teleport
     * Gadget 3: Make object movable between
     * Gadget 4: Dash
     *
     */
    public enum Gadgets {Gadget1, Gadget2, Gadget3, Gadget4 };

    public Text m_currentScore;
    public Text m_globalScore;
    public Image[] m_Upgrades;
    public Image[] m_Cells;
    public GameObject m_Puntator;
    private int m_pointTo = 0;
    private bool m_pointerIsMoving = false;

    void Start()
    {
        updateView();
        m_Puntator.GetComponent<RectTransform>().anchoredPosition = m_Cells[0].gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    void OnEnable()
    {
        m_pointTo = 0;
    }

    void Update()
    {
        if (GameManager.Instance.m_inventoryIsOpen && Input.GetButton("Interact") && m_pointTo < m_items.ToArray().Length)
        {
            m_items.ToArray()[m_pointTo].use();
        }

        if (GameManager.Instance.m_inventoryIsOpen && Input.GetButton("Use") && m_upgrades[(int)Gadgets.Gadget1] && m_pointTo < m_items.ToArray().Length)
        {
            GameManager.Instance.m_inventory[!GameManager.Instance.m_sel_pg ? 0 : 1].GetComponent<Inventory>().addItem(m_items.ToArray()[m_pointTo]);
            m_items.RemoveAt(m_pointTo);
            m_Cells[m_pointTo].gameObject.GetComponent<Animation>().Play("General_FadeOut");
            StartCoroutine(detachSprite(m_Cells[m_pointTo]));

        }

        if (Input.GetAxis("R_Horizontal") > 0 && !m_pointerIsMoving)
        {
            m_pointerIsMoving = true;
            m_pointTo = (m_pointTo + 1) % Inventory.MAX_CAPACITY;
            StartCoroutine(OneStep());
        }
        else if (Input.GetAxis("R_Horizontal") < 0 && !m_pointerIsMoving)
        {
            m_pointerIsMoving = true;
            m_pointTo = ((--m_pointTo) < 0 ? Inventory.MAX_CAPACITY + m_pointTo : m_pointTo) % Inventory.MAX_CAPACITY;
            StartCoroutine(OneStep());
        }

        m_Puntator.GetComponent<RectTransform>().anchoredPosition = m_Cells[m_pointTo].gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    IEnumerator detachSprite(Image im)
    {
        yield return new WaitUntil(() => !im.GetComponent<Animation>().isPlaying);
        im.sprite = null;
        GameManager.Instance.hideInventory(GameManager.Instance.m_sel_pg ? 0 : 1);

        Camera.main.GetComponent<SmoothSwitch>().select_singleView();
        if (!GameManager.Instance.m_playerswicth)
        {
            GameManager.Instance.m_sel_pg = !GameManager.Instance.m_sel_pg;
            GameObject.Find("GadgetSelection_1").GetComponent<SwitchGadget>().switchSelectionUI();
            GameObject.Find("GadgetSelection_2").GetComponent<SwitchGadget>().switchSelectionUI();
        }
        else
        {
            GameManager.Instance.m_playerswicth = false;
        }
        GameManager.Instance.mirino.GetComponent<Pointing>().resetPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
        GameManager.Instance.displayInventory(GameManager.Instance.m_sel_pg ? 0 : 1, 0.5f);
    }

    IEnumerator openOtherInventory()
    {
        yield return new WaitUntil(() => !(GameManager.Instance.m_inventory[!GameManager.Instance.m_sel_pg ? 0 : 1].GetComponent<Animation>().isPlaying));
        Debug.Log("Reach!");

    }

    IEnumerator OneStep()
    {
        yield return new WaitForSeconds(0.15f);
        m_pointerIsMoving = false;
    }


	public void addItem(Item i)
    {
        if(typeof(Item) == i.GetType() )
            m_items.Add(i);
    }

    public List<Item> getItems()
    {
        return m_items;
    }

    public Item takeItem(int n)
    {
        if (n < m_items.Count)
        {
            Item tmp = m_items.ToArray()[n];
            m_items.RemoveAt(n);
            return tmp;
        }
        return null;
    }

    public static void score(int level)
    {
        m_ncollectables[level]++;
    }

    public static int getScore(int level)
    {
        return m_ncollectables[level];
    }

    public static int getGlobalScore()
    {
        int sum = 0;
        foreach (int i in m_ncollectables) sum += i;
        return sum;
    }

    public bool hasUpgrade(int n)
    {
        if(n < m_upgrades.Length)
        {
            return m_upgrades[n];
        }
        return false;
    }

    public static bool[] getUpgrades() { return m_upgrades; }


    public static void gainUpgrade(int n)
    {
        if(n < m_upgrades.Length && !m_upgrades[n])
            m_upgrades[n] = true;
    }


    public void updateView()
    {
        m_currentScore.text = m_ncollectables[GameManager.Instance.whichLevelItIs()].ToString();
        m_globalScore.text = Inventory.getGlobalScore().ToString();
        for(int i=0; i < m_upgrades.Length; i++)
        {
            if (m_upgrades[i]) m_Upgrades[i].sprite = GameManager.Instance.getSprite(i);
            else m_Upgrades[i].sprite = null;
        }
        for(int j=0; j < m_items.Count; j++)
        {
            m_Cells[j].sprite = m_items.ToArray()[j].m_this;
            m_Cells[j].color = new Color(1, 1, 1, 1);
        }
    }
}
