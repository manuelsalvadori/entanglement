using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CoolCameraController : MonoBehaviour
{
    public GameObject cameradx;
    //Constant Level and Cam Geometry
    //public Vector3[] m_Level_Position;
    //public Vector3[] m_Wall_Position;
    private Vector3[] m_View;
    public float m_heigthOffset = 2f;
    public float m_BackOffset = 2.5f;
    public float m_LateralOffset = 2f;
    public float m_LateralRotation = 30f;
    //public float m_WallOffset = -0.44f;
    public float m_z3d = 0f, m_y3d = 29f;
    //~

    private bool m_followEnemy = false;
    private GameObject m_Enemy;

    private Vector3 m_SlideAmount;
    private Vector3 m_CameraTarget;                                     //Posizione di Arrivo
    private Vector3 m_CameraRotation;                                   //Rotazione di Arrivo
    //private Vector3 m_LevelTarget;                                      //Posizione di Arrivo del livello

    public enum Stato { First_Player, Second_Player, Doppia, TreD};
    private int m_oldState;

    //SmoothDump setting function
    //private Vector3 velocity = Vector3.zero;
    private Vector3 velocity2 = Vector3.zero;
    private Vector3 velocity4 = Vector3.zero;
    private float velocityfloat = 0f;
    public float smoothTime = 0.3F;                                     //Amount of smooth
    public float followingSpeed = 1.5f;
    public float playerPan = 0.15f;
    //~

    //Lerp Level1 setting function
    //public float m_Level_speed = 10F;
    //Players and Levels infos
    //Editor view
    //public Transform m_l1, m_l2;

    public Shader shaderBlue;
    public Shader shaderRed;
    public Material SkyboxBlue;
    public Material SkyboxRed;


#if UNITY_STANDALONE_WIN
    public bool waitForMove = false;
#endif

    private Dictionary<int, Vector3> m_player_position = new Dictionary<int, Vector3>() //easy way to select player initial infos.
    {
        {0, Vector3.zero},
        {1, Vector3.zero},
    };
    //~


    public void followEnemy(GameObject g)
    {
        m_Enemy = g;
        m_followEnemy = true;
    }

    public void resetFollowing()
    {
        m_followEnemy = false;
    }


    private Vector3 m_offset_from_players;

    // Use this for initialization
    void Start () {
        GameManager.Instance.m_Current_State = m_oldState = (int)Stato.First_Player;

        m_SlideAmount = new Vector3(0, 0, 0);

        m_player_position[0] = GameManager.Instance.m_players[0].transform.position;
        m_player_position[1] = GameManager.Instance.m_players[1].transform.position;

        /*
        m_l1 = GameManager.Instance.m_level1.transform;
        m_l2 = GameManager.Instance.m_level2.transform;

        m_Level_Position = new Vector3[3];
        m_Level_Position[0] = m_Level_Position[1] = m_Level_Position[2] = GameManager.Instance.m_level1.transform.position;
        m_Level_Position[1].z = m_Level_Position[2].z = 10.5f;
        m_Level_Position[2].y = -5f;

        m_Wall_Position = new Vector3[2];
        m_Wall_Position[0] = GameObject.FindGameObjectsWithTag("Wall")[1].GetComponent<MeshFilter>().transform.localPosition;
        m_Wall_Position[1] = new Vector3(GameObject.FindGameObjectsWithTag("Wall")[1].transform.localPosition.x, m_WallOffset, GameObject.FindGameObjectsWithTag("Wall")[1].GetComponent<MeshFilter>().transform.localPosition.z);
        */

        m_View = new Vector3[4];
        m_View[0] = new Vector3(0f, GameManager.Instance.m_level1.transform.position.y, -20f);
        m_View[1] = new Vector3(0f, GameManager.Instance.m_level2.transform.position.y, -20f);
        m_View[2] = new Vector3(0f, 0f, -29f);
        m_View[3] = new Vector3(m_player_position[1].x, m_y3d, m_z3d);


        Vector3 player_pos = Camera.main.WorldToViewportPoint(m_player_position[0]);
        player_pos.x += playerPan;
        player_pos = Camera.main.ViewportToWorldPoint(player_pos);

        m_offset_from_players = m_View[3] - new Vector3(m_player_position[0].x, 0f, 0f);

        m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
        m_CameraTarget.x = player_pos.x;
    }

	// Update is called once per frame
	void Update () {
        Debug.Log(Input.GetAxisRaw("ChangeLevel"));

        if (Input.GetButtonDown("Level1") && !GameManager.Instance.m_inventoryIsOpen && GameManager.Instance.m_currentLevel != 0 && !GameManager.Instance.m_IsWindowOver) //single_view
        {
            select_singleView(0);
        }

        if (Input.GetButtonDown("Level2") && !GameManager.Instance.m_3D_mode && !GameManager.Instance.m_inventoryIsOpen && GameManager.Instance.m_currentLevel != 0 && !GameManager.Instance.m_IsWindowOver) //double_view
        {
            select_singleView(1);
        }


        #if UNITY_STANDALONE_WIN

        if((Input.GetButton("ChangeLevel") || Input.GetAxis("ChangeLevel") > 0.3) && !waitForMove && !GameManager.Instance.m_3D_mode && !GameManager.Instance.m_inventoryIsOpen && GameManager.Instance.m_currentLevel != 0 && !GameManager.Instance.m_IsWindowOver)
        {
            waitForMove = true;
            select_singleView(GameManager.Instance.m_sel_pg ? 1 : 0);
            StartCoroutine(waitCamera());
        }

        if ((Input.GetButtonDown("3Dmode") || Input.GetAxis("ChangeLevel") < -0.3) && !waitForMove && !GameManager.Instance.m_inventoryIsOpen && GameManager.Instance.m_currentLevel != 0 && !GameManager.Instance.m_IsWindowOver) //3D_view
        {
            if (GameManager.Instance.isPlayersInline() || GameManager.Instance.m_3D_mode)
            {
                waitForMove = true;
                select_treD_View(); //else UIGameplayManager.Instance.displayMessage("Non è stato possibile stabilire il contatto.");
                StartCoroutine(waitCamera());
            }
        }

        #endif

        #if UNITY_STANDALONE_OSX

        if(Input.GetButton("ChangeLevel") && !GameManager.Instance.m_3D_mode && !GameManager.Instance.m_inventoryIsOpen && GameManager.Instance.m_currentLevel != 0 && !GameManager.Instance.m_IsWindowOver)
        {
            select_singleView(GameManager.Instance.m_sel_pg ? 1 : 0);
        }

        if(Input.GetButtonDown("3Dmode") && !GameManager.Instance.m_inventoryIsOpen && GameManager.Instance.m_currentLevel != 0 && !GameManager.Instance.m_IsWindowOver) //3D_view
        {
            if (GameManager.Instance.isPlayersInline() || GameManager.Instance.m_3D_mode) select_treD_View(); //else UIGameplayManager.Instance.displayMessage("Non è stato possibile stabilire il contatto.");
        }

        #endif

        if (GameManager.Instance.m_Current_State != (int)Stato.TreD)
        {
            Vector3 player_pos = Camera.main.WorldToViewportPoint(m_followEnemy ? m_Enemy.transform.position : GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.position);
            player_pos.x += playerPan;
            player_pos = Camera.main.ViewportToWorldPoint(player_pos);
            m_CameraTarget.x = player_pos.x;
            m_CameraTarget.y = player_pos.y + m_heigthOffset;

        }
        else
        {
            m_CameraTarget = m_offset_from_players + new Vector3(GameManager.Instance.m_players[0].transform.position.x, GameManager.Instance.m_players[0].transform.position.y, GameManager.Instance.m_players[0].transform.position.z);

            float h = Input.GetAxis("CameraPan");
            m_SlideAmount = new Vector3(0, 0, -h * m_LateralOffset);
            if (h == 1 || h == -1)
                m_CameraRotation = new Vector3(28f, 90f + (-m_LateralRotation * h), 0);
            else
                m_CameraRotation = new Vector3(m_CameraRotation.x, 90, m_CameraRotation.z);
            m_CameraTarget = m_CameraTarget + m_SlideAmount;
        }
    }


    IEnumerator waitCamera()
    {
        yield return new WaitForSeconds(1f);
        waitForMove = false;
    }


    void LateUpdate()
    {
        //Debug.Log(m_CameraTarget + " e " + m_offset_from_players);
        //Move the camera (SmoothDamp version)
        transform.position = Vector3.SmoothDamp(transform.position, m_CameraTarget + (GameManager.Instance.m_Current_State == (int)CoolCameraController.Stato.TreD ? new Vector3(-1f,3f,0f) : Vector3.zero), ref velocity4, smoothTime);
        float ydx = Mathf.SmoothDamp(cameradx.transform.position.y, GameManager.Instance.m_players[!GameManager.Instance.m_sel_pg ? 0 : 1].transform.position.y + m_heigthOffset + (GameManager.Instance.m_Current_State == (int) CoolCameraController.Stato.TreD ? 3 : 0) , ref velocityfloat, smoothTime);

        transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, m_CameraRotation, ref velocity2, smoothTime));
        cameradx.transform.position = new Vector3(transform.position.x, ydx , transform.position.z);
        cameradx.transform.rotation = transform.rotation;

        if (transform.rotation.eulerAngles.y > 88f && transform.rotation.eulerAngles.y < 90f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 0.02f, transform.rotation.eulerAngles.z));
        }

        if (Mathf.Abs((transform.position - m_CameraTarget).magnitude) < 0.005f)
            transform.position = m_CameraTarget;
    }


    public void select_singleView(int player)
    {
        switch (GameManager.Instance.m_Current_State)
        {
            case 0:

            case 1:
                if (player == GameManager.Instance.m_Current_State)
                    return;
                m_oldState = GameManager.Instance.m_Current_State;
                GameManager.Instance.m_Current_State = 1 - GameManager.Instance.m_Current_State;
                GameManager.Instance.m_sel_pg = !GameManager.Instance.m_sel_pg;
                GameManager.Instance.m_gadgetSelection[0].switchSelectionUI();
                GameManager.Instance.m_gadgetSelection[1].GetComponent<SwitchGadget>().switchSelectionUI();
                GameManager.Instance.mirino.GetComponent<Pointing>().resetPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position + new Vector3(0f, GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<CharacterController>().height * 2 / 3, 0f));
                GameManager.Instance.pistola.GetComponent<shoot>().resetShootPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position + new Vector3(0f, GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<CharacterController>().height * 2 / 3, 0f));
                m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
                m_CameraTarget.y = m_player_position[GameManager.Instance.m_Current_State].y;
                break;
            case 2:
                m_oldState = GameManager.Instance.m_Current_State;
                GameManager.Instance.m_Current_State = GameManager.Instance.m_sel_pg ? 0 : 1;
                m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
                break;
            case 3:
                return;
            default:
                return;
        }

    }

    private void select_doubleView()
    {
        //salva lo stato precedente ricordati
        switch (GameManager.Instance.m_Current_State)
        {
            case 0:
            case 1:
                m_oldState = GameManager.Instance.m_Current_State;
                GameManager.Instance.m_Current_State = (int)Stato.Doppia;
                m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
                break;
            case 2:
                GameManager.Instance.m_Current_State = GameManager.Instance.m_sel_pg ? 0 : 1;
                m_oldState = (int)Stato.Doppia;
                m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
                break;
            case 3:
                return;
            default:
                return;
        }
    }

    private void select_treD_View()
    {
        if(GameManager.Instance.m_Current_State != (int)Stato.TreD)
        {
            foreach(GlobalFog fog in GetComponents<GlobalFog>())
            {
                if (GameManager.Instance.m_sel_pg)
                {
                    fog.fogShader = shaderBlue;
                    foreach(GlobalFog fog_2 in cameradx.GetComponents<GlobalFog>()) fog_2.fogShader = shaderRed;
                    GetComponent<Skybox>().material = SkyboxBlue;
                    cameradx.GetComponent<Skybox>().material = SkyboxRed;
                }
                else
                {
                    fog.fogShader = shaderRed;
                    foreach (GlobalFog fog_2 in cameradx.GetComponents<GlobalFog>()) fog_2.fogShader = shaderBlue;
                    GetComponent<Skybox>().material = SkyboxRed;
                    cameradx.GetComponent<Skybox>().material = SkyboxBlue;
                }
                fog.enabled = true;
            }
            cameradx.SetActive(true);
            if (GameManager.Instance.m_sel_pg)
            {
                cameradx.GetComponent<Animation>()["CameraDX1"].speed = 1f;
                cameradx.GetComponent<Animation>()["CameraDX1"].time = 0f;
                GetComponent<Animation>()["Camera3D1"].speed = 1f;
                GetComponent<Animation>()["Camera3D1"].time = 0f;

                cameradx.GetComponent<Animation>().Play("CameraDX1");
                GetComponent<Animation>().Play("Camera3D1");
            }
            else
            {
                cameradx.GetComponent<Animation>()["CameraDX2"].speed = 1f;
                cameradx.GetComponent<Animation>()["CameraDX2"].time = 0f;
                GetComponent<Animation>()["Camera3D2"].speed = 1f;
                GetComponent<Animation>()["Camera3D2"].time = 0f;
                cameradx.GetComponent<Animation>().Play("CameraDX2");
                GetComponent<Animation>().Play("Camera3D2");
            }
            m_oldState =  GameManager.Instance.m_Current_State;
            GameManager.Instance.m_Current_State = (int)Stato.TreD;
            GameManager.Instance.m_3D_mode = true;
            GameManager.Instance.m_camIsMoving = true;
            m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
            m_CameraTarget.y = GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.position.y + m_heigthOffset;
            m_CameraTarget.x = GameManager.Instance.m_players[0].transform.position.x - m_BackOffset;
            m_CameraTarget.z = GameManager.Instance.m_players[0].transform.position.z;
            m_CameraRotation = new Vector3(28f, 90f, 0f);

            m_offset_from_players = m_CameraTarget - new Vector3(GameManager.Instance.m_players[0].transform.position.x, GameManager.Instance.m_players[0].transform.position.y, GameManager.Instance.m_players[0].transform.position.z);

            GameManager.Instance.m_gadgetSelection[(GameManager.Instance.m_sel_pg) ? 0 : 1].hideSelectionUI();
            //StartCoroutine(GameManager.activateChildMode());
            StartCoroutine(GameManager.alinePlayers());
        }
        else
        {
            if (GameManager.Instance.m_sel_pg)
            {
                cameradx.GetComponent<Animation>()["CameraDX1"].speed = -1f;
                cameradx.GetComponent<Animation>()["CameraDX1"].time = cameradx.GetComponent<Animation>()["CameraDX1"].length;

                GetComponent<Animation>()["Camera3D1"].speed = -1f;
                GetComponent<Animation>()["Camera3D1"].time = GetComponent<Animation>()["Camera3D1"].length;

                cameradx.GetComponent<Animation>().Play("CameraDX1");
                GetComponent<Animation>().Play("Camera3D1");
            }
            else
            {
                cameradx.GetComponent<Animation>()["CameraDX2"].speed = -1f;
                cameradx.GetComponent<Animation>()["CameraDX2"].time = cameradx.GetComponent<Animation>()["CameraDX1"].length;
                GetComponent<Animation>()["Camera3D2"].speed = -1f;
                GetComponent<Animation>()["Camera3D2"].time = GetComponent<Animation>()["Camera3D1"].length;

                cameradx.GetComponent<Animation>().Play("CameraDX2");
                GetComponent<Animation>().Play("Camera3D2");
            }
            StartCoroutine(GameManager.Instance.shutdown_thisWin(cameradx));
            StartCoroutine(shutFog());
            int tmp = m_oldState;
            m_oldState = GameManager.Instance.m_Current_State;
            GameManager.Instance.m_Current_State = tmp;
            GameManager.Instance.m_3D_mode = false;
            GameManager.Instance.m_camIsMoving = true;
            m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
            m_CameraRotation = Vector3.zero;

            //GameManager.deactivateChildMode();
            GameManager.Instance.m_gadgetSelection[(GameManager.Instance.m_sel_pg) ? 0 : 1].unhideSelectionUI();
        }
    }

    IEnumerator shutFog()
    {
        yield return new WaitUntil(() => !cameradx.activeSelf);
        foreach (GlobalFog fog in GetComponents<GlobalFog>())
        {
            fog.enabled = false;
        }
    }

    public void changeOldState(int state)
    {
        m_oldState = state;
    }
}
