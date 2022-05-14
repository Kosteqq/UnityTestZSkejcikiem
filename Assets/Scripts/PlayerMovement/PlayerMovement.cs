using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public Transform m_Transform;
    public Camera    m_Camera;
    public Transform m_CameraTransform;
    public Rigidbody m_PlayerRigidbody;
    public Transform m_PlayerTransform;

    [Header("Masks")]
    public LayerMask m_GroundMask;

    [Header("Keys")]
    public KeyCode m_RunningKey = KeyCode.LeftShift;
    public KeyCode m_SlidingKey = KeyCode.LeftControl;
    public KeyCode m_JumpKey    = KeyCode.Space;

    [Header("Speed")]
    public float m_WalkingSpeed;
    public float m_RunningSpeed;
    public float m_SlidingSpeed;
    public float m_AirMultiplier;
    public float m_DragValue;

    [Header("Jumping")]
    public float m_JumpForce;
    public float m_JumpCooldown;
    public float m_PlayerHeight;
    private bool m_CanJump = true;

    [Header("Sliding")]
    public float m_SlidingScale;
    public float m_MaxSlidingTime;

    private float m_PlayerDefaultScale;

    private Vector3 m_RawMouseInput = new Vector3();
    private float   m_CurrentSpeed;
    private bool    m_OnGround;


    private MoveState m_State;
    private enum MoveState
    {
        None, Walking, Running, Sliding, Jumping
    }

    private static PlayerMovement s_Instance;


    private void Start()
    {
        s_Instance = this;

        m_PlayerDefaultScale = m_PlayerTransform.localScale.y;
        m_PlayerRigidbody.freezeRotation = true;
        Cursor.visible = false;
    }

    private void Update()
    {
        InputUpdate();
        OnGroundUpdate();
        StateUpdate();
        UpdateCurrentSpeed();
        MovePlayer();
        Debug.Log(m_CurrentSpeed);
    }


    //////////////////////////
    ///   STATIC FUNCTIONS
    //////////////////////////
    public static void SetPos(Vector3 pos)
    { s_Instance.m_Transform.position = pos; }

    public static Vector3 GetPos()
    { return s_Instance.m_Transform.position; }

    public static void Push(Vector3 dir, float power)
    { s_Instance.m_PlayerRigidbody.AddForce(dir * power, ForceMode.Impulse); }


    //////////////////////////
    ///      BACKEND
    //////////////////////////

    private void InputUpdate()
    {
        m_RawMouseInput.x = Input.GetAxisRaw("Horizontal");
        m_RawMouseInput.y = Input.GetAxisRaw("Vertical");
    }

    private void OnGroundUpdate()
    {
        m_OnGround = Physics.Raycast(m_PlayerTransform.position, Vector3.down, m_PlayerHeight * 0.6f, m_GroundMask);
    }
    
    private void StateUpdate()
    {
        if (m_State == MoveState.None)
            m_State = MoveState.Walking;

        if (Input.GetKey(m_RunningKey) && m_State == MoveState.Walking)
            m_State = MoveState.Running;
        else if (Input.GetKeyUp(m_RunningKey))
            m_State = MoveState.Walking;

        if (Input.GetKeyDown(m_SlidingKey))
            StartSliding();
        else if (Input.GetKeyUp(m_SlidingKey))
            StopSliding();

        if (Input.GetKeyDown(m_JumpKey))
            Jump();
    }

    private void UpdateCurrentSpeed()
    {
        switch (m_State)
        {
            case MoveState.Walking: { m_CurrentSpeed = m_WalkingSpeed; break; }
            case MoveState.Running: { m_CurrentSpeed = m_RunningSpeed; break; }
            case MoveState.Sliding: { m_CurrentSpeed = m_SlidingSpeed; break; }
            default:                { m_CurrentSpeed = m_WalkingSpeed; break; }
        }
    }

    private void MovePlayer()
    {
        var direction = m_Transform.forward * m_RawMouseInput.y + m_Transform.right * m_RawMouseInput.x;
        float multipler = !m_OnGround ? m_AirMultiplier : 1;     // IF PLAYER IS IN THE AIR
        m_PlayerRigidbody.AddForce(direction * m_CurrentSpeed * multipler, ForceMode.Force);


        // SPEED CONTROL //
        // IF VELOCITY IS HIGHER THAN CURRENT SPEED
        var output = new Vector3(m_PlayerRigidbody.velocity.x, 0f, m_PlayerRigidbody.velocity.z);
        if (output.magnitude > m_CurrentSpeed)
        {
            output = output.normalized * m_CurrentSpeed;
            m_PlayerRigidbody.velocity = new Vector3(output.x, m_PlayerRigidbody.velocity.y, output.z);
        }
    }


    private Coroutine m_SlidingCroutine = null;
    private void StartSliding()
    {
        m_SlidingCroutine = StartCoroutine(SlidingLimiter());
        m_State = MoveState.Sliding;

        m_PlayerTransform.localScale = new Vector3(m_PlayerTransform.localScale.x, m_SlidingScale, m_PlayerTransform.localScale.z);
    }

    private void StopSliding()
    {
        if (m_State != MoveState.Sliding) return;
        if (m_SlidingCroutine != null) StopCoroutine(m_SlidingCroutine);
        m_State = MoveState.None;

        m_PlayerTransform.localScale = new Vector3(m_PlayerTransform.localScale.x, m_PlayerDefaultScale, m_PlayerTransform.localScale.z);
    }

    private IEnumerator SlidingLimiter()
    {
        yield return new WaitForSeconds(m_MaxSlidingTime);
        StopSliding();
        yield return null;
    }



    private void Jump()
    {
        if (!m_CanJump || !m_OnGround) return;

        m_PlayerRigidbody.velocity = new Vector3(m_PlayerRigidbody.velocity.x, 0f, m_PlayerRigidbody.velocity.z);
        m_PlayerRigidbody.AddForce(m_Transform.up * m_JumpForce, ForceMode.Impulse);
        StartCoroutine(JumpCooldown());
    }
    private IEnumerator JumpCooldown()
    {
        m_CanJump = false;
        m_State = MoveState.Jumping;
        yield return new WaitForSeconds(m_JumpCooldown);
        m_CanJump = true;
        m_State = MoveState.Walking;
        yield return null;
    }

}
