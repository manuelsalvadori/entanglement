using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MovableObject : MonoBehaviour
{

    private bool m_isActive = false;
    public float deltay = -14f;
    public Material blue, red;
    Material material_init;

    void Start()
    {
        material_init = GetComponent<Renderer>().material;
    }

    void OnCollisionExit(Collision col)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (m_isActive)
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    public void setActive ()
    {
        m_isActive = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ |RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        Camera.main.GetComponent<CoolCameraController>().followEnemy(gameObject);
        gameObject.layer = 9;
        StartCoroutine(movePistolabile());
    }

    void OnCollisionStay(Collision o)
    {   
        if (Mathf.Abs(o.collider.bounds.min.y - transform.GetComponent<Collider>().bounds.max.y) < 0.02f)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }


    float duration = 4f;
    IEnumerator movePistolabile()
    {
        Camera.main.GetComponent<CoolCameraController>().playerPan = 0f;
        yield return new WaitForSeconds(1f);
        if (GameManager.Instance.m_sel_pg)
            GetComponent<Renderer>().material = blue;
        else
            GetComponent<Renderer>().material = red;
        
        Camera.main.GetComponent<CoolCameraController>().smoothTime = 0.08f;
        Camera.main.GetComponent<BlurOptimized>().enabled = true;
        float start_y = transform.position.y;
        float end_y = start_y + deltay;
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            float yMovement = Mathf.Lerp(start_y, end_y, t / duration);
            transform.position = new Vector3(transform.position.x, yMovement, transform.position.z);
            yield return null;
        }
        Camera.main.GetComponent<CoolCameraController>().smoothTime = 0.3f;
        GetComponent<Renderer>().material = material_init;
        yield return new WaitForSeconds(1f);
        Camera.main.GetComponent<CoolCameraController>().playerPan = 0.15f;
        Camera.main.GetComponent<BlurOptimized>().enabled = false;
        Camera.main.GetComponent<CoolCameraController>().resetFollowing();
        gameObject.layer = 0;
    }
}
