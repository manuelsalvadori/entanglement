using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntatorePlayer : MonoBehaviour
{

    public float speed = 50f;
    private float[] start_ray_y = {40f, -20f};
    public bool p1 = true;

	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(0f, Time.deltaTime * speed, 0f, Space.World);
        transform.position = GameManager.Instance.m_players[p1 ? 1 : 0].transform.position;

        RaycastHit hit;
        Vector3 start = new Vector3(transform.position.x, start_ray_y[p1 ? 0 : 1], transform.position.z);
        Ray raggio = new Ray(start, Vector3.down);

        int layermask = 1 << 8;
        layermask = ~layermask;

        Physics.Raycast(raggio, out hit, Mathf.Infinity, layermask);

        if (hit.collider != null)
        {
            //transform.position = new Vector3(transform.position.x, (hit.collider.bounds.max.y + gameObject.GetComponent<MeshRenderer>().bounds.extents.y) + 0.01f, transform.position.z);
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.1f, transform.position.z);
        }
    }
}
