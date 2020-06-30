using System.Collections;
using System.Collections.Generic;
using CameraProduction;
using Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public enum ForwardSituation { Normal, Block, JumpCorrecting }
    
    [RequireComponent(typeof(PlayerBehaviour))]
    public class PlayerController : MonoBehaviour
    {
        public static List<KeyCode> _PlayerInputKeyCodeList = new List<KeyCode>();
        public static void AddKeyCodeList(KeyCode keyCode)
        {
            _PlayerInputKeyCodeList.Add(keyCode);
        }


        [System.NonSerialized] public ForwardSituation forwardSituation;

        public delegate void HackingCallBack(bool isFinished, ObjectType objectType);
        public HackingCallBack hackingCallBack;

        public delegate void PlayerInputCallBack(List<KeyCode> keyCode);
        public PlayerInputCallBack _PlayerInputCallBack;

        public GameObject _SoundManager;

        public Animator PlayerAnimator;
        public Animator EMPDroneAnimator;

        private Vector3 direction = Vector3.zero;
        private bool isPause = false;

        public bool _MoveInputable = true;

        private bool _IsControl = true;

        private bool _IsJump = false;
        private int _JumpCount = 0;

        private bool _IsDash = true;

        private bool _IsLadder = false;

        private bool _IsClimb = false;  
        private float _ClimbUptime = 0;
        private float _ClimbMaxtime = 0.3f;
        private bool _IsHang = false;

        //[System.NonSerialized]
        public bool _IsHacking = false;
        [System.NonSerialized] public bool _IsHacked = false;
        private bool _OnRide = false;

        [System.NonSerialized] public bool _IsLock = false;

        Vector3 addVelocity;
        [System.NonSerialized]public Vector3 setVelocity = Vector3.forward;

        private bool canClocking = false;
        private bool canHide = false;
        [System.NonSerialized] public bool isClocking = false;
        [System.NonSerialized] public bool isHide = false;

        PlayerSound _PlayerSound;
        PlayerBehaviour _PlayerBehaviour;
        PlayerInfo _playerInfo;
        [System.NonSerialized] public GameObject _RotateXObject;


         public bool _IsChange = false;
        [SerializeField] private bool _SoundPlay = false;

        [Space(10)]
        [Header("- External reference")]
        public GameObject _Aim;
        [SerializeField] public bool _IsAim;

        public GameObject _ParentObject;
        public GameObject _AroundCenter;

        CameraMovement _CameraMovement;
        CameraCollision _CameraCollision;

        public float additionalPosX;
        public float additionalPosY;
        [SerializeField] private float _ZoomInScale;

        Collider other;

        int layerMask;

        [Space(10)]
        [Header("- Ground/Forward Checker")]
        public GameObject _GroundChecker;
        private bool _PrevIsGrounded;

        private bool _IsGrounded;
        public bool IsGrounded {
            get => _IsGrounded;
            internal set => _IsGrounded = value;
        }

        private float _GroundCheckDistance = 0.15f;

        public GameObject[] _ForwardChecker;
        private float _ForwardCheckDistance = 0.15f;

        public float _Gravity;
        private const float _InverseValue = 1.0f;
        private const float _DetectSphereRadius = 0.2f;
        public float climbSpeed;

        RaycastHit hitInfo;
        ObjectInfo _PrevAimInfo;
        ObjectInfo _AimInfo;

        RaycastHit _GroundHit;

        private Vector3 _OldPos;
        float footstepTimer = 0.0f;
        bool soundCutOff;

        public bool ishackingFinished = false;

        [Space(10)]
        [Header("-")]
        public bool isTestScene;

        private GameObject _HackEffect;
        [SerializeField] private GameObject _HackingNotDoEffect;
        [SerializeField] private GameObject _HackingDoEffect;

        public GameObject _DescriptionWindow;
        public Image _DescriptionHackingIcon;
        public Sprite _LockSprite;
        public Text _Nametag;
        public Text _Description;
        public Image _RequirementImage;
        public Image _RequirementQuantity;

        public GameObject _PlayerModel;
        public GameObject _DroneModel;
        public GameObject _DroneEffect;

        public float _DroneTimer = 0;
        public float _DroneMaxTimer = 0;

        public float _SkillTimer = 0;
        public float _SkillMaxTimer = 0;

        public float _NonInputTime = 0;

        //임시
        Vector3 _SirenPos;
        public bool _IsSiren = false;

        private void Awake()
        {
            _SoundManager = GameManager.GetSoundManager().gameObject;

            layerMask = (1 << 2);
            layerMask = ~layerMask;

            _PlayerBehaviour = GetComponent<PlayerBehaviour>();
            _playerInfo = GetComponent<PlayerInfo>();
            _PlayerSound = GetComponent<PlayerSound>();

            _CameraMovement = Camera.main.GetComponentInParent<CameraMovement>();
            _CameraCollision = Camera.main.GetComponent<CameraCollision>();

            _HackEffect = _HackingNotDoEffect;
            _HackingNotDoEffect.SetActive(false);
            _HackingDoEffect.SetActive(false);
            _DescriptionWindow.SetActive(false);
            
        }

        private void FixedUpdate()
        {
            if (_playerInfo._hp > 0)
            {
                try
                {
                    if ((!Input.anyKey &&
                        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.activeSelf))
                        _NonInputTime += Time.deltaTime;
                    else _NonInputTime = 0;
                }
                catch
                {

                }
            }

            if(_NonInputTime >= 5f)
            {
                _PlayerBehaviour.pAnimator.Play("Idle2");
                _NonInputTime = 0.0f;
            }

            _PrevAimInfo = _AimInfo;
            _PrevIsGrounded = IsGrounded;

            IsGrounded = false;
            _IsControl = true;

            PlayerOnGroundCheck();
            PlayerLegForwardCheck();

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (!_IsChange && _PlayerInputKeyCodeList.Contains(KeyCode.Q)) {
                if (_IsSiren)
                {
                    _SirenPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                    ItemManager.HologramValue--;
                    GameObject Siren = Instantiate(_PlayerBehaviour._Siren, _SirenPos, Quaternion.identity);
                    _PlayerSound.PlayerSkillHologramSFX(_SoundPlay);
                    _IsSiren = false;
                }
            }

            if (!_IsChange && _MoveInputable && _PlayerInputKeyCodeList.Contains(KeyCode.Space))
            {
                _PlayerBehaviour.PlayerJump(_IsJump, _JumpCount);

                if (_IsAim)
                {
                    LockOnCancle();
                    _CameraCollision.SetAimModeCorrect(Vector3.zero, GameStandardInfo.StandardMainCameraFieldofView);

                    _IsAim = false;
                }

                if (!_IsJump) _IsJump = true;
                _JumpCount++;
            }

            if (_IsJump && _PlayerInputKeyCodeList.Contains(KeyCode.LeftShift))
            {
                if (!_IsDash)
                {
                    _PlayerBehaviour.PlayerDash();
                    //if (_JumpCount >= 1) { _JumpCount = 1; }
                    _IsDash = true;
                }
            }

            direction = v * _CameraMovement.transform.forward
                + h * _CameraMovement.transform.right;

            direction.y = 0;

            if (_PlayerInputKeyCodeList.Contains(KeyCode.LeftControl))
            {
            }

            GameObject aimTarget;

            if (!_IsHacking && !_IsJump)
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    _IsAim = true;
                    _CameraCollision.SetAimModeCorrect(new Vector3(additionalPosX, additionalPosY, 0), _ZoomInScale);
                    //TakingAim();
                    aimTarget = HackingBehaviour.TakingAim(Camera.main.transform);

                    if (aimTarget != null)
                    {
                        _AimInfo = aimTarget.GetComponentInParent<ObjectInfo>();
                        _DescriptionWindow.SetActive(true);
                        _Description.text = _AimInfo._Discripstion;
                        _Nametag.text = _AimInfo._Name;

                        if (_AimInfo._Icon != null)
                        {
                            _DescriptionHackingIcon.sprite = _AimInfo._Icon;
                        }
                        else
                        {
                            _DescriptionHackingIcon.sprite = _LockSprite;
                        }

                        if (ItemManager.KeyValue >= _AimInfo._KeyRequirement)
                        {
                            _RequirementImage.sprite = HUDManager.GetHUDManager()._CyanIcons[1];
                            _RequirementQuantity.sprite = HUDManager.GetHUDManager()._CyanNumberFonts[_AimInfo._KeyRequirement];
                            
                        }
                        else
                        {
                            _RequirementQuantity.sprite = HUDManager.GetHUDManager()._RedNumberFonts[_AimInfo._KeyRequirement];
                            _RequirementImage.sprite = HUDManager.GetHUDManager()._RedIcons[1];
                        }

                    }
                    else
                    {
                        _AimInfo = null;
                        _DescriptionWindow.SetActive(false);
                        _Description.text = null;
                        _Nametag.text = null;
                        _DescriptionHackingIcon.sprite = null;
                    }
                }

                if (!Input.GetKey(KeyCode.Mouse1))
                {
                    _IsAim = false;
                    _CameraCollision.SetAimModeCorrect(Vector3.zero, GameStandardInfo.StandardMainCameraFieldofView);

                    _DescriptionWindow.SetActive(false);
                    _Description.text = null;
                    _Nametag.text = null;
                    _DescriptionHackingIcon.sprite = null;

                    if (_AimInfo != null && _AimInfo._HackingMode)
                    {
                        _PlayerBehaviour.PlayerHackingCancle(_AimInfo);
                    }

                    if (_AimInfo != null)
                    {
                        _AimInfo = null;
                    }
                }
            }

            if (_IsAim && !_IsChange)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (_AimInfo != null && _AimInfo._HackingMode)
                    {
                        if (ItemManager.KeyValue >= _AimInfo._KeyRequirement)
                        {
                            _MoveInputable = false;
                            _IsHacking = true;
                            _PlayerSound.PlayerHackStartSFX(_SoundPlay);
                            soundCutOff = false;
                            _HackEffect = _HackingDoEffect;
                            _HackingNotDoEffect.SetActive(false);
                        }
                    }

                }

                if (Input.GetKey(KeyCode.F))
                {
                    if (_PrevAimInfo == null)
                    {

                    }

                    else
                    {
                        if (_PrevAimInfo == _AimInfo && _AimInfo != null && _AimInfo._HackingMode)
                        {
                            if (ItemManager.KeyValue >= _AimInfo._KeyRequirement)
                            {
                                _MoveInputable = false;
                                _PlayerBehaviour.PlayerHacking(_AimInfo, ref soundCutOff, _SoundPlay);
                                Transform target = _AimInfo.transform;
                                LockOnHackingTarget(target);
                            }
                        }

                        // 조준이 빗나간 경우
                        if (_PrevAimInfo != _AimInfo && _AimInfo == null)
                        {
                            _MoveInputable = true;
                            _PlayerBehaviour.PlayerHackingCancle(_PrevAimInfo);
                            LockOnCancle();
                        }
                    }
                }

                if (!Input.GetKey(KeyCode.F))
                {
                    if (_AimInfo != null && _AimInfo._HackingMode)
                    {
                        _PlayerBehaviour.PlayerHackingCancle(_AimInfo);
                        LockOnCancle();
                    }
                    _MoveInputable = true;


                    _HackEffect = _HackingNotDoEffect;
                    _HackingDoEffect.SetActive(false);
                }

                if(_AimInfo == null)
                {

                }
            }

            if (!_IsLock)
            {
            }

            // 조작이 바뀐경우
            if (_IsChange)
            {
                if(_AimInfo != null)
                {
                    ObjectType objectType = _AimInfo.ObjectType;
                    if (objectType == ObjectType.Drones)
                    {
                        _DroneTimer += Time.deltaTime;
                        _SkillTimer += Time.deltaTime;

                        if (_DroneTimer > _DroneMaxTimer)
                        {
                            _PlayerBehaviour.PlayerControlDroneCancle();
                            LockOnCancle();
                            _MoveInputable = true;
                            _IsHacking = false;
                            _IsChange = false;

                            _PlayerBehaviour.pAnimator = PlayerAnimator;

                            AkSoundEngine.SetRTPCValue("Bgm_EmpHack", 100, _SoundManager);
                            AkSoundEngine.SetRTPCValue("Bgm_Camera", 100, _SoundManager);
                        }

                        if (_PlayerInputKeyCodeList.Contains(KeyCode.Q) && _SkillTimer > _SkillMaxTimer)
                        {
                            _PlayerBehaviour.pAnimator.Play("Release");
                            GameObject effects = Instantiate(_DroneEffect, _PlayerBehaviour._DroneEMP.transform.position, Quaternion.identity);

                            _SkillTimer = 0.0f;
                        }
                    }
                }

                // 해킹(조작) 해제
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("Press F during Hacking");

                    if (_AimInfo != null)
                    {
                        ObjectType objectType = _AimInfo.ObjectType;

                        if (objectType == ObjectType.Drones)
                        {
                            _PlayerBehaviour.PlayerControlDroneCancle();
                            LockOnCancle();
                            _MoveInputable = true;
                            _IsHacking = false;
                            _PlayerBehaviour.pAnimator = PlayerAnimator;

                            //GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<DroneSound>().PlayDroneEMPHackingEndSFX(_DroneModel);
                        }

                        if (objectType == ObjectType.CCTV)
                        {
                            LockOnCancle();
                            _IsHacking = false;
                            _MoveInputable = true;
                        }

                        // 해킹 해제시 변수 초기화
                        //_IsHacking = false;
                        _IsChange = false;

                        _HackEffect = _HackingNotDoEffect;
                        _HackingDoEffect.SetActive(false);
                        _Description.text = null;
                        _Nametag.text = null;

                        AkSoundEngine.SetRTPCValue("Bgm_EmpHack", 100, _SoundManager);
                        AkSoundEngine.SetRTPCValue("Bgm_Camera", 100, _SoundManager);
                    }
                }
            }

            if (_PlayerInputKeyCodeList.Contains(KeyCode.Mouse0))
            {
                
            }

            if (_PlayerInputKeyCodeList.Contains(KeyCode.V))
            {
                //Debug.Log(canClocking);
                if (canClocking)
                {
                    isClocking = !isClocking;
                }

                if (isClocking)
                {
                    _CameraCollision._AdditionalPosition = new Vector3(additionalPosX, 0, 0);

                }
                else
                {
                    _CameraCollision._AdditionalPosition = Vector3.zero;
                }
            }


            if (_PlayerInputKeyCodeList.Contains(KeyCode.Escape))
            {
                _PlayerInputKeyCodeList.Clear();
                Cursor.lockState = CursorLockMode.Confined;
                GameManager.PauseGame();
                isPause = !isPause;

            }

            //if (Input.GetKeyDown(UP)) { }
            //if (Input.GetKeyDown(DOWN)) { }
            //if (Input.GetButtonDown(ENTER))
            //{

            //}

            //if (isPause) { Time.timeScale = 0.0f; }
            //else { Time.timeScale = 1.0f; }


            _Aim.SetActive(_IsAim);
            _HackEffect.SetActive(_IsAim || _IsHacking);

            //Debug.Log(string.Format("Current Hacking State : {0}",_IsHacking));

            if (_MoveInputable)
                _PlayerBehaviour.PlayerMove(direction, _GroundHit, _IsJump, _IsChange, _IsAim, forwardSituation);

            if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && IsGrounded)
            {
                if (footstepTimer > 0.3f)
                {
                    _PlayerSound.PlayerFootstepSFX(_SoundPlay);
                    //Instantiate(_PlayerBehaviour._DashEffect, transform.position, Quaternion.identity);
                    footstepTimer = 0.0f;
                }

                footstepTimer += Time.deltaTime;
            }

            //_PlayerBehaviour.UseAdditionalGravity(IsGrounded);
            _PlayerBehaviour.AnimationUpdate(IsGrounded, _IsAim, _IsHacking);
            _PlayerInputKeyCodeList.Clear();
        }
        
        public void LockOnHackingTarget(Transform target)
        {
            _CameraMovement.LockOnMode = true;
            //Debug.Log(string.Format("LockOn Target : {0}", target.name));

            ObjectInfo targetInfo = target.GetComponentInParent<ObjectInfo>();

            Camera.main.transform.LookAt(targetInfo._LookAtTarget);
        }

        public void LockOnCancle()
        {
            _CameraMovement.LockOnMode = false;
            Camera.main.transform.localRotation = Quaternion.Euler(24.0f,0.0f,0.0f);

        }

        public void NotifyFinishedHacking(bool isFinished, ObjectType objectType)
        {
            if (isFinished)
            {
                //Debug.Log(string.Format("Hacking Success Callback : {0}", objectType));

                _PlayerSound.PlayerHackingSFXCancle(_SoundPlay);
                _PlayerSound.PlayerHackingSuccessSFX(_SoundPlay);

                if (objectType == ObjectType.Platforms)
                {
                    Input.ResetInputAxes();
                    _IsHacking = false;
                    _MoveInputable = true;
                }

                if(objectType == ObjectType.Drones)
                {
                    DroneInfo droneInfo = null;
                    try
                    {
                        droneInfo = _AimInfo.GetComponent<DroneInfo>();
                        _DroneMaxTimer = droneInfo._DroneMaxHackingTime;
                        _DroneTimer = 0.0f;
                    }
                    catch
                    {
                        _PlayerBehaviour.PlayerControlDroneCancle();
                    }
                    //Debug.Log(string.Format("DroneInfo Name : {0}", droneInfo.name));
                    DroneType droneType = droneInfo.DroneType;

                    _PlayerBehaviour.PlayerControlDrone(droneInfo, droneType);
                    _CameraCollision.SetAimModeCorrect(Vector3.zero, GameStandardInfo.StandardMainCameraFieldofView);
                    _IsHacking = true;
                    _IsChange = true;

                    _PlayerBehaviour.pAnimator = EMPDroneAnimator;

                    if (GameManager._CurSceneNum == 0)
                    {
                        if (EventManager.curMission == 2)
                        {
                            EventManager.curMission++;
                        }
                    }

                    if (GameManager._CurSceneNum == 1)
                    {
                        if (EventManager.curMission == 2 && EventManager.curSubject == 1)
                            EventManager.curMission++;
                    }



                    AkSoundEngine.SetRTPCValue("Bgm_EmpHack", 30, _SoundManager);
                    AkSoundEngine.SetRTPCValue("Bgm_Camera", 100, _SoundManager);
                }

                if(objectType == ObjectType.CCTV)
                {
                    _CameraCollision.SetAimModeCorrect(Vector3.zero, GameStandardInfo.StandardMainCameraFieldofView);
                    _IsHacking = true;
                    _IsChange = true;
                    _IsAim = false;


                    AkSoundEngine.SetRTPCValue("Bgm_EmpHack", 100, _SoundManager);
                    AkSoundEngine.SetRTPCValue("Bgm_Camera", 30, _SoundManager);
                }

                _MoveInputable = true;
                ItemManager.KeyValue -= _AimInfo._KeyRequirement;
                Debug.Log(ItemManager.KeyValue);

                LockOnCancle();
            }
            else
            {
                //Debug.Log(string.Format("Hacking Fail Callback: {0}", objectType));
                if (objectType == ObjectType.Platforms)
                {
                    _PlayerSound.PlayerHackingSFXCancle(_SoundPlay);
                    _PlayerSound.PlayerHackingFailSFX(_SoundPlay);
                    soundCutOff = false;
                    _IsHacking = false;
                }

                if(objectType == ObjectType.Drones)
                {
                    _PlayerSound.PlayerHackingSFXCancle(_SoundPlay);
                    _PlayerSound.PlayerHackingFailSFX(_SoundPlay);
                    soundCutOff = false;
                    _IsHacking = false; 
                    _IsChange = false;
                }

                if(objectType == ObjectType.CCTV)
                {
                    _PlayerSound.PlayerHackingSFXCancle(_SoundPlay);
                    _PlayerSound.PlayerHackingFailSFX(_SoundPlay);
                    soundCutOff = false;
                    _IsHacking = false;
                    _IsChange = false;
                }

                _MoveInputable = true;
                LockOnCancle();
            }
        }

        #region Gizmos

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_GroundChecker.transform.position, _DetectSphereRadius);

            RaycastHit CheckObject;

            Ray GroundCheckRay = new Ray
            {
                origin = _GroundChecker.transform.position,
                direction = -_GroundChecker.transform.up
            };
            Debug.DrawRay(_GroundChecker.transform.position, (GroundCheckRay.direction * _GroundCheckDistance));

            if (Physics.SphereCast(GroundCheckRay, _DetectSphereRadius, out CheckObject,
                _GroundCheckDistance, layerMask))
            {
                Gizmos.DrawWireSphere(CheckObject.point, _DetectSphereRadius);
            }
            else
            {
                Gizmos.DrawWireSphere(GroundCheckRay.origin +
                    (GroundCheckRay.direction * _GroundCheckDistance), _DetectSphereRadius);
            }

            foreach (GameObject forward in _ForwardChecker)
            {

                Gizmos.DrawWireSphere(forward.transform.position, _DetectSphereRadius);

                Ray ForwardCheckRay = new Ray
                {
                    origin = forward.transform.position,
                    direction = forward.transform.forward
                };
                Debug.DrawRay(ForwardCheckRay.origin, ForwardCheckRay.direction * _ForwardCheckDistance);

                if (Physics.SphereCast(ForwardCheckRay, _DetectSphereRadius, out CheckObject,
                    _ForwardCheckDistance, layerMask))
                {
                    Gizmos.DrawWireSphere(CheckObject.point, _DetectSphereRadius);
                }
                else
                {
                    Gizmos.DrawWireSphere(ForwardCheckRay.origin +
                        (ForwardCheckRay.direction * _ForwardCheckDistance), _DetectSphereRadius);
                }
            }

        }
        #endregion

        public void PlayerOnGroundCheck()
        {
            //Debug.Log("GroundChecking");
            Ray GroundCheckRay = new Ray
            {
                origin = _GroundChecker.transform.position,
                direction = -_GroundChecker.transform.up
            };
            RaycastHit CheckObject;

            if (Physics.SphereCast(GroundCheckRay, _DetectSphereRadius, out CheckObject, _GroundCheckDistance, layerMask))
            {
                //Debug.Log(CheckObject.transform.name);
                DetectHit(ref _GroundHit, _GroundChecker.transform, _GroundCheckDistance);

                IsGrounded = true;
                _IsJump = false;
                _JumpCount = 0;
                _IsDash = false;
                _ClimbUptime = 0;

                if (_PrevIsGrounded != IsGrounded)
                {
                    _PlayerSound.PlayerLandingSFX(_SoundPlay);
                }

                ObjectInfo hitInfo = ObjectInfoCheck(CheckObject);
                ObjectType objectType = ObjectTypeCheck(CheckObject);

                if (objectType == ObjectType.Platforms)
                {
                    try
                    {
                        PlatformInfo platforms = hitInfo.GetComponentInParent<PlatformInfo>();

                        if (platforms._PlatformType == PlatformType.Move)
                        {
                            MovePlatform movePlatform = platforms.GetComponentInParent<MovePlatform>();
                            _PlayerBehaviour.PlayerRideOnPlatform(movePlatform._CurrentSpeed);
                        }

                        if (platforms._PlatformType == PlatformType.Rotate)
                        {
                            RotatePlatform rotatePlatform = CheckObject.transform.GetComponentInParent<RotatePlatform>();
                            if (rotatePlatform._Direction == RotatePlatform.RotateDirection.Y)
                                _PlayerBehaviour.PlayerAround(rotatePlatform._Operator.transform.position, rotatePlatform._TurnSpeed);
                        }
                    }

                    catch
                    {

                    }
                }

            }

            else
            {
                IsGrounded = false;
                //_IsRope = false;
                //this.transform.parent = _ParentObject.transform;

                if (_PlayerBehaviour.Rigidbody.velocity.y <= 0.2f)
                    _PlayerBehaviour.Rigidbody.velocity += Vector3.down * _Gravity * Time.deltaTime;
            }

            //if (_PrevIsGrounded == IsGrounded)
            //{
            //    Quaternion quaternion = this.transform.rotation;
            //    quaternion.x = 0;

            //    this.transform.rotation = quaternion;
            //}
        }

        void PlayerLegForwardCheck()
        {
            bool[] isCheck = new bool[_ForwardChecker.Length];
            
            for(int i=0; i<_ForwardChecker.Length; i++)
            {
                isCheck[i] = false;
            }

            for(int i=0; i< _ForwardChecker.Length; i++)
            {
                hitInfo = new RaycastHit();

                DetectHit(ref hitInfo, _ForwardChecker[i].transform, _ForwardCheckDistance);

                if (hitInfo.collider != null)
                {
                    //Debug.Log(string.Format("Checker[{0}] -> transform name : {1}", i, hitInfo.collider.name));
                    isCheck[i] = true;
                    //ObjectInfo objectInfo = new ObjectInfo();
                    //ObjectType objectType = ObjectTypeCheck(hitInfo);

                }
            }

            if (!isCheck[0])
            {
                forwardSituation = ForwardSituation.Normal;
            }

            if(isCheck[0] && isCheck[1])
            {
                forwardSituation = ForwardSituation.Block;
            }

            if(!IsGrounded && isCheck[0])
            {
                forwardSituation = ForwardSituation.JumpCorrecting;
            }
        }

        public ObjectInfo ObjectInfoCheck(RaycastHit hit)
        {
            ObjectInfo objectInfo;

            try
            {
                objectInfo = hit.transform.GetComponentInParent<ObjectInfo>();
                //Debug.Log(string.Format("Checking ObjectInfo : {0}", objectInfo.transform.name));
            }
            catch
            {
                objectInfo = null;
            }

            return objectInfo;
        }

        public ObjectType ObjectTypeCheck(RaycastHit hit)
        {
            ObjectInfo objectInfo;

            try
            {
                objectInfo = hit.transform.GetComponentInParent<ObjectInfo>();
                Debug.Log(objectInfo.transform.name);
            }
            catch
            {
                objectInfo = null;
            }

            ObjectType objectType;

            if(objectInfo == null)
            {
                objectType = ObjectType.NonType;
            }
            else
            {
                objectType = objectInfo.ObjectType;
            }

            return objectType;
        }

        void DetectHit(ref RaycastHit detectedHit, Transform transform, float distance)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, distance);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == this.GetComponent<Collider>())
                    continue;

                if (hit.collider.isTrigger)
                    continue;

                if (detectedHit.collider == null || hit.distance < detectedHit.distance)
                    detectedHit = hit;
            }
        }

        public void OnCollisionStay(Collision other)
        {
            if (!IsGrounded && other.contacts[0].normal.y < 0.1f)
            {
                //if (Input.GetKeyDown(JUMP))
                //{
                //    Debug.DrawRay(other.contacts[0].point, other.contacts[0].normal, Color.red, 1.25f);

                //    transform.rotation = Quaternion.LookRotation(other.contacts[0].normal * 1f);
                //    _PlayerBehaviour.Rigidbody.AddForce(other.contacts[0].normal * 100f);
                //}
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.transform.tag == "ClockingSight")
            {
                Debug.Log("aa");
                
                this.other = other;
                canClocking = true;
            }

            if (other.transform.tag == "Hide")
            {
                this.other = other;
                canHide = true;
            }

            if(other.transform.tag == "Drones")
            {
                this.other = other;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.transform.tag == "ClockingSight")
            {
                this.other = null;
                canClocking = false;
                isClocking = false;
            }

            if (other.transform.tag == "Hide")
            {
                this.other = null;
                canHide = false;
            }
        }

        public void GameResume()
        {
            isPause = false;
            Time.timeScale = 1.0f;
            _PlayerInputKeyCodeList.Clear();
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void GameRestart()
        {
            isPause = false;
            Time.timeScale = 1.0f;
            _PlayerInputKeyCodeList.Clear();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
