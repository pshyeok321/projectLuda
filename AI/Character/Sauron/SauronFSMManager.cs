using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;

public enum SauronState
{
    IDLE = 0,
    RIGHTSIGHT,
    LEFTSIGHT,
    ATTACK,
    DEAD,
    //ATTACKDELAY,
    //ATTACKCANCLE,
}

[RequireComponent(typeof(SauronStat))]
[ExecuteInEditMode]
public class SauronFSMManager : FSMManager
{
    private bool _isinit = false;
    public SauronState startState = SauronState.IDLE;
    private Dictionary<SauronState, SauronFSMState> _states = new Dictionary<SauronState, SauronFSMState>();

    [SerializeField]
    private SauronState _currentState;
    public SauronState CurrentState
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
    
    private SauronStat _stat;
    public SauronStat Stat { get { return _stat; } }

    private Animator _anim;
    public Animator Anim { get { return _anim; } }

    public int _sightCount;
        
    public GameObject[] SightLight;

    public bool isSighting;

    protected override void Awake()
    {
        base.Awake();

        SetGizmoColor(Color.blue);
        _cc = GetComponent<CharacterController>();
        _stat = GetComponent<SauronStat>();
        _anim = GetComponentInChildren<Animator>();

        //_playercc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        _playercs = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
        _playerTransform = _playercs.transform;
        
        //SightLight = new GameObject[4];

        SauronState[] stateValues = (SauronState[])System.Enum.GetValues(typeof(SauronState));
        foreach (SauronState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("Sauron" + s.ToString());
            SauronFSMState state = (SauronFSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (SauronFSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }

    }

    public void SetState(SauronState newState)
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

        //StartPos_Vec_A = StartPos_A.transform.position;
        //StartPos_Vec_B = StartPos_B.transform.position;
    }

    public override void NotifyTargetKilled()
    {
        SetState(SauronState.IDLE);
    }

    public override void SetDeadState()
    {
    //    SetState(SauronState.DEAD);
    }

    //public override bool IsDie() { return CurrentState == SauronState.DEAD; }
}
