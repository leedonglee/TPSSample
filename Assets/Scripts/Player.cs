using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IPlayer : MonoBehaviour
{
    protected Animator _animator;
    protected bool _isSpeedy;

    protected float _moveForward;
    protected float _moveRight;

    // Anim
    public bool OnPistol { get { return _animator.GetBool("onPistol"); } }
    public bool OnAiming { get { return _animator.GetBool("onAiming"); } }

    // State
    public bool IsSpeedy { get { return _isSpeedy; } }

    // Move
    public float MoveForward { get { return _moveForward; } }
    public float MoveRight { get { return _moveRight; } }
}

public class Player : IPlayer
{
    [SerializeField]
    private GameObject[] _swords;
    [SerializeField]
    private GameObject _pistol;
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private Transform _muzzleTransform;
    [SerializeField]
    private Transform _aimTransform;

    private Camera _camera;
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;

    // Anim
    private const string ANIM_ON_GROUND = "onGround";
    private const string ANIM_ON_PISTOL = "onPistol";
    private const string ANIM_ON_AIMING = "onAiming";
    private const string ANIM_NUM_ATTACK = "numAttack";
    private const string ANIM_MOVE_RIGHT = "moveRight";
    private const string ANIM_MOVE_FORWARD = "moveForward";
    private const string ANIM_ON_JUMP = "onJump";
    private const string ANIM_ON_ROLL = "onRoll";

    private Transform _chest;
    private Vector3 _chestDir;
    private Vector3 _chestOffset;

    private bool _isAttack;
    private bool _isAttacking;
    private bool _isJump;
    private bool _isRolling = false;

    private int _swordCount;

    // Move
    private float _playerHorizontal = 0.0f;
    private float _playerVertical = 0.0f;

    // Acceleration
    private float _speedyCount;

    void Start()
    {
        _camera = Camera.main;

        _animator = GetComponent<Animator>();
        _animator.applyRootMotion = true;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        _capsuleCollider = GetComponent<CapsuleCollider>();

        _chest = _animator.GetBoneTransform(HumanBodyBones.Chest);
        _chestDir = new Vector3();
        _chestOffset = new Vector3(0, -45, -95);
    }

