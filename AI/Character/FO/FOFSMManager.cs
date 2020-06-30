using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;

public enum FOState
{
    IDLE = 0,
    //PATROL,
    //CHASE,
    //BASICATTACK,
    //HILLWIND,
    //RUSH,
    //SOMMON,
    ATTACK,
    DEAD
}

[RequireComponent(typeof(FOStat))]
[ExecuteInEditMode]
public class FOFSMManager : FSMManager
{
    private bool _isinit = false;
    public FOState startState = FOState.IDLE;
    private Dictionary<FOState, FOFSMState> _states = new Dictionary<FOState, FOFSMState>();

    //[HideInInspector]
    //public bool _bOnFound = false;

    [SerializeField]
    private FOState _currentState;
    public FOState CurrentState
    {
        get
        {
            return _currentState;
        }
    }

    private CharacterController _cc;
    public CharacterController CC { get { return _cc; } }

    private CapsuleCollider _playercc;
    public CapsuleCollider PlayerCC { get { return _playercc; } }

    private Transform _playerTransform;
    public Transform PlayerTransform { get { return _playerTransform; } }

    private FOStat _stat;
    public FOStat Stat { get { return _stat; } }

    private Animator _anim;
    public Animator Anim { get { return _anim; } }


    private List<GameObject> _obj = new List<GameObject>();
    public List<GameObject> Obj { get { return _obj; } }

    public ObjectType _objType;

    public List<GameObject> list = new List<GameObject>();

    public int listcount;

    float _objDistance;

    public GameObject Ball;
    public Transform[] BallsPos;
    //public void AttackBehavior()
    //{
    //    _states[CurrentState].AttackBehavior();
    //}
    public GameObject eft;
    protected override void Awake()
    {
        base.Awake();
        eft.transform.localScale = new Vector3(2, 2, 2);
        _obj.Clear();
        list.Clear();
        _obj.AddRange(GameObject.FindGameObjectsWithTag("Object"));

        Debug.Log(_obj.Count);
        for (int i = 0; i < _obj.Count; i++)
        { //오브젝트 갯수 만큼 for문
            ObjectInfo[] _objectInfo = new ObjectInfo[_obj.Count]; // 오브젝트 갯수만큼 
            _objectInfo[i] = _obj[i].GetComponent<ObjectInfo>(); // objectinfo 생성

            if (_objectInfo[i] == null)
                continue;

            Debug.Log(_objectInfo[i].transform.name); // objectinfo 갖고있는거 체크

            _objType = _objectInfo[i].ObjectType; // 타입체크

            if (_objType == ObjectType.Platforms) //만약 그게 엘리베이터면
            {
                _objDistance = Vector3.Distance(transform.position, _obj[i].transform.position);// 드론과 박스의 거리차이 확인
                list.Add(_obj[i]); // 리스트 추가
                list.Sort(delegate (GameObject g1, GameObject g2) { return Vector3.Distance(g1.transform.position, transform.position).CompareTo(Vector3.Distance(g2.transform.position, transform.position)); });
            }
        }
        listcount = list.Count;

        Debug.Log(list[0]);
        Debug.Log(list[1]);
        Debug.Log(list[2]);




        SetGizmoColor(Color.green);
        _cc = GetComponent<CharacterController>();
        _stat = GetComponent<FOStat>();
        //_anim = GetComponentInChildren<Animator>();

        _playercc = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
        _playerTransform = _playercc.transform;

        // 스테이트를 다 담는 처리
        FOState[] stateValues = (FOState[])System.Enum.GetValues(typeof(FOState));
        foreach (FOState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("FO" + s.ToString());
            FOFSMState state = (FOFSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (FOFSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }

    }

    public void SetState(FOState newState)
    {
        if (_isinit)
        {
            _states[_currentState].enabled = false;
            _states[_currentState].EndState();
        }
        _currentState = newState;
        _states[_currentState].BeginState();
        _states[_currentState].enabled = true;
     //   _anim.SetInteger("CurrentState", (int)_currentState);
    }

    private void Start()
    {
        SetState(startState);
        _isinit = true;
    }

    public override void NotifyTargetKilled()
    {
        //_bOnFound = false;
        SetState(FOState.IDLE);
    }

    public override void SetDeadState()
    {
        SetState(FOState.DEAD);
    }

    public override bool IsDie() { return CurrentState == FOState.DEAD; }
}
