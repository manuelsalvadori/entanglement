using UnityEngine;
using System.Collections;

public class PickableObject : MonoBehaviour{

    public bool isCollectable = false;
    public bool isUpgrade = false;
    public float min_distance = 5f;
    public Inventory.Gadgets m_gadget;
    private string m_name;
    public GameObject m_target;
    public Vector3 m_rot = new Vector3(15, 30, 45);
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
        m_this.min_distance = min_distance;
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(m_rot * Time.deltaTime, Space.World);
    }

}