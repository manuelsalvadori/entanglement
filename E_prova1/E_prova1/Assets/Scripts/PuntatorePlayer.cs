using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntatorePlayer : MonoBehaviour
{

    public float speed = 50f;
    private float[] start_ray_y = {40f, -20f};
    public bool p1 = true;
    int layermask = 1 << 8;
    Vector3 start;
    MeshRenderer mr;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        layermask = ~layermask;
    }

	void Update ()
    {
        if (GameManager.Instance.m_3D_mode)
        {
            mr.enabled = false;
        }
        else
        {
            mr.enabled = true;
            transform.Rotate(0f, Time.deltaTime * speed, 0f, Space.World);
            transform.position = GameManager.Instance.m_players[p1 ? 1 : 0].transform.position;

            RaycastHit hit;
            start = new Vector3(transform.position.x, GameManager.Instance.m_players[!p1 ? 1 : 0].transform.position.y + 1f, transform.position.z);
            Ray raggio = new Ray(start, Vector3.down);

            Physics.Raycast(raggio, out hit, Mathf.Infinity, layermask);

            if (hit.collider != null)
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + 0.1f, transform.position.z);
            }
        }
    }
}
