using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Item{

    private string m_name;
    public Sprite m_this;
    public GameObject m_target;
    public float min_distance = 5f;

    public Item(string name)
    {
        this.m_name = name;
    }

    public bool use()
    {

        Vector3 p_pos = GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.position;
        Debug.Log("use fuori "+ (p_pos - m_target.transform.position).magnitude);

        if((p_pos - m_target.transform.position).magnitude < min_distance)
        {
            Debug.Log("use");
            m_target.GetComponent<ActivateButton>().unlock();
            return true;
        }
        else
        {
            UIGameplayManager.Instance.displayMessage("C'è luogo e momento per ogni cosa ma non ora!");
            return false;
        }
    }

    public bool testTarget(GameObject other)
    {
        return this.m_target == other;
    }

    public override string ToString()
    {
        return m_name;
    }

}
