using UnityEngine;
using System.Collections;

public class Pointing : MonoBehaviour
{
    public float speed = 8f;
    public float[] start_ray_y = {10f, -2f};
    LineRenderer lr;
    public Material[] mat;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        resetPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0:1].transform.position);
    }

    void OnEnable()
    {
        
        resetPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0:1].transform.position);
        RaycastHit hit;
        Vector3 start = new Vector3(transform.position.x, start_ray_y[(GameManager.Instance.m_sel_pg) ? 0 : 1], transform.position.z);
        Ray raggio = new Ray(start, Vector3.down);
        Physics.Raycast(raggio, out hit);

        if (hit.collider != null)
        {
            transform.position = new Vector3(transform.position.x, (hit.collider.bounds.max.y + gameObject.GetComponent<MeshRenderer>().bounds.extents.y),transform.position.z);
        }
           
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos = Camera.main.ViewportToWorldPoint(new Vector3(Mathf.Clamp(pos.x, 0.03f, 0.97f), pos.y, pos.z));

        transform.position = new Vector3(pos.x, transform.position.y, -4.6f);

        StartCoroutine(enableRenderer());
    }

	void Update ()
    {

        float va = (GameManager.Instance.m_inventoryIsOpen) ? 0 : Input.GetAxis("R_Vertical");
        float ha = (GameManager.Instance.m_inventoryIsOpen) ? 0 : Input.GetAxis("R_Horizontal");

        RaycastHit hit;
        Vector3 start = new Vector3(transform.position.x, start_ray_y[(GameManager.Instance.m_sel_pg) ? 0 : 1], transform.position.z);
        Ray raggio = new Ray(start, Vector3.down);
        int layermask = 1 << 8;
        layermask = ~layermask;
        Physics.Raycast(raggio, out hit, Mathf.Infinity, layermask);

        transform.position += (new Vector3(ha, 0f, va).normalized * Time.deltaTime * speed);
        if (hit.collider != null)
        {

            transform.position = new Vector3(transform.position.x, (hit.collider.bounds.max.y + gameObject.GetComponent<MeshRenderer>().bounds.extents.y), transform.position.z);
        }

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos = Camera.main.ViewportToWorldPoint(new Vector3(Mathf.Clamp(pos.x, 0.03f, 0.97f), pos.y, pos.z));

        transform.position = new Vector3(pos.x, transform.position.y, -4.6f);
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0:1].transform.position);
	}

    public void resetPosition(Vector3 posplayer)
    {
        float dir = (GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.rotation.eulerAngles.y >= 0f && GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.rotation.eulerAngles.y < 180f) ? 3.0f : -3.0f;
        transform.position = posplayer + new Vector3(dir,0f,0f);
        if(lr != null)
            lr.material = mat[GameManager.Instance.m_sel_pg ? 0:1];
        GetComponent<Renderer>().material = mat[GameManager.Instance.m_sel_pg ? 0:1];
    }

    void OnDisable()
    {
        GetComponent<Renderer>().enabled = false;
        lr.enabled = false;
    }

    IEnumerator enableRenderer()
    {
        yield return new WaitForSeconds(0.01f);
        GetComponent<Renderer>().enabled = true;
        lr.enabled = true;
    }
}