    void Update()
    {
        InputEvent();

        if (_isAttack)
        {
            Attack();
            _isAttack = false;
        }

        if (_isAttacking)
        {
            if (!_animator.GetBool(ANIM_ON_GROUND) || _animator.GetBool(ANIM_ON_PISTOL))
            {
                _animator.SetInteger(ANIM_NUM_ATTACK, 0);
                _isAttacking = false;
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || _animator.GetCurrentAnimatorStateInfo(0).IsName("JumpForward") || _animator.GetCurrentAnimatorStateInfo(0).IsName("JumpBack") || _animator.GetCurrentAnimatorStateInfo(0).IsName("JumpOver"))
            {
                _animator.SetInteger(ANIM_NUM_ATTACK, 0);
                _isAttacking = false;
            }            
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack1") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && _swordCount == 1)
            {
                _animator.SetInteger(ANIM_NUM_ATTACK, 0);
                _isAttacking = false;             
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack2") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && _swordCount == 2)
            {                
                _animator.SetInteger(ANIM_NUM_ATTACK, 0);
                _isAttacking = false;
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack3") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && _swordCount > 2)
            {
                _animator.SetInteger(ANIM_NUM_ATTACK, 0);
                _isAttacking = false;
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("SpeedAttack") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                _rigidbody.useGravity = true;
                _capsuleCollider.isTrigger = false;

                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * -2f, 10f * Time.deltaTime);
                _animator.SetInteger(ANIM_NUM_ATTACK, 0);
                _isAttacking = false;
            }
            /*
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack1") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
            {
                
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack2") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f)
            {                
                
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack3") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
            {
                
            }
            */
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("SpeedAttack") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.7f && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f)
            {
                _rigidbody.useGravity = false;
                _capsuleCollider.isTrigger = true;

                transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * 2f, 10f * Time.deltaTime);
            }
        }
    }

    void LateUpdate()
    {
        // Bone
        if (_animator.GetBool(ANIM_ON_PISTOL))
        {
            // 카메라 Update -> 플레이어 LateUpdate, 이 코드는 Update에서는 동작하지 않음
            _chestDir = _camera.transform.position + _camera.transform.forward * 50f;
            _chest.LookAt(_chestDir);
            _chest.rotation = _chest.rotation * Quaternion.Euler(_chestOffset);
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void InputEvent()
    {
        // GetAxisRaw는 -1 또는 0 또는 1을 반환한다.
        _playerHorizontal = Input.GetAxisRaw("Horizontal"); // A, D
        _playerVertical = Input.GetAxisRaw("Vertical"); // W, S

        if (Input.GetMouseButtonDown(0))
        {
            _isAttack = true;
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeWeapon();
        }

        // PistolAiming
        if (_animator.GetBool(ANIM_ON_PISTOL))
        {
            _animator.SetBool(ANIM_ON_AIMING, Input.GetMouseButton(1) && _animator.GetBool(ANIM_ON_GROUND));
        }

        if (!_isJump)
        {
            _isJump = Input.GetButtonDown("Jump");
        }
    }

    private void Move()
    {
        RaycastHit hit;

        // Vector.up * 0.1f로 부터 Vector.down으로 최대 1.1f의 거리만큼 Ray를 쏜다. 충돌하면 true.
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit, 1.1f))
        {
            _animator.applyRootMotion = true;
            _capsuleCollider.center = new Vector3(0, 0.9f, 0);

            if (_isRolling)
            {
                _animator.SetBool(ANIM_ON_ROLL, true);
                _isRolling = false;
            }
            else
            {
                _animator.SetBool(ANIM_ON_JUMP, false);
                _animator.SetBool(ANIM_ON_ROLL, false);
                _animator.SetBool(ANIM_ON_GROUND, true);

                _moveRight = _playerHorizontal;
                _moveForward = _playerVertical;

                if (_playerVertical == 1f && !_animator.GetCurrentAnimatorStateInfo(0).IsName("SpeedAttack"))
                {
                    _capsuleCollider.center = new Vector3(0, 0.9f, 0.3f);
                }

                if (_playerHorizontal == 0 && _playerVertical == 1f && _animator.GetCurrentAnimatorStateInfo(0).IsName("Sword"))
                {
                    _speedyCount += Time.deltaTime;
                }
                else
                {
                    _speedyCount = 0;
                    _isSpeedy = false;
                }

                if (_speedyCount > 1f)
                {
                    _moveForward = _moveForward + (_speedyCount - 1f);

                    if (_speedyCount >= 2f)
                    {
                        _speedyCount = 2f;
                        _isSpeedy = true;
                    }
                }

                _animator.SetFloat(ANIM_MOVE_RIGHT, _moveRight, 0.1f, Time.deltaTime);
                _animator.SetFloat(ANIM_MOVE_FORWARD, _moveForward, 0.1f, Time.deltaTime);

                // Jump 관련
                if (_isJump)
                {
                    _animator.SetBool(ANIM_ON_JUMP, true);

                    _speedyCount = 0;
                    _isSpeedy = false;
                }
                // 캐릭터 튕김 방지
                else if (_rigidbody.velocity.y > 2)
                {
                    _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 2, _rigidbody.velocity.z);
                }
                
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpForward") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f)
                {
                    _capsuleCollider.center = new Vector3(0, 1.8f, 0);
                }
                else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("JumpOver") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.01f)
                {
                    _capsuleCollider.center = new Vector3(0, 1.8f, 0);
                }
            }
        }
        // Falling (지면과 1.0f 차이)
        else
        {
            _animator.SetBool(ANIM_ON_GROUND, false);
            _animator.SetBool(ANIM_ON_AIMING, false);
            _animator.applyRootMotion = false;

            if (_rigidbody.velocity.y < -10f) // Airborne
                _isRolling = true;

            _speedyCount = 0;
            _isSpeedy = false;
        }

        _isJump = false;
    }

    private void ChangeWeapon()
    {
        if (!_animator.GetBool(ANIM_ON_PISTOL))
        {
            _animator.SetBool(ANIM_ON_PISTOL, true);
            _animator.SetLayerWeight(1, 1f); // Pistol Mask Weight 1

            _swords[0].SetActive(false);
            _swords[1].SetActive(false);
            _swords[2].SetActive(true);
            _swords[3].SetActive(true);
            _pistol.SetActive(true);
        }
        else
        {
            _animator.SetBool(ANIM_ON_PISTOL, false);
            _animator.SetBool(ANIM_ON_AIMING, false);
            _animator.SetLayerWeight(1, 0f); // Pistol Mask Weight 0

            _swords[0].SetActive(true);
            _swords[1].SetActive(true);
            _swords[2].SetActive(false);
            _swords[3].SetActive(false);
            _pistol.SetActive(false);
        }
    }

    private void Attack()
    {
        if (_animator.GetBool(ANIM_ON_PISTOL))
        {
            float right = Random.Range(-0.2f, 0.2f);
            float up = Random.Range(-0.2f, 0.2f);
            float forward = 10f;

            if (_animator.GetBool(ANIM_ON_AIMING))
            {
                right = 0.4f;
                up = Random.Range(-0.3f, 0.3f);
                forward = 20f;
            }

            _aimTransform.localPosition = new Vector3(right, up, forward);
            _muzzleTransform.rotation = Quaternion.LookRotation(_aimTransform.position - _muzzleTransform.position).normalized;

            Instantiate(_bullet, _muzzleTransform.position, _muzzleTransform.rotation);
        }
        else
        {
            if (!_isAttacking && _animator.GetCurrentAnimatorStateInfo(0).IsName("Sword"))
            {
                _isAttacking = true;
                _swordCount = _isSpeedy ? -1 : 0;
                _animator.SetInteger(ANIM_NUM_ATTACK, _swordCount);
            }
            else if (_isAttacking && _swordCount > 0 && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.6f)
            {
                _swordCount++;
                _animator.SetInteger(ANIM_NUM_ATTACK, _swordCount);
            }
        }
    }
}
