using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;

public enum DroneState
{
    IDLE = 0,
    PATROL,
    CHASE,    
    ATTACKDELAY,
    ATTACK,
    ATTACKCANCLE,
    SCANCHASE,
    SCAN,
    GOUPSTAIR,
    WAITUPSTAIR,
    DEAD,
}

[RequireComponent(typeof(DroneStat))]
[ExecuteInEditMode]
public class DroneFSMManager : FSMManager
{
    private bool _isinit = false;
    public DroneState startState = DroneState.IDLE;
    private Dictionary<DroneState, DroneFSMState> _states = new Dictionary<DroneState, DroneFSMState>();

    [SerializeField]
    private DroneState _currentState;
    public DroneState CurrentState
    {
        get
        {
            return _currentState;
        }
    }

    //private CharacterController _cc;
    //public CharacterController CC { get { return _cc; } }

    private CapsuleCollider _cc;
    public CapsuleCollider CC { get { return _cc; } }
    Rigidbody _rigid;
    private CharacterController _playercc;
    public CharacterController PlayerCC { get { return _playercc; } }

    private CapsuleCollider _playercs;
    public CapsuleCollider PlayerCS { get { return _playercs; } }

    private Transform _playerTransform;
    public Transform PlayerTransform { get { return _playerTransform; } }

    private DroneStat _stat;
    public DroneStat Stat { get { return _stat; } }

    private Animator _anim;
    public Animator Anim { get { return _anim; } }

    public int _patrolcount;

    private List<GameObject> _obj = new List<GameObject>();
    public List<GameObject> Obj { get { return _obj; } }

    public ObjectType _objType;

    public List<GameObject> list = new List<GameObject>();

    public int listcount;
    //private Dictionary<>
    //    private Dictionary<DroneState, DroneFSMState> _states = new Dictionary<DroneState, DroneFSMState>();

    public int _objTypeCount = 0;

    float _objDistance;

    public bool isScan;

    public Path path;

    public bool isSecond;

    protected override void Awake()
    {
        base.Awake();

        // _objType = ObjectType.ClockingBox;
        
        //_obj = new GameObject[20];
        _obj.Clear();
        list.Clear();
        _obj.AddRange(GameObject.FindGameObjectsWithTag("Object"));         // 오브젝트 갯수 파악


        Debug.Log(_obj.Count);
        for (int i = 0; i < _obj.Count; i++)
        { //오브젝트 갯수 만큼 for문
            ObjectInfo[] _objectInfo = new ObjectInfo[_obj.Count]; // 오브젝트 갯수만큼 
            _objectInfo[i] = _obj[i].GetComponent<ObjectInfo>(); // objectinfo 생성

            if (_objectInfo[i] == null)
                continue;

            Debug.Log(_objectInfo[i].transform.name); // objectinfo 갖고있는거 체크

            _objType = _objectInfo[i].ObjectType; // 타입체크

            if (_objType == ObjectType.Platforms) //만약 그게 클로킹박스면
            {
                _objDistance = Vector3.Distance(transform.position, _obj[i].transform.position);// 드론과 박스의 거리차이 확인
                list.Add(_obj[i]); // 리스트 추가
                list.Sort(delegate (GameObject g1, GameObject g2) { return Vector3.Distance(g1.transform.position, transform.position).CompareTo(Vector3.Distance(g2.transform.position, transform.position)); });
            }

        }
        listcount = list.Count;


        _patrolcount = 0;
        SetGizmoColor(Color.blue);
        //_cc = GetComponent<CharacterController>();
        _cc = GetComponent<CapsuleCollider>();
        _rigid = GetComponent<Rigidbody>();
        _stat = GetComponent<DroneStat>();
        _anim = GetComponentInChildren<Animator>();

        _playercc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        _playercs = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
        _playerTransform = _playercs.transform;




        DroneState[] stateValues = (DroneState[])System.Enum.GetValues(typeof(DroneState));
        foreach (DroneState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Drone" + s.ToString());
            DroneFSMState state = (DroneFSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (DroneFSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }

    }
    public void SetState(DroneState newState)
    {
        if (_isinit)
        {
            _states[_currentState].enabled = false;
            _states[_currentState].EndState();
        }
        _currentState = newState;
        _states[_currentState].BeginState();
        _states[_currentState].enabled = true;
        //_anim.SetInteger("CurrentState", (int)_currentState);
    }

    private void Start()
    {
        SetState(startState);
        _isinit = true;
    }

    public override void NotifyTargetKilled()
    {
        SetState(DroneState.IDLE);
    }

    public override void SetDeadState()
    {
        SetState(DroneState.DEAD);
    }

    public override bool IsDie() { return CurrentState == DroneState.DEAD; }
}
