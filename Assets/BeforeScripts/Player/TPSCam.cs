using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCam : MonoBehaviour
{
    public Transform m_Target;
    public Transform m_Pivot;

    public float m_MoveSpeed = 999999f; // 타겟에 붙는 속도
    public float m_TurnSpeed = 1.5f; // 회전 속도(마우스 민감도)
    public float m_TurnSmoothing = 0.0f; // 회전 미끄러짐

    [HideInInspector] public float m_LookAngle; // 좌우 회전 // <- Player
    private Quaternion m_TransformTargetRot; // 좌우 각도 결과
    
    private float m_TiltAngle; // 상하 회전
    public float m_TiltMin = 45f; // 각도 상
    public float m_TiltMax = 75f; // 각도 하
    
    private Vector3 m_PivotEulers; // 상하 각도 계산
    private Quaternion m_PivotTargetRot; // 상하 각도 결과
    
    private bool m_VerticalAutoReturn = false; // set wether or not the vertical axis should auto return.

    void Awake()
    {
        m_TransformTargetRot = transform.localRotation;
        
        m_PivotEulers = m_Pivot.rotation.eulerAngles;
        m_PivotTargetRot = m_Pivot.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotationMovement();            
    }

    void LateUpdate()
    {
        FollowTarget();
    }

    private void HandleRotationMovement()
    {
        if (Time.timeScale < float.Epsilon)
			return;

        float _fX = Input.GetAxis("Mouse X");
        float _fY = Input.GetAxis("Mouse Y");

        // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
        m_LookAngle += _fX * m_TurnSpeed;

        // Rotate the rig (the root object) around Y axis only:
        m_TransformTargetRot = Quaternion.Euler(0f, m_LookAngle, 0f);
        
        if (m_VerticalAutoReturn)
        {
            m_TiltAngle = _fY > 0 ? Mathf.Lerp(0, -m_TiltMin, _fY) : Mathf.Lerp(0, m_TiltMax, -_fY);
        }
        else
        {
            // on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
            m_TiltAngle -= _fY * m_TurnSpeed;
            // and make sure the new value is within the tilt range
            m_TiltAngle = Mathf.Clamp(m_TiltAngle, -m_TiltMin, m_TiltMax);
        }
        
        // Tilt input around X is applied to the pivot (the child of this object)
        m_PivotTargetRot = Quaternion.Euler(m_TiltAngle, m_PivotEulers.y , m_PivotEulers.z);

        if (m_TurnSmoothing > 0)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_TransformTargetRot, m_TurnSmoothing * Time.deltaTime);
            m_Pivot.localRotation = Quaternion.Slerp(m_Pivot.localRotation, m_PivotTargetRot, m_TurnSmoothing * Time.deltaTime);            
        }
        else
        {
            transform.localRotation = m_TransformTargetRot;
            m_Pivot.localRotation = m_PivotTargetRot;
        }
    }

    private void FollowTarget()
    {        
        if (m_Target == null)
            return;
        // Move the rig towards target position.
        /*
        if (m_Target.GetComponent<Animator>().GetFloat("fnumForward") > 0f)
            transform.position = Vector3.Lerp(transform.position, m_Target.position, m_MoveSpeed * Time.deltaTime);
        */
        transform.position = m_Target.position;

        if (m_Target.GetComponent<PlayerNinja>().m_Forward > 0f && m_Target.GetComponent<PlayerNinja>().m_Right != 0f)
        {
            if (!m_Target.GetComponent<Animator>().GetBool("onFocus")) // && onSword
            {                
                m_LookAngle += Input.GetAxis("Horizontal") / 5f;
            }
        }

        // Cam
        if (m_Target.GetComponent<PlayerNinja>().m_Fast)
        {
            GetComponent<TPSCamWallClip>().m_Fast = true;
        }
        else
        {
            GetComponent<TPSCamWallClip>().m_Fast = false;
        }
        if (m_Target.GetComponent<Animator>().GetBool("onPistol") && m_Target.GetComponent<Animator>().GetBool("onFocus"))
        {
            GetComponent<TPSCamWallClip>().m_Focus = true;
            m_Target.transform.localRotation = Quaternion.Slerp(m_Target.transform.localRotation, transform.localRotation, 10f * Time.deltaTime);

            m_TiltMin = 35f; // 각도 상
            m_TiltMax = 35f; // 각도 하
        }
        else
        {
            GetComponent<TPSCamWallClip>().m_Focus = false;
            m_Target.transform.localRotation = transform.localRotation;

            m_TiltMin = 45f; // 각도 상
            m_TiltMax = 75f; // 각도 하
        }

    }

}