using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP01WATCH : MP01FSMState {



 

    [SerializeField]
    float ReconCapture = 1.5f; // 접촉인식시간(기획자가 지정한 공식 명칭)
    [SerializeField]
    float _time = 0;
    [SerializeField]
    bool isRecognition; // 위치 저장을 돕는 bool 값

    [SerializeField]
    float GuardCapture = 0.5f;

    public int test = 0;

    Vector3 playerTrans;


    public override void BeginState() {
        base.BeginState();
        // 시작 시 인식 방향을 플레이어 방향으로
        
        _manager.Emotion1.SetActive(true);
    }


    public override void EndState() {
        base.EndState();

        isRecognition = false;
        _time = 0;

        _manager.SightLight[0].SetActive(false);
        //_manager.SightLight[1].SetActive(false);
    }
    //public Transform _lastTransform;
    //public Transform _lastPlayerTransform;

    public Vector3 lastvector;
    public Vector3 lastplayervector;
    private void FixedUpdate()
    {
        playerTrans = new Vector3(_manager.PlayerTransform.position.x, transform.position.y, _manager.PlayerTransform.position.z);
        transform.LookAt(playerTrans);

        if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        {
            _manager._UIs[2].gameObject.SetActive(true);
            _time += Time.deltaTime;
            if (!isRecognition)
            {
                _manager._lastTransform.position = transform.position;
                lastvector = new Vector3(_manager._lastTransform.position.x, transform.position.y, _manager._lastTransform.position.z);
                _manager._lastPlayerTransform.position = _manager.PlayerTransform.position;
                lastplayervector = new Vector3(_manager._lastPlayerTransform.position.x, transform.position.y, _manager._lastPlayerTransform.position.z);
                isRecognition = true;
            }
            if (!_manager.isAlertPatrol)
            {
                _manager._UIs[2].fillAmount = _time/0.67f;
                if (_time >= ReconCapture)
                {
                    _manager.SetState(MP01State.CWATCH);
                    _manager.SightLight[1].SetActive(false);
                    _manager.SightLight[2].SetActive(true);
                    return;
                }
            }
            if (_manager.isAlertPatrol)
            {
                _manager._UIs[2].fillAmount = _time * 2f;
                if (_time >= GuardCapture)
                {
                    _manager.SetState(MP01State.CWATCH);
                    _manager.SightLight[1].SetActive(false);
                    _manager.SightLight[2].SetActive(true);
                    _manager.isAlert = false;
                    return;
                }
            }
        }
        if (_manager._UIs[2].fillAmount >= 1)
        {
            _manager._UIs[2].fillAmount = 1;
        }
        if (_manager._UIs[2].fillAmount <= 0)
        {
            _manager._UIs[2].fillAmount = 0;
        }
    }
    private void Update() {
      

        if (!GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS) && isRecognition) {
            //인식시간보다 덜 인식했다.
            if (_time < ReconCapture) {
                _manager.SetState(MP01State.ALERT);
                //_manager.SightLight[1].SetActive(true);
                return;
            }
        }


    }

}