using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class EnemyTest : MonoBehaviour
{
    // COMMON
    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private CapsuleCollider m_CapsuleCollider;

    NavMeshAgent nav;

    UnitStatus m_Status;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        nav = GetComponent<NavMeshAgent>();
        /*
        nav.SetDestination(StageManager.s_SM.m_Player.transform.position);
        m_Animator.SetFloat("fnumForward", 1f); // 성공
        */

        m_Status = GetComponent<UnitStatus>();
    }

    void Update()
    {
        if (m_Status.m_HP <= 0)
        {
            Instantiate(Resources.Load("Effects/RobotDestroy"), transform.position + transform.up * 1.5f, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
