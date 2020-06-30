using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactoryBossState
{
    IDLE = 0,
    ATTACKDELAY,
    ATTACKONE,//여기에 패턴 2개 적용
    ATTACKTWO,
    LAGERATTACK, //3패턴
    SLERPATTACK,
    DRAINED, // 4패턴
    DEAD, 
}

[RequireComponent(typeof(FactoryBossStat))]
[ExecuteInEditMode]
public class FactoryBossFSMManager : FSMManager
{
    private bool _isinit = false;
    public FactoryBossState startState = FactoryBossState.IDLE;
    private Dictionary<FactoryBossState, FactoryBossFSMState> _states = new Dictionary<FactoryBossState, FactoryBossFSMState>();

    [SerializeField]
    private FactoryBossState _currentState;
    public FactoryBossState CurrentState
    {
        get
        {
            return _currentState;
        }
    }

    private CharacterController _cc;
    public CharacterController CC { get { return _cc; } }
        
    private CapsuleCollider _playercs;
    public CapsuleCollider PlayerCS { get { return _playercs; } }

    private Transform _playerTransform;
    public Transform PlayerTransform { get { return _playerTransform; } }

    private FactoryBossStat _stat;
    public FactoryBossStat Stat { get { return _stat; } }

    private Animator _anim;
    public Animator Anim { get { return _anim; } }

    public int _patrolcount;

    public bool PT1, PT2, PT3;

    public Transform[] AttackSpots;

    public GameObject AttackDelayEffect;

    public GameObject RingEffet;

    public GameObject LagerEffect;

    public GameObject SlerpEffect;

    public int[] randSpot;

    public Vector3 _playerTrans;

    public static FactoryBossFSMManager FindFactoryBossFSM()
    {
        return GameObject.Find("FactoryBoss(Prefabs)").GetComponentInChildren<FactoryBossFSMManager>();
    }
    
    protected override void Awake()
    {
        randSpot = new int[3];
        _playerTrans = new Vector3(1, 1, 1);

        base.Awake();
        PT1 = false;
        PT2 = false;
        PT3  = false;
        _patrolcount = 0;

        SetGizmoColor(Color.blue);
        _cc = GetComponent<CharacterController>();
        _stat = GetComponent<FactoryBossStat>();
        _anim = GetComponentInChildren<Animator>();

        _playercs = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
        _playerTransform = _playercs.transform;

        FactoryBossState[] stateValues = (FactoryBossState[])System.Enum.GetValues(typeof(FactoryBossState));
        foreach (FactoryBossState s in stateValues)
        {
            System.Type FSMType = System.Type.GetType("FactoryBoss" + s.ToString());
            FactoryBossFSMState state = (FactoryBossFSMState)GetComponent(FSMType);
            if (null == state)
            {
                state = (FactoryBossFSMState)gameObject.AddComponent(FSMType);
            }

            _states.Add(s, state);
            state.enabled = false;
        }

    }

    public void SetState(FactoryBossState newState)
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
        SetState(FactoryBossState.IDLE);
    }

    public override void SetDeadState()
    {
        SetState(FactoryBossState.DEAD);
    }

    public override bool IsDie() { return CurrentState == FactoryBossState.DEAD; }
}
