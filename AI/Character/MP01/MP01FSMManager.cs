using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;
using UnityEngine.UI;
using Player;
using UnityEngine.AI;

public enum MP01State
{
    IDLE = 0,
    PATROL,
    ALERT,
    WATCH,    
    CWATCH,
    CHASE,
    ATTACK,
    INCAP,
    SIREN,
    LOSTTARGET,
    DEAD,
}

[RequireComponent(typeof(MP01Stat))]
[ExecuteInEditMode]
public class MP01FSMManager : FSMManager
{
    
    private bool _isinit = false;
    public MP01State startState = MP01State.IDLE;
    private Dictionary<MP01State, MP01FSMState> _states = new Dictionary<MP01State, MP01FSMState>();

    [SerializeField]
    private MP01State _currentState;
    public MP01State CurrentState
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
    
    private MP01Stat _stat;
    public MP01Stat Stat { get { return _stat; } }

    private Animator _anim;
    public Animator Anim { get { return _anim; } }

    public int _patrolcount = 0;

    public GameObject[] SightLight;



    [Header("클릭 시 시계방향으로 Patrol 합니다.")]
    public bool isClockwise;

    [Header("Patrol 할 위치 지정")]
    public Transform[] patrolSpots;

    [Header("Spot들의 위치를 가지고 있는 부모 GameObject")]
    [SerializeField]
    GameObject _Node;

    List<Transform> list = new List<Transform>();

    public int _listCount;

    public Transform _lastTransform;
    public Transform _lastPlayerTransform;

    public int _alertCount = 0;

    public bool isAlert = false;

    public Vector3 lastPlayerVector;

    public Path path;
    public GameObject UIPosition, Emotion1, Emotion2;

    public bool isAlertPatrol = false;

    public GameObject AttackEffect;

    public static MP01FSMManager FindMP01FSM() {
        return GameObject.Find("MP01(Prefab)").GetComponentInChildren<MP01FSMManager>();
    }
    PlayerInfo playerinfo;

    [SerializeField]
    protected GameObject _UICanvas;
    public Image[] _UIs;

    // list로 했지만 우린 값들이 있음.
    public NavMeshAgent agent;
    protected override void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;


        base.Awake();
        _UICanvas = GetComponentInChildren<Canvas>().gameObject;
        //_UIs = _UICanvas.GetComponentsInChildren<Image>();

        Emotion1.transform.position = UIPosition.transform.position;
        Emotion2.transform.position = UIPosition.transform.position;
        list.Clear();
        
        
        lastPlayerVector = new Vector3(0, 0, 0);
        patrolSpots = _Node.transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < patrolSpots.Length - 1; i++)
        {
            list.Add(patrolSpots[i]);
        }
        _listCount = list.Count;


        //Debug.Log(list.Count);


        SetGizmoColor(Color.blue);
        _cc = GetComponent<CharacterController>();
      
        _stat = GetComponent<MP01Stat>();
        _anim = GetComponentInChildren<Animator>();

        //_playercc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        _playercs = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
        _playerTransform = _playercs.transform;

        //_objectbc = GameObject.FindGameObjectWithTag("Cabinet").GetComponent<BoxCollider>();
        


        MP01State[] stateValues = (MP01State[])System.Enum.GetValues(typeof(MP01State));
        foreach (MP01State s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("MP01" + s.ToString());
            MP01FSMState state = (MP01FSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (MP01FSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }

        SightLight[1].SetActive(false);
        SightLight[2].SetActive(false);

    }
    public void SetState(MP01State newState)
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
        SetState(MP01State.IDLE);
    }

    public override void SetDeadState()
    {
        SetState(MP01State.DEAD);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Siren")
        {
            SetState(MP01State.SIREN);
        }

        if (other.tag == "Effect")
        {
            SetState(MP01State.INCAP);
            SightLight[0].SetActive(false);
        }
    }
    
    public override bool IsDie() { return CurrentState == MP01State.DEAD; }

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
