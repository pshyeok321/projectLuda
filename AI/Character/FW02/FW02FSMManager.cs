using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;
using UnityEngine.UI;
using Player;
using UnityEngine.AI;

public enum FW02State
{
    IDLE = 0,
    PATROL,
    CHASE,
    ATTACK,
    INCAP,
    SIREN,
    CHASESTART,
    CHASEEND,
    DEAD,
}

[RequireComponent(typeof(FW02Stat))]
[ExecuteInEditMode]
public class FW02FSMManager : FSMManager
{
    
    private bool _isinit = false;
    public FW02State startState = FW02State.IDLE;
    private Dictionary<FW02State, FW02FSMState> _states = new Dictionary<FW02State, FW02FSMState>();

    [SerializeField]
    private FW02State _currentState;
    public FW02State CurrentState
    {
        get
        {
            return _currentState;
        }
    }

    private CharacterController _cc;
    public CharacterController CC { get { return _cc; } }

    //private CharacterController _playercc;
    //public CharacterController PlayerCC { get { return _playercc; } }

    private CapsuleCollider _playercs;
    public CapsuleCollider PlayerCS { get { return _playercs; } }

    private Transform _playerTransform;
    public Transform PlayerTransform { get { return _playerTransform; } }
    
    private FW02Stat _stat;
    public FW02Stat Stat { get { return _stat; } }

    private Animator _anim;
    public Animator Anim { get { return _anim; } }

    public int _patrolcount = 0;

    //public GameObject[] SightLight;



    [Header("클릭 시 시계방향으로 Patrol 합니다.")]
    public bool isClockwise;

    [Header("Patrol 할 위치 지정")]
    public Transform[] patrolSpots;

    [Header("Spot들의 위치를 가지고 있는 부모 GameObject")]
    [SerializeField]
    GameObject _Node;

    List<Transform> list = new List<Transform>();

    public int _listCount;

    //public Transform _lastTransform;
    //public Transform _lastPlayerTransform;

    //public int _alertCount = 0;
    //public Vector3 lastPlayerVector;

    public GameObject Effect_First;
    public GameObject Effect_Shoot;

    public static FW02FSMManager FindFW02FSM() {
        return GameObject.Find("FW02(Prefab)").GetComponentInChildren<FW02FSMManager>();
    }

    PlayerInfo playerinfo;

    public bool isWall = false;
    public bool isPlayer = false;

    [SerializeField]
    // list로 했지만 우린 값들이 있음.
    public NavMeshAgent agent;
    protected override void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;


        base.Awake();
        //_UIs = _UICanvas.GetComponentsInChildren<Image>();

        list.Clear();
        
        
        patrolSpots = _Node.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < patrolSpots.Length - 1; i++)
        {
            list.Add(patrolSpots[i]);
        }
        _listCount = list.Count;


        //Debug.Log(list.Count);


        SetGizmoColor(Color.blue);
        _cc = GetComponent<CharacterController>();
      
        _stat = GetComponent<FW02Stat>();
        _anim = GetComponentInChildren<Animator>();

        //_playercc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        _playercs = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
        _playerTransform = _playercs.transform;

        //_objectbc = GameObject.FindGameObjectWithTag("Cabinet").GetComponent<BoxCollider>();
        


        FW02State[] stateValues = (FW02State[])System.Enum.GetValues(typeof(FW02State));
        foreach (FW02State s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("FW02" + s.ToString());
            FW02FSMState state = (FW02FSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (FW02FSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }


    }
    public void SetState(FW02State newState)
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
        SetState(FW02State.IDLE);
    }

    public override void SetDeadState()
    {
        SetState(FW02State.DEAD);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Siren")
        {
            SetState(FW02State.SIREN);
            return;
        }
        if (other.transform.tag == "Effect")
        {
            SetState(FW02State.INCAP);
            return;
        }
    }
    public override bool IsDie() { return CurrentState == FW02State.DEAD; }

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
