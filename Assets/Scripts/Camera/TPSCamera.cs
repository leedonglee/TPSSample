using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITPSCameraWallClip : MonoBehaviour
{
    protected bool _isSpeedy;
    protected bool _isAiming;

    public bool IsSpeedy { set { _isSpeedy = value; } }
    public bool IsAiming { set { _isAiming = value; } }
}

public class TPSCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _pivotTransform;
    [SerializeField]
    private IPlayer _player;

    private const float TURN_SPEED = 1.5F;
    private const float TURN_SMOOTHING = 0.0F;
    private const bool VERTICAL_AUTO_RETURN = false;

    private ITPSCameraWallClip _tpsCameraWallClip;

    // 좌우 회전
    private Quaternion _transformTargetRotation; // 좌우 각도 결과
    private float _lookAngle; // 좌우 회전

    // 상하 회전
    private Quaternion _pivotTransformTargetRotation; // 상하 각도 결과
    private Vector3 _pivotTransformEulers; // 상하 각도 계산
    private float _tiltAngle; // 상하 회전
    private float _tiltMin = 45f;
    private float _tiltMax = 75f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _tpsCameraWallClip = GetComponent<ITPSCameraWallClip>();

        _transformTargetRotation = transform.localRotation;
        _pivotTransformEulers = _pivotTransform.rotation.eulerAngles;
        _pivotTransformTargetRotation = _pivotTransform.transform.localRotation;
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

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
        _lookAngle += mouseX * TURN_SPEED;

        // Rotate the rig (the root object) around Y axis only:
        _transformTargetRotation = Quaternion.Euler(0f, _lookAngle, 0f);

        if (VERTICAL_AUTO_RETURN)
        {
            // _tiltAngle = mouseY > 0 ? Mathf.Lerp(0, -_tiltMin, mouseY) : Mathf.Lerp(0, _tiltMax, -mouseY);
        }
        else
        {
            // on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
            _tiltAngle -= mouseY * TURN_SPEED;
            // and make sure the new value is within the tilt range
            _tiltAngle = Mathf.Clamp(_tiltAngle, -_tiltMin, _tiltMax);
        }

        // Tilt input around X is applied to the pivot (the child of this object)
        _pivotTransformTargetRotation = Quaternion.Euler(_tiltAngle, _pivotTransformEulers.y , _pivotTransformEulers.z);

        if (TURN_SMOOTHING > 0f)
        {
            // transform.localRotation = Quaternion.Slerp(transform.localRotation, _transformTargetRotation, TURN_SMOOTHING * Time.deltaTime);
            // _pivotTransform.localRotation = Quaternion.Slerp(_pivotTransform.localRotation, _pivotTransformTargetRotation, TURN_SMOOTHING * Time.deltaTime);            
        }
        else
        {
            transform.localRotation = _transformTargetRotation;
            _pivotTransform.localRotation = _pivotTransformTargetRotation;
        }
    }

    private void FollowTarget()
    {        
        if (_player == null)
            return;

        transform.position = _player.transform.position;

        if (_player.MoveForward > 0f && _player.MoveRight != 0f)
        {
            if (!_player.OnAiming)
            {
                _lookAngle += Input.GetAxis("Horizontal") / 5f;
            }
        }

        _tpsCameraWallClip.IsSpeedy = _player.IsSpeedy;

        if (_player.OnPistol && _player.OnAiming)
        {
            _tpsCameraWallClip.IsAiming = true;
            _player.transform.localRotation = Quaternion.Slerp(_player.transform.localRotation, transform.localRotation, 10f * Time.deltaTime);

            _tiltMin = 35f;
            _tiltMax = 35f;
        }
        else
        {
            _tpsCameraWallClip.IsAiming = false;
            _player.transform.localRotation = transform.localRotation;

            _tiltMin = 45f;
            _tiltMax = 75f;
        }

    }
}
