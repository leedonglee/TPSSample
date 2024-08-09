using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerNinja : MonoBehaviour
{
    // COMMON
    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private CapsuleCollider m_CapsuleCollider;

    // ANIM, MOVE
    private Transform m_Chest;
    private Vector3 m_ChestDir;
    private Vector3 m_ChestOffset;
    
    [HideInInspector] public float m_Forward, m_Right;

    [HideInInspector] public bool m_Fast;
    private float m_FastCount;

    private bool m_Roll = false;

    // ATTACK
    [HideInInspector] public bool m_Fire;

    [SerializeField] private GameObject[] m_Swords;
    private bool m_Attacking = false;
    private int m_SwordCount;

    [SerializeField] private GameObject m_Pistol;
    [SerializeField] private GameObject m_Bullet;
    [SerializeField] private Transform m_Muzzle;
    [SerializeField] private Transform m_Aim;

    [SerializeField] private UnitAttackRange m_AttackRange;
    private int m_AttackNumber = 9000;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Animator.applyRootMotion = true;

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_CapsuleCollider.center = new Vector3(0, 0.9f, 0);
        m_CapsuleCollider.radius = 0.3f;
        m_CapsuleCollider.height = 1.8f;

        m_Chest = m_Animator.GetBoneTransform(HumanBodyBones.Chest);
        m_ChestDir = new Vector3();
        m_ChestOffset = new Vector3(0, -45, -95);

        m_Fast = false;
        m_FastCount = 0;

        m_SwordCount = 0;
    }

    private void LateUpdate()
    {
        // Bone
        if (m_Animator.GetBool("onPistol"))
        {
            m_ChestDir = Camera.main.transform.position + Camera.main.transform.forward * 50f;
            m_Chest.LookAt(m_ChestDir);
            m_Chest.rotation = m_Chest.rotation * Quaternion.Euler(m_ChestOffset);
        }

        // Fire
        if (m_Fire)        
            Fire(); // Bone과 싱크 맞추기 위해 LateUpdate에 위치.
        
        if (m_Attacking)
        {
            if (!m_Animator.GetBool("onGround") || m_Animator.GetBool("onPistol"))
            {
                m_Animator.SetInteger("inumAttack", 0);
                m_Attacking = false;
            }
            else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("JumpForward") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("JumpBack") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("JumpOver"))
            {
                m_Animator.SetInteger("inumAttack", 0);
                m_Attacking = false;
            }            
            else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack1") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && m_SwordCount == 1)
            {
                m_Animator.SetInteger("inumAttack", 0);
                m_Attacking = false;             
            }
            else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack2") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && m_SwordCount == 2)
            {                
                m_Animator.SetInteger("inumAttack", 0);
                m_Attacking = false;
            }
            else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack3") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && m_SwordCount > 2)
            {
                m_Animator.SetInteger("inumAttack", 0);
                m_Attacking = false;
            }
            else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("SpeedAttack") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                m_Rigidbody.useGravity = true;
                m_CapsuleCollider.isTrigger = false;

                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * -2f, 10f * Time.deltaTime);
                m_Animator.SetInteger("inumAttack", 0);
                m_Attacking = false;
            }

            // Attacking
            else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack1") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
            {
                m_AttackRange.m_Power = 30;
                m_AttackRange.m_AttackNumber = m_AttackNumber;
            }
            else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack2") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f)
            {                
                m_AttackRange.m_Power = 30;
                m_AttackRange.m_AttackNumber = m_AttackNumber + 1;
            }
            else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack3") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
            {
                m_AttackRange.m_Power = 40;
                m_AttackRange.m_AttackNumber = m_AttackNumber + 2;
            }
            else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("SpeedAttack") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f)
            {
                m_Rigidbody.useGravity = false;
                m_CapsuleCollider.isTrigger = true;

                m_AttackRange.m_Power = 100;
                m_AttackRange.m_AttackNumber = m_AttackNumber;

                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * 2f, 10f * Time.deltaTime);
            }            
        }
        else
        {
            m_AttackRange.m_Power = 0;
            m_AttackRange.m_AttackNumber = 0;
            m_AttackRange.m_AttackSplash = 99;
        }
    }

    public void Move(float x_H, float x_V, bool x_Jump)
    {
        RaycastHit hit;
        
        // Vector.up * 0.1f로 부터 Vector.down으로 최대 1.1f의 거리만큼 Ray를 쏜다. 충돌하면 true.
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, 1.1f))
        {
            m_Animator.applyRootMotion = true;
            m_CapsuleCollider.center = new Vector3(0, 0.9f, 0);

            if (m_Roll)
            {
                m_Animator.SetBool("onRoll", true);
                m_Roll = false;
            }
            else
            {
                m_Animator.SetBool("onJump", false);
                m_Animator.SetBool("onRoll", false);
                m_Animator.SetBool("onGround", true);

                m_Right = x_H;
                m_Forward = x_V;

                if (x_V == 1f && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("SpeedAttack")) 
                    m_CapsuleCollider.center = new Vector3(0, 0.9f, 0.3f);

                if (x_H == 0 && x_V == 1f && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Sword"))
                {                    
                    m_FastCount += Time.deltaTime;
                }
                else
                {
                    m_FastCount = 0;
                    m_Fast = false;
                }

                if (m_FastCount > 1f)
                {
                    m_Forward = m_Forward + (m_FastCount - 1f);
                    if (m_FastCount >= 2f)
                    {
                        m_FastCount = 2f;
                        m_Fast = true;
                    }
                }

                m_Animator.SetFloat("fnumRight", m_Right, 0.1f, Time.deltaTime);
                m_Animator.SetFloat("fnumForward", m_Forward, 0.1f, Time.deltaTime);
                
                // Jump 관련
                if (x_Jump)
                {
                    m_Animator.SetBool("onJump", true);

                    m_FastCount = 0;
                    m_Fast = false;
                }
                else // 캐릭터 튕김 방지
                {
                    if (m_Rigidbody.velocity.y > 2)
                    {
                        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, 2, m_Rigidbody.velocity.z);
                    }
                }
                
                if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("JumpForward") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f)
                {
                    m_CapsuleCollider.center = new Vector3(0, 1.8f, 0);
                }
                else if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("JumpOver") && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f)
                {
                    m_CapsuleCollider.center = new Vector3(0, 1.8f, 0);
                }

            }
        }
        else // Falling (지면과 1.0f 차이)
        {
            m_Animator.SetBool("onGround", false);
            m_Animator.SetBool("onFocus", false);
            m_Animator.applyRootMotion = false;
            
            if (m_Rigidbody.velocity.y < -10f) // Airborne
                m_Roll = true;
            
            m_FastCount = 0;
            m_Fast = false;
        }

    }

    public void ChangeWeapon()
    {
        if (!m_Animator.GetBool("onPistol"))
        {
            m_Animator.SetBool("onPistol", true);
            m_Animator.SetLayerWeight(1, 1f); // Pistol Mask Weight 1

            m_Swords[0].SetActive(false);
            m_Swords[1].SetActive(false);
            m_Swords[2].SetActive(true);
            m_Swords[3].SetActive(true);
            m_Pistol.SetActive(true);
        }
        else
        {
            m_Animator.SetBool("onPistol", false);
            m_Animator.SetBool("onFocus", false);

            m_Animator.SetLayerWeight(1, 0f); // Pistol Mask Weight 0

            m_Swords[0].SetActive(true);
            m_Swords[1].SetActive(true);
            m_Swords[2].SetActive(false);
            m_Swords[3].SetActive(false);
            m_Pistol.SetActive(false);
        }
    }
    
    public void PistolFocus(bool x_MouseRight)
    {
        if (m_Animator.GetBool("onGround"))
        {
            if (x_MouseRight && m_Animator.GetBool("onPistol"))
                m_Animator.SetBool("onFocus", true);
            else
                m_Animator.SetBool("onFocus", false);
        }
        else
            m_Animator.SetBool("onFocus", false);
    }

    public void Fire()
    {
        if (m_Animator.GetBool("onPistol"))
        {
            float _right = Random.Range(-0.2f, 0.2f);
            float _up = Random.Range(-0.2f, 0.2f);
            float _forward = 10f;

            if (m_Animator.GetBool("onFocus"))
            {
                _right = 0.4f;
                _up = Random.Range(-0.3f, 0.3f);
                _forward = 20f;
            }

            m_Aim.localPosition = new Vector3(_right, _up, _forward);
            m_Muzzle.rotation = Quaternion.LookRotation(m_Aim.position - m_Muzzle.position).normalized;
            Instantiate(m_Bullet, m_Muzzle.position, m_Muzzle.rotation);
        }
        else
        {
            if (!m_Attacking && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Sword"))
            {
                m_Attacking = true;
                m_AttackNumber = GetAttackNumber();

                if (m_Fast)
                    m_SwordCount = -1;
                else
                    m_SwordCount = 1;

                m_Animator.SetInteger("inumAttack", m_SwordCount);
            }
            else if (m_Attacking && m_SwordCount > 0 && m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f)
            {
                m_SwordCount++;
                m_Animator.SetInteger("inumAttack", m_SwordCount);
            }
        }

    }

    void OnTriggerEnter(Collider x_Obj)
    {        
        if (x_Obj.gameObject.layer != 8 && x_Obj.gameObject.layer != 9 && x_Obj.gameObject.layer != 10 && x_Obj.gameObject.layer != 30 && x_Obj.gameObject.layer != 31 && !x_Obj.gameObject.name.Equals("Plane"))
        {
            m_Rigidbody.useGravity = true;
            m_CapsuleCollider.isTrigger = false;

            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * -2f, 10f * Time.deltaTime);
            m_Animator.SetInteger("inumAttack", 0);
            m_Attacking = false;
        }        
    }
    
    int GetAttackNumber()
    {
        return 9000 + Random.Range(0, 1000);
    }

}