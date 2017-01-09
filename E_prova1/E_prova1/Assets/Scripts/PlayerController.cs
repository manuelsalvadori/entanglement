using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float smoothTime = 0.3f;
    private bool firstDash = true;

    public void Update()
    {
        if (GetComponent<ThirdPersonCharacterNostro>().m_IsGrounded)
            firstDash = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        bool pick_item = true;
        if (GameManager.isPickble(other.gameObject))
        {
            if (other.gameObject.GetComponent<PickableObject>().isCollectable)
            {
                GetComponent<AudioSource>().PlayOneShot(GetComponent<ThirdPersonCharacterNostro>().clips[11], 1f);
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
                GameManager.Instance.gainUpgrade((int)other.gameObject.GetComponent<PickableObject>().m_gadget);
            }
            if (pick_item)
                other.gameObject.SetActive(false);
            GameManager.Instance.m_inventory[GameManager.Instance.whoAmI(this.name)].GetComponent<Inventory>().updateView();
        }

        if (other.tag.Equals("LaMuerte"))
        {
            foreach (Renderer j in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (!j.gameObject.name.Equals("trail"))
                {
                    j.enabled = false;
                }
            }
            GetComponent<AudioSource>().PlayOneShot(GetComponent<ThirdPersonCharacterNostro>().clips[10], 0.8f);
            StartCoroutine(waitForDeath());
        }

        if (other.tag.Equals("LaMuerte2"))
        {
            foreach (Renderer j in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (!j.gameObject.name.Equals("trail"))
                {
                    j.enabled = false;
                }
            }
            GetComponent<AudioSource>().PlayOneShot(GetComponent<ThirdPersonCharacterNostro>().clips[10], 0.8f);
            StartCoroutine(waitForDeath());
        }

        if (other.tag.Equals("Checkpoint"))
        {
            GameManager.Instance.updateCheckpoint(other.transform);
        }
    }

    public bool teleportAllowed = false;
    public bool gunAllowed = false;
    public void useGadget(int n)
    {
        switch (n)
        {
            case 0:
                if (gunAllowed)
                    moveObject();
                else
                {
                    switchGun(true);
                    gunAllowed = true;
                }
                break;
            case 1:
                if (teleportAllowed)
                    teleport();
                else
                {
                    switchMirino(true);
                    teleportAllowed = true;
                }
                break;
            case 2:
                if (GetComponent<ThirdPersonCharacterNostro>().m_IsGrounded)
                    dash();
                else
                {
                    if (firstDash)
                    {
                        dash();
                        firstDash = false;
                    }
                }
                break;
            default:
                return;
        }

    }

    private void teleport()
    {
        RaycastHit hit;
        Vector3 start = transform.position +  new Vector3(0f, gameObject.GetComponent<CharacterController>().height/2f, 0f);
        Vector3 direction = GameManager.Instance.mirino.transform.position - transform.position;
        Ray raggio = new Ray(start, direction);
        int layermask = 1 << 8;

        if (Physics.Raycast(raggio, out hit, Mathf.Abs(direction.magnitude), layermask))
        {
            #if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(start + (Vector3.up * 0.2f), hit.point);
            #endif
            StartCoroutine(stoppedTeleport(hit));
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(GetComponent<ThirdPersonCharacterNostro>().clips[7], 0.8f);
            transform.position = GameManager.Instance.mirino.transform.position + new Vector3(0f, gameObject.GetComponent<CapsuleCollider>().bounds.extents.y, 0f);
        }
        GameManager.Instance.mirino.GetComponent<Pointing>().resetPosition(transform.position);
        switchMirino(false);
        teleportAllowed = false;
    }

    private void dash()
    {
        transform.GetChild(0).gameObject.SetActive(true);

        if(transform.rotation.eulerAngles.y >= 0f && transform.rotation.eulerAngles.y < 180f)
            StartCoroutine(dashGate(1.0f));
        else
            StartCoroutine(dashGate(-1.0f));
    }

    private bool isdashing = false;
    private float durations = 0.15f;
    IEnumerator dashGate(float direction)
    {
        if (isdashing)
        {
            yield break;
        }
        isdashing = true;
        GetComponent<AudioSource>().PlayOneShot(GetComponent<ThirdPersonCharacterNostro>().clips[9], 0.8f);
        Vector3 currentpos = transform.position;

        float counter = 0;
        while (counter < durations)
        {
            counter += Time.deltaTime;
            transform.position = Vector3.Lerp(currentpos, currentpos + new Vector3(6f*direction,0f,0f), counter / durations);
            yield return null;
        }
        yield return new WaitForSeconds(0.15f);
        transform.GetChild(0).gameObject.SetActive(false);

        isdashing = false;
    }

    void switchMirino(bool enabled)
    {
        GameManager.Instance.mirino.SetActive(enabled);
        if(enabled)
            GameManager.Instance.mirino.GetComponent<Pointing>().resetPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
    }

    IEnumerator stoppedTeleport(RaycastHit hit)
    {
        yield return new WaitForSeconds(0.05f);
        Debug.Log(hit.collider.gameObject.name);
        float diff = (transform.position.x > hit.collider.gameObject.transform.position.x) ? hit.collider.bounds.max.x : hit.collider.bounds.min.x;
        float meno = (transform.position.x > hit.collider.gameObject.transform.position.x) ? 1.0f : -1.0f;

        transform.position = new Vector3(diff, transform.position.y, transform.position.z) + new Vector3(meno * gameObject.GetComponent<CapsuleCollider>().bounds.extents.x +0.01f, gameObject.GetComponent<CapsuleCollider>().bounds.extents.y+0.01f, 0f);

    }

    void switchGun(bool enabled)
    {
        GameManager.Instance.pistola.SetActive(enabled);
        if(enabled)
            GameManager.Instance.mirino.GetComponent<Pointing>().resetPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position + new Vector3(0f, GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<CharacterController>().height * 2 / 3, 0f));
    }

    private void moveObject()
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<ThirdPersonCharacterNostro>().clips[6], 0.8f);

        if (GameManager.Instance.pistola.GetComponent<shoot>().hit.collider.tag.Equals("Pistolabile"))
        {
            GameManager.Instance.pistola.GetComponent<shoot>().hit.collider.gameObject.GetComponent<MovableObject>().setActive();
        }
        GameManager.Instance.pistola.GetComponent<shoot>().resetShootPosition(transform.position);
        switchGun(false);
        gunAllowed = false;
    }

    IEnumerator waitForDeath()
    {
        yield return new WaitForSeconds(1f);

        GameManager.Instance.onDeathPlayer(GameManager.Instance.whoAmI(gameObject.name));
        foreach (Renderer j in gameObject.GetComponentsInChildren<Renderer>())
        {
            if (!j.gameObject.name.Equals("trail"))
            {
                j.enabled = true;
            }
        }
    }

    /*
    GameObject culledObject = null;
    bool culled = false;
    void playerCulled()
    {
        RaycastHit hit;
        Vector3 start = Camera.main.transform.position;
        Vector3 direction = (transform.position - Camera.main.transform.position);
        Ray raggio = new Ray(start, direction);
        Physics.Raycast(raggio, out hit);

        if (!hit.collider.tag.Equals("Player") && !hit.collider.tag.Equals("Spostabile") && !hit.collider.tag.Equals("Checkpoint") && !hit.collider.tag.Equals("LaMuerte") && !hit.collider.tag.Equals("LaMuerte2") && !hit.collider.tag.Equals("World"))
        {
            if (!hit.collider.gameObject.GetComponent<Renderer>())
                return;
            Material m = hit.collider.gameObject.GetComponent<Renderer>().material;
            m.SetFloat("_Mode", 3f);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = 3000;
            culledObject = hit.collider.gameObject;
            culled = true;
        }
        else
        {
            if (culled)
            {
                culledObject.GetComponent<Renderer>().material.SetFloat("_Mode", 0f);
                culledObject.GetComponent<Renderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                culledObject.GetComponent<Renderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                culledObject.GetComponent<Renderer>().material.SetInt("_ZWrite", 1);
                culledObject.GetComponent<Renderer>().material.DisableKeyword("_ALPHATEST_ON");
                culledObject.GetComponent<Renderer>().material.DisableKeyword("_ALPHABLEND_ON");
                culledObject.GetComponent<Renderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                culledObject.GetComponent<Renderer>().material.renderQueue = -1;
                culled = false;
            }
        }

    }
    */
}