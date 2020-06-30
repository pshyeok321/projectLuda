using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;
using Player;
public enum MS04State
{
    IDLE = 0,
    PATROL,
    ATTACK,
    INCAP,
    INCAPEND,
    DEAD,
}

[RequireComponent(typeof(MS04Stat))]
[ExecuteInEditMode]
public class MS04FSMManager : FSMManager
{    
    private bool _isinit = false;
    public MS04State startState = MS04State.IDLE;
    private Dictionary<MS04State, MS04FSMState> _states = new Dictionary<MS04State, MS04FSMState>();

    [SerializeField]
    private MS04State _currentState;
    public MS04State CurrentState { get { return _currentState; } }

    private CharacterController _cc;
    public CharacterController CC { get { return _cc; } }       

    private CapsuleCollider _playercs;
    public CapsuleCollider PlayerCS { get { return _playercs; } }

    private Transform _playerTransform;
    public Transform PlayerTransform { get { return _playerTransform; } }
    
    private MS04Stat _stat;
    public MS04Stat Stat { get { return _stat; } }

    private Animator _anim;
    public Animator Anim { get { return _anim; } }

    public int _patrolcount = 1;

    public GameObject[] SightLight;
    
    

    [Header("클릭 시 시계방향으로 Patrol 합니다.")]
    public bool isClockwise;
    [Header("MS04_1전용 이펙트")]
    public GameObject EMP;

    [Header("클릭 시 MS04_2몬스터로 변형")]
    public bool isChangeMS04;
    [Header("MS04_2전용 이펙트")]
    public GameObject EMPLOOP;

    [Header("Patrol 할 위치 지정")]
    public Transform[] patrolSpots;

    [Header("Spot들의 위치를 가지고 있는 부모 GameObject")]
    [SerializeField]
    GameObject _Node;

    List<Transform> list = new List<Transform>();

    public int _listCount;

 



    protected override void Awake()
    {
        base.Awake();

        list.Clear();
        

        patrolSpots = _Node.transform.GetComponentsInChildren<Transform>();
        for(int i=0; i<patrolSpots.Length -1; i++)
        {
            list.Add(patrolSpots[i]);
        }
        _listCount = list.Count;



        if (isChangeMS04)
        {
            EMPLOOP.SetActive(true);
        }
        else if (!isChangeMS04)
        {
            EMPLOOP.SetActive(false);
        }
        //_obj.Clear();
        //list.Clear();
        //_obj.AddRange(GameObject.FindGameObjectsWithTag("Object"));         // 오브젝트 갯수 파악


        //for (int i = 0; i < _obj.Count; i++)
        //{
        //    ObjectInfo[] _objectinfo = new ObjectInfo[_obj.Count];
        //    _objectinfo[i] = _obj[i].GetComponent<ObjectInfo>();

        //    if (_objectinfo[i] == null)
        //        continue;

        //    _objType = _objectinfo[i]._ObjectType;

        //    if (_objType != ObjectType.Node)
        //    {
        //        list.Add(_obj[i]);
        //    }
        //}

        //Debug.Log(list.Count);

        SetGizmoColor(Color.blue);
        _cc = GetComponent<CharacterController>();
        _stat = GetComponent<MS04Stat>();
        _anim = GetComponentInChildren<Animator>();

        //_playercc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        _playercs = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
        _playerTransform = _playercs.transform;

        //_objectbc = GameObject.FindGameObjectWithTag("Cabinet").GetComponent<BoxCollider>();
        


        MS04State[] stateValues = (MS04State[])System.Enum.GetValues(typeof(MS04State));
        foreach (MS04State s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("MS04" + s.ToString());
            MS04FSMState state = (MS04FSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (MS04FSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }

    }
    public void SetState(MS04State newState)
    {
        if (_isinit)
        {
            _states[_currentState].enabled = false;
            _states[_currentState].EndState();
        }
        _currentState = newState;
        _states[_currentState].BeginState();
        _states[_currentState].enabled = true;
        _anim.SetInteger("CurrentState", (int)_currentState);
    }

    private void Start()
    {
        SetState(startState);

        _isinit = true;
    }

    public override void NotifyTargetKilled()
    {
        SetState(MS04State.IDLE);
    }

    public override void SetDeadState()
    {
        SetState(MS04State.DEAD);
    }

    public override bool IsDie() { return CurrentState == MS04State.DEAD; }


    private void OnDrawGizmos()
    {
        if (patrolSpots == null)
            return;
        Gizmos.color = Color.black;

        for(int i=0; i<patrolSpots.Length -1; i++)
        {
            if (patrolSpots[i + 1] == null)
                return;

            Gizmos.DrawLine(patrolSpots[i].position, patrolSpots[i + 1].position);
        }
    }
}
