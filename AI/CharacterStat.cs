using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    protected int _str = 10;
    public int Str { get { return _str; } }

    protected int _maxHp = 3;
    public int MaxHp { get { return _maxHp; } }

    public int _hp = 3;
    public int Hp { get { return _hp; } }

    [SerializeField] protected float _moveSpeed = 3.0f;
    public float MoveSpeed { get { return _moveSpeed; } }

    [SerializeField] protected float _turnSpeed = 180.0f;
    public float TurnSpeed { get { return _turnSpeed; } }

    protected float _attackRange = 2.0f;
    public float AttackRange { get { return _attackRange; } }    

    protected float _aroundSpeed = 60f;
    public float AroundSpeed { get { return _aroundSpeed; } }

    [SerializeField] protected float _JumpPower = 265f;
    public float JumpPower { get { return _JumpPower; } }

    [SerializeField] protected float _DashPower = 500f;
    public float DashPower { get { return _DashPower; } }

    protected float _turnDst = 5f;
    public float TurnDst { get { return _turnDst; } }

    protected float _stoppingDst = 10f;
    public float StoppingDst { get { return _stoppingDst; } }



    [HideInInspector]
    public CharacterStat lastHitBy = null;
    [SerializeField]
    protected StatData statData;

    protected virtual void Awake()
    {
        //_turnSpeed = statData.TurnSpeed;
        
    }

    public void TakeDamage(CharacterStat from, int damage)
    {
        _hp = Mathf.Clamp(_hp - damage, 0, _maxHp);
        if (_hp <= 0)
        {
            if (lastHitBy == null)
                lastHitBy = from;

            //GetComponent<FSMManager>().SetDeadState();
            //from.GetComponent<FSMManager>().NotifyTargetKilled();
        }

        //if(player)
    }

    private static int CalcDamage(CharacterStat from, CharacterStat to)
    {
        return from.Str;
    }

    public static void ProcessDamage(CharacterStat from, CharacterStat to)
    {
        int finalDamage = CalcDamage(from, to);
        to.TakeDamage(from, finalDamage);
    }

    public static void ProcessDamage(CharacterStat from, CharacterStat to, int damage)
    {
        int finalDamage = damage;
        to.TakeDamage(from, finalDamage);
    }
}
