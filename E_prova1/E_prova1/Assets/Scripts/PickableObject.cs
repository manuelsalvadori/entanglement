using UnityEngine;
using System.Collections;

public class PickableObject : MonoBehaviour{

    public bool isCollectable = false;
    public bool isUpgrade = false;
    public Inventory.Gadgets m_gadget;
    private string m_name;
    public GameObject m_target;

    private Item m_this;

    public Sprite m_spriteTmp;

    public Item getItem()
    {
        return m_this;
    }

    void Awake()
    {
        m_this = new Item(gameObject.name);
    }

    void Start()
    {
        m_this.m_this = m_spriteTmp;
        m_this.m_target = m_target;
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

}