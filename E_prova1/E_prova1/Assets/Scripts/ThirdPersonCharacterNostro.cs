using UnityEngine;
using System.Collections;


//[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacterNostro : MonoBehaviour
{
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 180;
	[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
	[SerializeField] float m_MoveSpeedMultiplier = 1f;
	[SerializeField] float m_AnimSpeedMultiplier = 1f;
	[SerializeField] float m_GroundCheckDistance = 0.1f;

	Rigidbody m_Rigidbody;
	Animator m_Animator;
	public bool m_IsGrounded;
	float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_TurnAmount;
	float m_ForwardAmount;
	Vector3 m_GroundNormal;
	float m_CapsuleHeight;
	Vector3 m_CapsuleCenter;
	CharacterController m_Capsule;
	bool m_Crouching;



    public LinkedPlayer link;
    private Vector3 previousPos;
    public float m_JumpTiming;
    public float m_JumpHeight;
    public float m_JumpForwardAmount;
    [SerializeField] float m_FallingSpeed = 12f;
    public float pushPower = 2.0F;


    private float fallSpeed;

    private float oscillazione;
    private float startJump = 0;
    private bool m_IsJumping = false;

    private bool m_IsFalling = false;
    private float startFall = 0;
    private float currentHeight = 0;

    public bool anim_fall = false;

    private static bool locker = false;
    public bool isLinked = false;

    public float m_deltaTime = 0f;
    public float m_passedTime = 0f;

    public AudioClip[] clips;
    bool isSounding = false;

    void Start()
	{
		m_Animator = GetComponent<Animator>();
		//m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CharacterController>();
        m_Capsule.detectCollisions = false;
		m_CapsuleHeight = m_Capsule.height;
		m_CapsuleCenter = m_Capsule.center;

        clips = new AudioClip[10];

        for (int i = 0; i < 4; i++)
            clips[i] = Resources.Load("Audio/Footstep" + i.ToString("00")) as AudioClip;

        clips[4] = Resources.Load("Audio/Jump") as AudioClip;
        clips[5] = Resources.Load("Audio/Land") as AudioClip;
        clips[9] = Resources.Load("Audio/Dash") as AudioClip;
        clips[6] = Resources.Load("Audio/Gun") as AudioClip;
        clips[7] = Resources.Load("Audio/Teleport") as AudioClip;
        clips[8] = Resources.Load("Audio/Incubator") as AudioClip;



        //m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
	}


    public void Move(Vector3 move, bool crouch, bool jump)
	{
        m_deltaTime = Time.time - m_passedTime;

		// convert the world relative moveInput vector into a local-relative
		// turn amount and forward amount required to head in the desired
		// direction.
		if (move.magnitude > 1f) move.Normalize();
		move = transform.InverseTransformDirection(move);
		CheckGroundStatus();
		move = Vector3.ProjectOnPlane(move, m_GroundNormal);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;

        //Debug.Log("delta time: " + Time.deltaTime + " fixed delta time" + Time.fixedDeltaTime + " Calculated delta time: " + m_deltaTime);
		ApplyExtraTurnRotation();

		// control and velocity handling is different when grounded and airborne:
		if (m_IsGrounded)
		{
            startJump = 0;
			HandleGroundedMovement(crouch, jump);
        }
		else
		{
            startJump += m_deltaTime*m_JumpTiming;
            HandleAirborneMovement();
		}
        /*
		ScaleCapsuleForCrouching(crouch);
		PreventStandingInLowHeadroom();
        */
		// send input and other state parameters to the animator
		UpdateAnimator(move, jump);
        /*
        if (m_IsFalling && !m_IsJumping)
            Debug.Log(currentHeight + " " + m_IsJumping);
	    */
        m_passedTime = Time.time;
    }


	void ScaleCapsuleForCrouching(bool crouch)
	{
		if (m_IsGrounded && crouch)
		{
			if (m_Crouching) return;
			m_Capsule.height = m_Capsule.height / 2f;
			m_Capsule.center = m_Capsule.center / 2f;
			m_Crouching = true;
		}
		else
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
				return;
			}
			m_Capsule.height = m_CapsuleHeight;
			m_Capsule.center = m_CapsuleCenter;
			m_Crouching = false;
		}
	}

	void PreventStandingInLowHeadroom()
	{
		// prevent standing up in crouch-only zones
		if (!m_Crouching)
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
			}
		}
	}


	void UpdateAnimator(Vector3 move, bool jump)
	{
		// update the animator parameters
		m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
		m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
		m_Animator.SetBool("Crouch", m_Crouching);
		m_Animator.SetBool("OnGround", m_IsGrounded);
		if (!m_IsGrounded)
		{
            //Debug.Log(fallSpeed);

            //m_Animator.SetFloat("Jump", GetComponent<CharacterController>().velocity.y != 0 ? ((GetComponent<CharacterController>().velocity.y - 20) / 40 * 14) - 9: 0f);
            if(!anim_fall)
                m_Animator.SetFloat("Jump", (!m_IsJumping)? currentHeight/startFall : startJump);
            else
                m_Animator.SetFloat("Jump", currentHeight / startFall);
        }


		// calculate which leg is behind, so as to leave that leg trailing in the jump animation
		// (This code is reliant on the specific run cycle offset in our animations,
		// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
		float runCycle =
			Mathf.Repeat(
				m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
		float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
		if (m_IsGrounded)
		{
			m_Animator.SetFloat("JumpLeg", jumpLeg);
		}



		// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
		// which affects the movement speed because of the root motion.
		if (m_IsGrounded && move.magnitude > 0)
		{
			m_Animator.speed = m_AnimSpeedMultiplier;
		}
		else
		{
			// don't use that while airborne
			m_Animator.speed = 1;
		}
    }

    int c = 1;

    public void Footstep()
    {
        //GetComponent<AudioSource>().PlayOneShot(clips[(int)Mathf.Floor(Random.Range(0f, 3.99f))], 0.2f);
        if(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].name.Equals(this.gameObject.name))
            GetComponent<AudioSource>().PlayOneShot(clips[(c % 2) + 1], 0.15f);
        c++;
    }

    public void Jump()
    {
        if (!isSounding)
        {
            isSounding = true;
            if (GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].name.Equals(this.gameObject.name))
            {
                GetComponent<AudioSource>().PlayOneShot(clips[4], 0.2f);
                StartCoroutine(shutUP());
            }

        }
    }



    IEnumerator shutUP()
    {
        yield return new WaitForSeconds(0.1f);
        isSounding = false;
    }



	void HandleAirborneMovement()
	{
        if (startJump > 0.55)
        {
            m_IsJumping = false;
        }

        Vector3 v;



            v = transform.InverseTransformVector(new Vector3(0f, 0f, m_ForwardAmount * m_JumpForwardAmount));
            v.x = -v.x;



        if (!m_IsJumping)
        {
            Vector3 caduta =  new Vector3(0f, -Time.deltaTime * m_FallingSpeed, 0f);
            startJump = 0;

            GetComponent<CharacterController>().Move(caduta + v * Time.deltaTime);
        }
        else
        {
            oscillazione = (Mathf.Cos(startJump * 2 * Mathf.PI));
            //Debug.Log(startJump);
            Vector3 salto = new Vector3(0f, m_JumpHeight * oscillazione, 0f);
            GetComponent<CharacterController>().Move(salto + v * Time.deltaTime);
            m_IsJumping = true;
        }

        if(currentHeight < 0.5 && !isSounding)
        {
            isSounding = true;
            if (GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].name.Equals(this.gameObject.name))
            {
                GetComponent<AudioSource>().PlayOneShot(clips[5], 0.2f);
                StartCoroutine(shutUP());
            }

        }

        //m_Rigidbody.AddForce(new Vector3(0f,0f,(m_ForwardAmount * m_MoveSpeedMultiplier) / Time.deltaTime));
        /*
        Vector3 v = transform.InverseTransformVector(new Vector3(0f, -fallSpeed, (m_Animator.deltaPosition.magnitude) * m_MoveSpeedMultiplier));
        Debug.Log(m_Animator.deltaPosition + " " + v + " " + Vector3.ProjectOnPlane(v, m_GroundNormal));
        v.x = -v.x;

        GetComponent<CharacterController>().Move(m_Animator.deltaPosition * m_MoveSpeedMultiplier * 3 + new Vector3(0f, -fallSpeed, 0f));

        // apply extra gravity from multiplier:
        /*
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
		m_Rigidbody.AddForce(extraGravityForce);
        */
        m_GroundCheckDistance = oscillazione < 0.2 ? m_OrigGroundCheckDistance : 0.01f;

    }


	void HandleGroundedMovement(bool crouch, bool jump)
	{
		// check whether conditions are right to allow a jump:
		if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
		{

            m_IsGrounded = false;
            m_Animator.applyRootMotion = false;
            GetComponent<CharacterController>().Move(new Vector3(0f, m_OrigGroundCheckDistance,0f));
            m_GroundCheckDistance = 0.1f;
            m_IsJumping = true;

            /* jump!
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_FallingSpeed, m_Rigidbody.velocity.z);
			m_IsGrounded = false;
			m_Animator.applyRootMotion = false;
			m_GroundCheckDistance = 0.1f;
		    */
        }
	}

	void ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}


	public void OnAnimatorMove()
	{
		// we implement this function to override the default root motion.
		// this allows us to modify the positional speed before it's applied.
		if (m_IsGrounded && Time.deltaTime > 0)
		{
            //Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            if (GameManager.Instance.m_Current_State == (int)CoolCameraController.Stato.TreD && isLinked)
            {
                transform.position = new Vector3(GameManager.Instance.m_players[1 - GameManager.Instance.m_lockedPlayer].transform.position.x, transform.position.y, GameManager.Instance.m_players[1 - GameManager.Instance.m_lockedPlayer].transform.position.z);
            }
            else
            {
                Vector3 v = transform.InverseTransformVector(new Vector3(0f, 0f, (m_Animator.deltaPosition.magnitude) * m_MoveSpeedMultiplier));

                v.x = -v.x;
                GetComponent<CharacterController>().Move(v);
            }
    }
	}


	void CheckGroundStatus()
	{
		RaycastHit hitInfo;

        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hitInfo, Mathf.Infinity))
        {

            if(hitInfo.distance < m_GroundCheckDistance)
            {
                m_GroundNormal = hitInfo.normal;
                GetComponent<CharacterController>().Move(new Vector3(0f, -m_OrigGroundCheckDistance, 0f));
                m_IsGrounded = true;
                m_Animator.applyRootMotion = true;
                m_IsFalling = false;
                m_IsJumping = false;
                startJump = 0;
            }
            else
            {

                m_IsGrounded = false;

                m_GroundNormal = Vector3.up;
                m_Animator.applyRootMotion = false;
                if (!m_IsFalling)
                {
                    startFall = hitInfo.distance;
                }
                m_IsFalling = true;
                currentHeight = hitInfo.distance;
            }
            if (hitInfo.transform.gameObject.tag.Equals("Platform") && m_IsGrounded)
            {
                transform.parent.parent = hitInfo.transform.gameObject.transform;

            }
            else
            {
                transform.parent.parent = GameManager.Instance.whoAmI(gameObject.name) == 0 ? GameManager.Instance.m_level1.transform : GameManager.Instance.m_level2.transform;
            }
        }


