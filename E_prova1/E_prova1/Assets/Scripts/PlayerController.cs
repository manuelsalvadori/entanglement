﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float m_v, m_h, m_force = 10f, m_jump = 10f, m_GravityMultiplier = 3f;
    public float m_Zfixed = -4.6f;
    public float smoothTime = 0.3F;                                     //Amount of smooth
    Rigidbody m_rb;

    public float m_velocity_boundary = 10f;

    bool m_grounded = true;
    Camera cam;

    void Start ()
    {
        m_rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}

    void FixedUpdate ()
    {
        if ((GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].name.Equals(this.gameObject.name) && !GameManager.Instance.m_inventoryIsOpen) || GameManager.Instance.m_3D_mode)
        {
            //m_grounded = (Mathf.Abs(m_rb.velocity.y) < 0.005f); <-- Deprecated
            m_v = Input.GetAxis("Horizontal");
            m_h = Input.GetAxis("Vertical");

            if (GameManager.Instance.m_camIsMoving) //During a transition between camera's modes player can't move
                    m_h = m_v = 0f;

            //Convert movement in rotation behaviour: when a player says to go in one direction the gameobject have to rotate in this direction and move forward!
            Quaternion m_look = transform.rotation;
            Vector3 move = new Vector3(m_v, 0f, m_h);
            if (move.magnitude > 1)
                move = move.normalized;
            Vector3 force = cam.transform.TransformDirection(move * m_force);
            force.y = 0f;


            //Move
            m_rb.AddForce(force);


            //Constraint velocity
            if (Mathf.Abs(m_rb.velocity.z) > m_velocity_boundary)
                m_rb.velocity = new Vector3(m_rb.velocity.x, m_rb.velocity.y, Mathf.Clamp(m_rb.velocity.z, -(m_velocity_boundary), m_velocity_boundary));

            if (Mathf.Abs(m_rb.velocity.x) > m_velocity_boundary)
                m_rb.velocity = new Vector3(Mathf.Clamp(m_rb.velocity.x, -(m_velocity_boundary), m_velocity_boundary), m_rb.velocity.y, m_rb.velocity.z);

            //
            if (force.magnitude != 0)
                m_look = Quaternion.LookRotation(force.normalized);

            StartCoroutine(rotatePlayer(m_look, 0.1f));

            //Jump if is touching the "Ground"
            if ((!Input.GetButton("L2") && Input.GetButtonDown("Jump")) && m_grounded)
            {
                m_rb.velocity = new Vector3(m_rb.velocity.x, m_jump, m_rb.velocity.z);
            }

            //Increase gravity in order to fall faster
            if (m_rb.velocity.y < 0)
            {
                Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
                m_rb.AddForce(extraGravityForce);
            }
        }
	}

    void LateUpdate()
    {
        //Fix Z motion
        if (!GameManager.Instance.m_3D_mode && !GameManager.Instance.m_camIsMoving)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, m_Zfixed);
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        bool pick_item = true;
        if (GameManager.isPickble(other.gameObject))
        {

            if (other.gameObject.GetComponent<PickableObject>().isCollectable)
            {
                Inventory.score(GameManager.Instance.whichLevelItIs());
            }
            else if(!other.gameObject.GetComponent<PickableObject>().isUpgrade)
            {
                if (GameManager.Instance.m_inventory[GameManager.Instance.whoAmI(this.name)].GetComponent<Inventory>().getItems().Count < Inventory.MAX_CAPACITY)
                {
                    Item pk = other.gameObject.GetComponent<PickableObject>().getItem();
                    GameManager.Instance.m_inventory[GameManager.Instance.whoAmI(this.name)].GetComponent<Inventory>().addItem(pk);
                }
                else
                    pick_item = false;
            }
            else
            {
                Inventory.gainUpgrade((int)other.gameObject.GetComponent<PickableObject>().m_gadget);
            }
            if (pick_item)
                other.gameObject.SetActive(false);
            GameManager.Instance.m_inventory[GameManager.Instance.whoAmI(this.name)].GetComponent<Inventory>().updateView();
        }
    }

    public void OnCollisionStay(Collision other)
    {
        //If this gameobject is touching the "Ground" it can jump
        if (other.gameObject.tag.Equals("Ground"))
            m_grounded = true;
    }

    public void OnCollisionExit(Collision other)
    {
        //If this gameobject is not touching anymore the "Ground" it can't jump
        if (other.gameObject.tag.Equals("Ground"))
            m_grounded = false;
    }

    private bool rotating = false;
    IEnumerator rotatePlayer(Quaternion newRot, float duration)
    {
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Quaternion currentRot = transform.rotation;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
        rotating = false;
    }

    public void useGadget(int n)
    {
        Debug.Log(gameObject.name + " usa gadget "+n);
        switch (n)
        {
            case 0:
                moveObject();
                break;
            case 1:
                teleport();
                break;
            case 2:
                if(transform.rotation.eulerAngles.y >= 0f && transform.rotation.eulerAngles.y < 180f)
                    StartCoroutine(dashGate(1.0f));
                else
                    StartCoroutine(dashGate(-1.0f));
                break;
            default:
                return;
        }

    }

    void moveObject()
    {
        Debug.Log(" usa gadget muovi oggetto tra i livelli");
    }

    void teleport()
    {
        Debug.Log(" usa gadget teletrasporto");
        transform.position = GameObject.Find("Mirino").transform.position + new Vector3(0f,gameObject.GetComponent<CapsuleCollider>().bounds.extents.y,0f);
    }

    private bool isdashing = false;
    private float durations = 0.2f;
    IEnumerator dashGate(float direction)
    {
        Debug.Log(" usa gadget passa attraverso cancelli elettrici");
        if (isdashing)
        {
            yield break;
        }
        isdashing = true;

        Vector3 currentpos = transform.position;

        float counter = 0;
        while (counter < durations)
        {
            counter += Time.deltaTime;
            transform.position = Vector3.Lerp(currentpos, currentpos + new Vector3(6f*direction,0f,0f), counter / durations);
            yield return null;
        }
        isdashing = false;
    }
}
