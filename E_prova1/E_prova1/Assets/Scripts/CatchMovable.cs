using UnityEngine;
using System.Collections;

public class CatchMovable : MonoBehaviour
{
    public float deltaMove = 5.0f;
    private Color initColor;
    public Color changedColor;
    void OnCollisionEnter(Collision movable)
    {
        if (movable.gameObject.tag.Equals("Spostabile"))
        {
            Vector3 endPos;
            if(movable.gameObject.transform.parent.tag.Equals("Level2"))
            {
                movable.gameObject.transform.parent = GameObject.FindGameObjectWithTag("Level1").transform;
                endPos = movable.gameObject.transform.position + new Vector3(0f, 0f, transform.position.z + deltaMove);
            }
            else
            {
                movable.gameObject.transform.parent = GameObject.FindGameObjectWithTag("Level2").transform;

                endPos = movable.gameObject.transform.position + new Vector3(0f,0f,transform.position.z - deltaMove);
            }
            StartCoroutine(moveMovable(endPos, movable.gameObject));
            Physics.IgnoreCollision(GetComponent<BoxCollider>(), movable.collider);
            initColor = movable.gameObject.GetComponent<Renderer>().material.color;
            movable.gameObject.layer = 9;
            movable.gameObject.GetComponent<Renderer>().material.color = changedColor;
        }
    }

    private bool isMoving = false;
    private float durations = 0.25f;

    IEnumerator moveMovable(Vector3 endPos, GameObject movable)
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        Vector3 currentpos = movable.transform.position;

        float counter = 0;
        while (counter < durations)
        {
            counter += Time.deltaTime;
            movable.transform.position = Vector3.Lerp(currentpos, endPos, counter / durations);
            yield return null;
        }

        yield return new WaitForSeconds(0.15f);
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), movable.GetComponent<Collider>(), false);
        StartCoroutine(smoothColor(movable));
        movable.layer = 0;
        isMoving = false;
    }
    private float duration = 0.2f;

    IEnumerator smoothColor(GameObject movable)
    {
        float counter = 0;
        while (counter < durations)
        {
            counter += Time.deltaTime;
            movable.GetComponent<Renderer>().material.color = Color.Lerp(changedColor, initColor, counter / duration);
            yield return null;
        }

    }
}

