using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    private static bool[] m_upgrades = new bool[4] /*{ false, false, false, false }*/{true, true, true, true};
    private List<Item> m_items = new List<Item>();
    private static int[] m_ncollectables = new int[6] { 0, 0, 0, 0, 0, 0 };

    public static readonly int MAX_CAPACITY = 8;


    public enum Gadgets {Gadget1, Gadget2, Gadget3, Gadget4 };

    public Text m_currentScore;
    public Text m_globalScore;
    public Image[] m_Upgrades;
    public Image[] m_Cells;

    void Awake()
    {
        updateView();
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