#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.2f), transform.position + (Vector3.up * 0.2f) + (Vector3.down * hitInfo.distance));
#endif
        /*
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
            GetComponent<CharacterController>().Move(new Vector3(0f, -m_OrigGroundCheckDistance, 0f));
            m_IsGrounded = true;
			m_Animator.applyRootMotion = true;
		}
		else
		{
			m_IsGrounded = false;
			m_GroundNormal = Vector3.up;
			m_Animator.applyRootMotion = false;
		}
        */
    }




    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.point.y < transform.position.y + m_GroundCheckDistance)
        {
            m_IsGrounded = true;
            m_Animator.applyRootMotion = true;
            m_IsFalling = false;
            m_IsJumping = false;
            startJump = 0.6f;

        }

        if (hit.gameObject.tag.Equals("Spostabile")){

            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
                return;

            if (hit.moveDirection.y < -0.3F)
                return;

            Vector3 pushDir = new Vector3(hit.moveDirection.x * pushPower, -1f, hit.moveDirection.z * pushPower);
            body.velocity = pushDir;
            //body.AddForce(pushDir * pushPower);
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_IsGrounded && !collision.gameObject.tag.Equals("Ground") && !ThirdPersonCharacterNostro.locker)
            ThirdPersonCharacterNostro.locker = true;

    }

    private void OnCollisionStay(Collision collision)
    {

        if (m_IsGrounded && !collision.gameObject.tag.Equals("Ground") && ThirdPersonCharacterNostro.locker)
        {
            if (gameObject.tag.Equals("Player1"))
            {
                GameManager.Instance.m_lockedPlayer = 1;
            }
            else
                GameManager.Instance.m_lockedPlayer = 0;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_IsGrounded && !collision.gameObject.tag.Equals("Ground") && ThirdPersonCharacterNostro.locker)
            ThirdPersonCharacterNostro.locker = false;

    }


    public void setGroundDistance(float distance) { m_GroundCheckDistance = distance; }
}

