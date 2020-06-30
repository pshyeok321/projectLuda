using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;
using CameraProduction;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerInfo))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerBehaviour : MonoBehaviour
    {
        public bool isAutoChange;

        #region Components
        private Animator _Animator;
        public Animator pAnimator {
            get => _Animator; 
            set => _Animator = value; 
        }

        PlayerInfo _PlayerInfo;
        public PlayerInfo PlayerInfo {
            get => _PlayerInfo; 
        }

        Rigidbody _Rigidbody;
        public Rigidbody Rigidbody
        {
            get => _Rigidbody;
            set => _Rigidbody = value;
        }

        CameraMovement _CameraMovement;
        HUDManager _HUDManager;
        PlayerSound _PlayerSound;
        #endregion

        int layerMask;

        private float _MoveSpeed;
        private float _ForwardAmount;

        private float _TurnSpeed;
        private float _TurnAmount;

        private float _JumpPower;
        private float _DashPower;
        private float _AroundSpeed;

        public float _JumpMaxHeightTime;
        public float _JumpTime;

        private float _DecentMaxTime = 0;
        private float _DecentTime;
        private float _JumpMoveSpeedDecentRatio;
        private float _AimMoveSpeedDecentRatio;

        Vector3 addMoveSpeed = Vector3.zero;

        [SerializeField] private float _Gravity = 9.8f;
        [SerializeField] private float _AimMultiplier;
        [SerializeField] private float _MoveMultiplier;
        [SerializeField] private float _GravityMultiplier;

        public GameObject _JumpEffect;
        public GameObject _DoubleJumpEffect;
        public Transform _DoubleJumpEffectLocate;
        public GameObject _DoubleJumpEffectPoom;
        public GameObject _DashEffect;
        public GameObject _DashPos;
        public GameObject _LandingEffect;
        public GameObject _DamageEffect;
        public GameObject _FootstepEffect;

        public GameObject _ControlObject;

        private DroneInfo _ControlDroneInfo;
        public GameObject _DroneEMP;

        public GameObject _Siren;

        public Transform _PlayerPelvis;

        private const float _InverseValue = 1.0f;
        bool useGravity = false;

        private void Awake()
        {
            layerMask = (1 << 2);
            layerMask = ~layerMask;

            _CameraMovement = Camera.main.GetComponentInParent<CameraMovement>();
            _HUDManager = HUDManager.GetHUDManager();

            _PlayerSound = GetComponent<PlayerSound>();
            _PlayerInfo = GetComponent<PlayerInfo>();
            _Animator = GetComponent<Animator>();

            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.constraints = 
                RigidbodyConstraints.FreezeRotationX | 
                RigidbodyConstraints.FreezeRotationY |
                RigidbodyConstraints.FreezeRotationZ;

            //_Rigidbody.useGravity = false;
            //_Rigidbody.isKinematic = true;

            _JumpMoveSpeedDecentRatio = _PlayerInfo.JumpMoveSpeedDecentRatio;
            _AimMoveSpeedDecentRatio = _PlayerInfo.AimMoveSpeedDecentRatio;

            _TurnSpeed = _PlayerInfo.TurnSpeed;
            _JumpPower = _PlayerInfo.JumpPower;
            _DashPower = _PlayerInfo.DashPower;
            _AroundSpeed = _PlayerInfo.AroundSpeed;
            _MoveSpeed = _PlayerInfo.MoveSpeed;

            _ControlObject = transform.gameObject;
        }

        private void Update()
        {

            if (isAutoChange)
                AutoChange();
        }

        void AutoChange()
        {
            _MoveSpeed = _PlayerInfo.MoveSpeed;
            _TurnSpeed = _PlayerInfo.TurnSpeed;
            _JumpPower = _PlayerInfo.JumpPower;
            _DashPower = _PlayerInfo.DashPower;
            _AroundSpeed = _PlayerInfo.AroundSpeed;

            _JumpMoveSpeedDecentRatio = _PlayerInfo.JumpMoveSpeedDecentRatio;
            _AimMoveSpeedDecentRatio = _PlayerInfo.AimMoveSpeedDecentRatio;
        }

        public void UseAdditionalGravity(bool isGround)
        {
            if (isGround)
            {
                useGravity = false;
            }

            if (_Rigidbody.velocity.y > 0.4f)
            {
                useGravity = true;
            }

            if (useGravity)
            {
                Vector3 _GravityVector = new Vector3( 0,
                    _Gravity * _GravityMultiplier * Time.deltaTime, 0);
                _ControlObject.transform.position -= _GravityVector;
            }
        }

        void RotateToLookCameraForward()
        {
            Vector3 lookForward = Camera.main.transform.forward;
            lookForward.y = 0;

            _ControlObject.transform.rotation = Quaternion.LookRotation(lookForward);
        }

        void ApplyToRotateNormal()
        {
            float turnSpeed = Mathf.Lerp(_TurnSpeed, _TurnSpeed, _ForwardAmount);
            _ControlObject.transform.Rotate(0, _TurnAmount * turnSpeed * Time.deltaTime, 0);
        }

        private void OnDrawGizmos()
        {   
        }

        public void PlayerMove(Vector3 direction, RaycastHit hit, bool isJump, bool ischange, bool isAim, ForwardSituation forwardSituation)
        {
            if (hit.transform != null) _DecentTime = 0;
            _DecentTime += Time.deltaTime;
            Debug.DrawLine(transform.position, transform.position + transform.forward * 0.1f, Color.cyan, 2f);

            if (isJump) _MoveSpeed *= (_InverseValue - (_DecentTime / _DecentMaxTime < 1 ? _DecentTime / _DecentMaxTime : 1) * _JumpMoveSpeedDecentRatio);
            else if (isAim) _MoveSpeed *= (_InverseValue - _AimMoveSpeedDecentRatio);
            else _MoveSpeed = PlayerInfo.MoveSpeed;

            if (direction.magnitude > 1f) direction.Normalize();

            direction = _ControlObject.transform.InverseTransformDirection(direction);
            direction = Vector3.ProjectOnPlane(direction, hit.point);
            _ForwardAmount = direction.z;
            _TurnAmount = Mathf.Atan2(direction.x, direction.z);

            Vector3 finalVelocity = Vector3.zero;

            if (!ischange)
            {
                if (isAim)
                {
                    RotateToLookCameraForward();
                }
                else
                {
                    ApplyToRotateNormal();
                }
            }

            if (ischange)
            {
                RotateToLookCameraForward();
            }

            switch (forwardSituation)
            {
                case ForwardSituation.Normal:
                    finalVelocity = direction * _MoveSpeed;

                    break;
                case ForwardSituation.Block:
                    _ForwardAmount = 0;

                    break;

                case ForwardSituation.JumpCorrecting:
                    finalVelocity.y = 0.3f;
                    break;
            }
            _ControlObject.transform.Translate(finalVelocity * Time.deltaTime);

        }

        public void PlayerJump(bool isJump, int jumpCount)
        {
            if (!isJump && jumpCount < 1)
            {

                Vector3 jumpPower = Vector3.up * _JumpPower;
                _Rigidbody.AddForce(jumpPower, ForceMode.Force);

                Instantiate(_JumpEffect, _ControlObject.transform.position, Quaternion.identity);
            }
            else if (isJump && jumpCount == 1)
            {
                _Rigidbody.velocity = Vector3.zero;
                Vector3 jumpPower = Vector3.up * _JumpPower;
                _Rigidbody.AddForce(jumpPower, ForceMode.Force);

                Instantiate(_DoubleJumpEffectPoom, _ControlObject.transform.position, Quaternion.identity);
                Instantiate(_DoubleJumpEffect, _ControlObject.transform);

                _Animator.Play("Double Jump");
            }
        }

        public void PlayerAround(Vector3 position, float aroundSpeed)
        {
            if(aroundSpeed > 0)
                _ControlObject.transform.RotateAround(position, Vector3.up, aroundSpeed * Time.deltaTime);
            else if(aroundSpeed < 0)
                _ControlObject.transform.RotateAround(position, Vector3.down, -aroundSpeed * Time.deltaTime);
        }

        public void PlayerRideOnPlatform(Vector3 speed)
        {
            _ControlObject.transform.position += speed;
        }

        public void PlayerDash()
        {
            if (_Rigidbody.velocity.y < 0)
            {
                _Rigidbody.velocity = Vector3.zero;
            }

            Vector3 oldPos = this.transform.position;
            Vector3 direction = this.transform.forward;


            Vector3 dashPower = direction * _DashPower;
            dashPower.y = oldPos.y;

            _Rigidbody.AddForce(dashPower);
            GameObject dashEffect = Instantiate(_DashEffect, _DashPos.transform.position, Quaternion.LookRotation(_ControlObject.transform.forward));
            dashEffect.transform.parent = this.transform;

            //Debug.Log(string.Format("This Transform: {0}, Direction: {1}", 
            //    this.transform.position, dashPower));

            _Animator.Play("Dash");
        }
        
        public void PlayerHacking(ObjectInfo objectInfo, ref bool soundCutOff, bool _SoundPlay)
        {
            try
            {
                //Debug.Log(string.Format("{0} : Try InterAction", objectInfo.transform.name));
            }
            catch
            {
                //Debug.Log(string.Format("There's no InterAction Object"));
            }

            if (objectInfo != null && objectInfo._HackingMode)
            {
                ObjectType objectType = objectInfo.ObjectType;

                if (objectType == ObjectType.Platforms)
                {
                    //Debug.Log(string.Format("Try interactive TYPE::Platforms : {0}", objectInfo.transform.name));
                    PlayerHackingPlatform(objectInfo);

                    if (!soundCutOff)
                    {
                        _PlayerSound.PlayerHackingSFX(_SoundPlay);
                        soundCutOff = true;
                    }
                }

                if (objectType == ObjectType.Drones)
                {
                    Debug.Log(string.Format("Try interactive TYPE::Drones : {0}", objectInfo.transform.name));
                    PlayerHackingDrone(objectInfo);

                    if (!soundCutOff)
                    {
                        _PlayerSound.PlayerHackingSFX(_SoundPlay);
                        soundCutOff = true;
                    }
                }

                if (objectType == ObjectType.CCTV)
                {
                    Debug.Log(string.Format("Try interactive TYPE::InterActions : {0}", objectInfo.transform.name));
                    PlayerHackingInterActionObject(objectInfo);
                    if (!soundCutOff)
                    {
                        _PlayerSound.PlayerHackingSFX(_SoundPlay);
                        soundCutOff = true;
                    }
                }
            }
        }

        public void PlayerHackingPlatform(ObjectInfo objectInfo)
        {
            //Debug.Log(string.Format("Platform Hacking Now : {0}", objectInfo.name));

            objectInfo._IsHack = true;

        }

        public void PlayerHackingDrone(ObjectInfo objectInfo)
        {
            objectInfo._IsHack = true;
        }

        public void PlayerHackingCancle(ObjectInfo objectInfo)
        {
            objectInfo._IsHack = false;
        }

        public void PlayerControlDrone(ObjectInfo objectInfo, DroneType droneType)
        {
            if(droneType == DroneType.EMP)
            {
                _DroneEMP.SetActive(true);
                _DroneEMP.transform.position = objectInfo.transform.position;
                _DroneEMP.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
                //Rigidbody = _DroneEMP.GetComponent<Rigidbody>();
                _ControlObject = _DroneEMP;
                _CameraMovement.SetPivotObject(_DroneEMP);
                _HUDManager.CurHUDType = HUDManager.HUDType.DronHUD;
            }

            _ControlDroneInfo = objectInfo.GetComponent<DroneInfo>();
            objectInfo.gameObject.SetActive(false);
        }

        public void PlayerControlDroneCancle()
        {
            //Rigidbody = this.gameObject.GetComponent<Rigidbody>();
            _ControlObject = this.gameObject;
            _ControlDroneInfo.gameObject.SetActive(true);
            _ControlDroneInfo.transform.position = _DroneEMP.transform.position;
            _ControlDroneInfo.HackingReset();
            _DroneEMP.SetActive(false);
            _CameraMovement.SetPivotObject(this.gameObject);
            _HUDManager.CurHUDType = HUDManager.HUDType.PlayerHUD;


        }

        public void PlayerHackingInterActionObject(ObjectInfo objectInfo)
        {
            objectInfo._IsHack = true;
        }

        public void LandingEffect()
        {
            Instantiate(_LandingEffect, transform.position, Quaternion.identity);
        }

        public void PlayerDamaged()
        {
            if (GameOverCheck(--PlayerInfo._hp))
            {
                Debug.Log("GameOver");
                pAnimator.Play("GameOver");

            }
            else
            {
                Debug.Log("Hit");
                pAnimator.Play("Damaged");
                Instantiate(_DamageEffect, _PlayerPelvis);
                _PlayerSound.PlayerDamagedSFX(true);
            }
        }

        public void PlayerSupportFuncReturnToPlace()
        {
            GameManager.DamagedAfter(_ControlObject);
        }

        public void GameOverAnimSupport()
        {
            GameManager.GameOver();
        }

        public bool GameOverCheck(int hp)
        {
            bool isGameOver = false;

            if(hp<= 0)
            {
                isGameOver = true;
            }

            return isGameOver;
        }

        public void AnimationUpdate(bool isGrounded, bool isAim, bool isHack)
        {
            float jumpAmount = _Rigidbody.velocity.y;
            jumpAmount = Mathf.Clamp(jumpAmount, -9, 5);

            _ForwardAmount = Mathf.Abs(_ForwardAmount);

            //Debug.Log(isHack);

            _Animator.SetBool("Grounded", isGrounded);
            _Animator.SetFloat("Forward", _ForwardAmount, 0.1f, Time.deltaTime);
            _Animator.SetFloat("Turn", _TurnAmount, 0.1f, Time.deltaTime);
            _Animator.SetBool("Aim", isAim);
            _Animator.SetBool("Hacking", isHack);

            if (!isGrounded)
            {
                _Animator.SetFloat("Jump", jumpAmount);
            }


        }

        private void OnCollisionStay(Collision collision)
        {
            if(collision != null)
            {
                //Debug.Log(collision.transform.name);
            }
        }

        bool GameOver(int damage)
        {
            bool isGameOver = false;
                
            return isGameOver;
        }
    }
}
