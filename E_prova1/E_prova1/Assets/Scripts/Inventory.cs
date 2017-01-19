using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    private bool[] m_upgradesActivation;
    private List<Item> m_items = new List<Item>();
    public static int[] m_ncollectables = new int[6] { 0, 0, 0, 0, 0, 0 };

    public static readonly int MAX_CAPACITY = 10;

    private bool locker = false;
    private bool locker2 = false;

    public Text descr_b, descr_r;

    /*
     * Gadget 4: Exchange object between players
     * Gadget 2: Teleport
     * Gadget 1: Make object movable between
     * Gadget 3: Dash
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
        m_upgradesActivation = GameManager.Instance.m_UpgradesActive;
    }

    void OnEnable()
    {
        m_pointTo = 0;
        m_upgradesActivation = GameManager.Instance.m_UpgradesActive;
    }

    bool usedkey = false;

    void Update()
    {
        if (GameManager.Instance.m_inventoryIsOpen && Input.GetButton("Interact") && m_pointTo < m_items.ToArray().Length && !locker2)
        {

            if (m_items.ToArray()[m_pointTo].description.Substring(5, 3) == "key" || m_items.ToArray()[m_pointTo].description.Substring(4, 3) == "key")
                usedkey = true;
            
            if (m_items.ToArray()[m_pointTo].use())
            {
                locker2 = true;
                //m_items.RemoveAt(m_pointTo);
                m_Cells[m_pointTo].gameObject.GetComponent<Animation>().Play("General_FadeOut");
                GameManager.Instance.m_IsFading = true;
                StartCoroutine(detachSprite(m_pointTo));
            }
        }

        if (GameManager.Instance.m_inventoryIsOpen && Input.GetButtonDown("Use") && GameManager.Instance.hasUpgrade((int)Gadgets.Gadget4) && m_pointTo < m_items.ToArray().Length && !locker)
        {
            locker = true;
            GameManager.Instance.m_inventory[!GameManager.Instance.m_sel_pg ? 0 : 1].GetComponent<Inventory>().addItem(m_items.ToArray()[m_pointTo]);
            //m_items.RemoveAt(m_pointTo);
            GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].GetComponent<ThirdPersonCharacterNostro>().clips[8]);
            m_Cells[m_pointTo].gameObject.GetComponent<Animation>().Play("General_FadeOut");
            StartCoroutine(detachSprite(m_pointTo, true));
        }

        if (Input.GetAxis("InventoryNav") > 0 && !m_pointerIsMoving)
        {
            m_pointerIsMoving = true;
            m_pointTo = (m_pointTo + 1) % Inventory.MAX_CAPACITY;
            StartCoroutine(OneStep());
            if (m_items.ToArray().Length > m_pointTo)
            {
                if (GameManager.Instance.m_sel_pg)
                    descr_b.text = m_items.ToArray()[m_pointTo].description;
                else
                    descr_r.text = m_items.ToArray()[m_pointTo].description;
            }
            else
            {
                if (GameManager.Instance.m_sel_pg)
                    descr_b.text = "Select an item to show a description";
                else
                    descr_r.text = "Select an item to show a description";
            }
        }
        else if (Input.GetAxis("InventoryNav") < 0 && !m_pointerIsMoving)
        {
            m_pointerIsMoving = true;
            m_pointTo = ((--m_pointTo) < 0 ? Inventory.MAX_CAPACITY + m_pointTo : m_pointTo) % Inventory.MAX_CAPACITY;
            StartCoroutine(OneStep());

            if (m_items.ToArray().Length > m_pointTo)
            {
                if (GameManager.Instance.m_sel_pg)
                    descr_b.text = m_items.ToArray()[m_pointTo].description;
                else
                    descr_r.text = m_items.ToArray()[m_pointTo].description;
            }
            else
            {
                if (GameManager.Instance.m_sel_pg)
                    descr_b.text = "Select an item to show a description";
                else
                    descr_r.text = "Select an item to show a description";
            }
        }

        m_Puntator.GetComponent<RectTransform>().anchoredPosition = m_Cells[m_pointTo].gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    IEnumerator detachSprite(int im, bool passToOther = false)
    {
        yield return new WaitUntil(() => !m_Cells[im].GetComponent<Animation>().isPlaying);
        m_Cells[im].sprite = null;
        if (passToOther)
        {
            GameManager.Instance.hideInventory(GameManager.Instance.m_sel_pg ? 0 : 1);

            Camera.main.GetComponent<CoolCameraController>().select_singleView(1 - GameManager.Instance.m_Current_State);
            GameManager.Instance.displayInventory(GameManager.Instance.m_sel_pg ? 0 : 1, 0.5f);
            locker = false;
        }
        else
        {
            locker2 = false;
        }

        m_items.RemoveAt(im);
        GameManager.Instance.m_IsFading = false;
        if (usedkey)
        {
            usedkey = false;
            GameManager.Instance.hideInventory(GameManager.Instance.m_sel_pg ? 0 : 1);
        }

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

    public void updateView()
    {
        if (m_items.ToArray().Length > m_pointTo)
        {
            if (GameManager.Instance.m_sel_pg)
                descr_b.text = m_items.ToArray()[m_pointTo].description;
            else
                descr_r.text = m_items.ToArray()[m_pointTo].description;
        }
        else
        {
            if (GameManager.Instance.m_sel_pg)
                descr_b.text = "Select an item to show a description";
            else
                descr_r.text = "Select an item to show a description";
        }
        m_currentScore.text = m_ncollectables[GameManager.Instance.whichLevelItIs()].ToString();
        /*for(int i=0; i < m_upgradesActivation.Length; i++)
        {
            if (m_upgradesActivation[i]) m_Upgrades[i].sprite = GameManager.Instance.getSprite(i);
            else m_Upgrades[i].sprite = null;
        }*/
        int j;
        for(j=0; j < m_items.Count; j++)
        {
            m_Cells[j].sprite = m_items.ToArray()[j].m_this;
            m_Cells[j].color = new Color(1, 1, 1, 1);
        }
        for(j = m_items.Count; j < m_Cells.Length; j++)
        {
            m_Cells[j].sprite = null;
            m_Cells[j].color = new Color(1, 1, 1, 0);
        }
    }

    public void resetDescription()
    {
        if (m_items.ToArray().Length > m_pointTo)
        {
            if (GameManager.Instance.m_sel_pg)
                descr_b.text = m_items.ToArray()[m_pointTo].description;
            else
                descr_r.text = m_items.ToArray()[m_pointTo].description;
        }
        else
        {
            if (GameManager.Instance.m_sel_pg)
                descr_b.text = "Select an item to show a description";
            else
                descr_r.text = "Select an item to show a description";
        }
    }
}
