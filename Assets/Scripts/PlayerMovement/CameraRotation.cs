using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [Header("References")]
    public Transform m_Transform;
    public Camera m_Camera;
    public Transform m_CameraTransform;

    [Header("Config")]
    public float m_Sensitivity;

    public float m_DistanceFromPlayer = 4;
    public float m_VerticalOffset     = 0.5f;

    public float m_MinRotation = -70f;
    public float m_MaxRotation = 70f;

    private Vector2 m_Rotation;

    private static CameraRotation s_Instance;

    private void Start()
    {
        s_Instance = this;
    }

    private void Update()
    {
        InputUpdate();
        UpdateCameraPosition();
    }

    public static Vector2 GetRotation()
    { return s_Instance.m_Rotation; }


    //////////////////////////
    ///      BACKEND
    //////////////////////////

    private void InputUpdate()
    {
        var mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * m_Sensitivity;
        var mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * m_Sensitivity;

        m_Rotation.y += mouseX;
        m_Rotation.x -= mouseY;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x, m_MinRotation, m_MaxRotation);
    }

    private void UpdateCameraPosition()
    {
        m_CameraTransform.rotation = Quaternion.Euler(m_Rotation.x, m_Rotation.y, 0);

        m_CameraTransform.localPosition = new Vector3();
        m_CameraTransform.localPosition -= m_CameraTransform.forward * m_DistanceFromPlayer;
    }
}
