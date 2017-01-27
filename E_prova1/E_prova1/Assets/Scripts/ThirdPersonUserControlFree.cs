using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.ImageEffects;


[RequireComponent(typeof (ThirdPersonCharacterFree))]
public class ThirdPersonUserControlFree : MonoBehaviour
{
    private ThirdPersonCharacterFree m_Character; // A reference to the ThirdPersonCharacter on the object
    public Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.


    public GameObject gameMenuUI;

    private void Start()
    {


        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacterFree>();
        m_Character.m_deltaTime = Time.time;
        m_Character.m_passedTime = Time.time;

    }


    private void Update()
    {

        if (Input.GetButtonDown("GameMenu"))
        {
            Camera.main.GetComponent<BlurOptimized>().enabled = true;
            gameMenuUI.SetActive(true);
            Time.timeScale = 0;
        }

        if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
    }

    /*
    private void FixedUpdate()
    {
        m_Character.m_deltaTime = Time.time - m_Character.m_passedTime;
        if (GameManager.Instance.whoAmI(gameObject.name) == GameManager.Instance.m_lockedPlayer)
            m_Character.isLinked = true;
        else
        {
            m_Character.isLinked = false;
        }
        m_Character.m_passedTime = Time.time;
    }
    */

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {

        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        if (v < 0)
            v = 0;
        bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v*m_CamForward + h*m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v*Vector3.forward + h*Vector3.right;
        }
#if !MOBILE_INPUT
		// walk speed multiplier
	    if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

        // pass all parameters to the character control script

        StartCoroutine(movePlayer(m_Move, crouch, m_Jump));
        //m_Character.Move(m_Move, crouch, m_Jump);


        //Debug.Log(GetComponent<Rigidbody>().velocity.y);


        m_Jump = false;

    }



    IEnumerator changeGround(float f, float time = 0.2f)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Fall!");
        m_Character.setGroundDistance(f);
    }


    IEnumerator movePlayer(Vector3 m_Move, bool crouch, bool m_Jump)
    {
        yield return new WaitForFixedUpdate();
        m_Character.Move(m_Move, crouch, m_Jump);

    }

}

