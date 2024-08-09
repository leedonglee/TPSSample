using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    private PlayerNinja m_Ninja;

    private bool m_Jump;
    private bool m_Fire;

    private float m_H = 0.0f;
    private float m_V = 0.0f;
    /*
    private float m_TabSpeed = 0.5f;
    private float m_LastTabTime = 0;

    private bool m_DashW = false;
    */

    private void Start()
    {
        m_Ninja = StageManager.s_SM.m_Player.GetComponent<PlayerNinja>();
    }

    private void Update()
    {
        // Attack
        if (Input.GetMouseButtonDown(0))
            m_Fire = true;

        // Weapon Change
        else if (Input.GetKeyDown(KeyCode.Tab))
            m_Ninja.ChangeWeapon();

        // Jump
        else if (!m_Jump)
            m_Jump = Input.GetButtonDown("Jump"); // Space

        m_Ninja.m_Fire = m_Fire;
        m_Fire = false;
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - m_LastTabTime < m_TabSpeed)
            {
                m_DashW = true;
            }
            m_LastTabTime = Time.time;
        }
        else if (Input.GetKeyUp(KeyCode.W) && m_DashW)
        {
            m_DashW = false;
        }
        */
    }

    private void FixedUpdate()
    {
        // GetAxisRaw는 -1 또는 0 또는 1을 반환한다.
        m_H = Input.GetAxisRaw("Horizontal"); // A, D
        m_V = Input.GetAxisRaw("Vertical"); // W, S

        // Focus
        bool _MouseRight = Input.GetMouseButton(1);
        m_Ninja.PistolFocus(_MouseRight);

        // Move
        m_Ninja.Move(m_H, m_V, m_Jump);
        m_Jump = false;
        
    }

}