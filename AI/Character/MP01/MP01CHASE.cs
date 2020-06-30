using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MP01CHASE : MP01FSMState
{

    float ChaseTime = 15f;

    float RunTime = 1f;
    float UITime = 3f;
    [SerializeField]
    float _time = 0;

    public Vector3 playerTrans, playerT1, playerT2, playerT3;

    public bool last1, last2;

    public const float minPathUpdateTime = .2f;
    public const float pathUpdateMoveThreshold = .5f;

    


    public override void BeginState() {
        base.BeginState();

        playerT1 = new Vector3(1, 1, 1);
        playerT2 = new Vector3(2, 2, 2);
        playerT3 = new Vector3(3, 3, 3);
        playerTrans = new Vector3(_manager.PlayerTransform.position.x, transform.position.y, _manager.PlayerTransform.position.z);
        
        //  StartCoroutine("UpdatePath");
    }


    public override void EndState() {
        base.EndState();

      //  StopAllCoroutines();
        _time = 0;
        last1 = false;
        last2 = false;

        _manager.Emotion2.SetActive(false);
        _manager._UIs[2].fillAmount = 0;

        _manager.agent.isStopped = true;
    }


    private void Update() {

        _time += Time.deltaTime;
        _manager._UIs[2].fillAmount = 1 - (_time /15f);
        //1초 동안 플레이어 도망갈 시간 줌.
        if (!last1) {
            _manager.agent.destination = playerTrans;
            _manager.agent.isStopped = false;
            //_manager.CC.CKMove(playerTrans, _manager.Stat);
            //transform.LookAt(playerTrans);
            //UI.setactive(true);
            if (_time >= UITime) {
                //UI.setactive(false);
                _manager.Emotion2.SetActive(false);
                last1 = true;
                playerT1 = new Vector3(_manager.PlayerTransform.position.x, transform.position.y, _manager.PlayerTransform.position.z);                
            }            
        }
        // playerT1 -> playerT2로 이동
        if (last1 && !last2) {
            _manager.agent.destination = playerT1;
            _manager.agent.isStopped = false;
            //_manager.CC.CKMove(playerT1, _manager.Stat);
            //transform.LookAt(playerT1);
            if (Vector3.Distance(playerT1, transform.position) < 1f) {
                last2 = true;
                playerT2 = new Vector3(_manager.PlayerTransform.position.x, transform.position.y, _manager.PlayerTransform.position.z);
            }
        }
        // playerT2의 정보를 가져오기 위한 bool 값
        if (last2) {
            float _timer = 0;
            float MoveTime = 5f;
            _timer += Time.deltaTime;

            //5초 동안만 이동
            if (_timer <= MoveTime) {
                _manager.agent.destination = playerT2;
                _manager.agent.isStopped = false;
                //_manager.CC.CKMove(playerT2, _manager.Stat);
                //transform.LookAt(playerT2);
            }
        }


        // 1초 시간 준 후에 시야에 발각되면 Attack으로 보내줌. attack에서는 체력-1시키는거와 체크포인트로 이동시키는걸 함. 그리고 다시 경계모드로 돌아감.           
        if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS)) {
            _manager.SetState(MP01State.ATTACK);
            return;
        }

        if (_time >= ChaseTime) {
            _manager.SetState(MP01State.ALERT);
            _manager.SightLight[2].SetActive(false);

        }
        //3초간 ! UI 출력

        //3초간 거리와 상관없이 인식
        //유저에게 이동 = _manager.CKMove(_manager.PlayerTransform.position, _manager.Stat);

        // 범위 안에 적이 존재 할 시 HP-- 시킴.
        // 아니면 거리 인식 종료 및 그 위치 저장.
        // Chase 시작 할 때 위치로 이동
        // 하다가 시간 되면 경계모드로 변경
    }


    #region Astar
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
        if (pathSuccessful) {
            _manager.path = new Path(waypoints, transform.position, _manager.Stat.TurnDst, _manager.Stat.StoppingDst);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public IEnumerator UpdatePath() {

        if (Time.timeSinceLevelLoad < .3f) {
            yield return new WaitForSeconds(.3f);
        }
        if (!last1) {
            PathRequestManager.RequestPath(new PathRequest(transform.position, _manager.PlayerTransform.position, OnPathFound));

            float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
            Vector3 targetPosOld = _manager.PlayerTransform.position;

            while (true) {
                yield return new WaitForSeconds(minPathUpdateTime);
                print(((_manager.PlayerTransform.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
                if ((_manager.PlayerTransform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, _manager.PlayerTransform.position, OnPathFound));
                    targetPosOld = _manager.PlayerTransform.position;
                }
            }
        }

        if (last1 && !last2) {
            PathRequestManager.RequestPath(new PathRequest(transform.position, playerT1, OnPathFound));

            float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
            Vector3 targetPosOld = playerT1;

            while (true) {
                yield return new WaitForSeconds(minPathUpdateTime);
                print(((playerT1 - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
                if ((playerT1 - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, playerT1, OnPathFound));
                    targetPosOld = playerT1;
                }
            }
        }

        if (last2) {
            PathRequestManager.RequestPath(new PathRequest(transform.position, playerT2, OnPathFound));

            float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
            Vector3 targetPosOld = playerT2;

            while (true) {
                yield return new WaitForSeconds(minPathUpdateTime);
                print(((playerT2 - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
                if ((playerT2 - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
                    PathRequestManager.RequestPath(new PathRequest(transform.position, playerT2, OnPathFound));
                    targetPosOld = playerT2;
                }
            }
        }
    }

    IEnumerator FollowPath() {

        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(_manager.path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath) {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (_manager.path.turnBoundaries[pathIndex].HasCrossedLine(pos2D)) {
                if (pathIndex == _manager.path.finishLineIndex) {
                    followingPath = false;
                    break;
                } else {
                    pathIndex++;
                }
            }

            if (followingPath) {

                if (pathIndex >= _manager.path.slowDownIndex && _manager.Stat.StoppingDst > 0) {
                    speedPercent = Mathf.Clamp01(_manager.path.turnBoundaries[_manager.path.finishLineIndex].DistanceFromPoint(pos2D) / _manager.Stat.StoppingDst);
                    if (speedPercent < 0.01f) {
                        followingPath = false;
                    }
                }

                Quaternion targetRotation = Quaternion.LookRotation(_manager.path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _manager.Stat.TurnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * _manager.Stat.MoveSpeed, Space.Self);
            }

            yield return null;

        }
    }

    //#if UNITY_EDITOR
    //public void OnDrawGizmos() {
    //    if (_manager.path != null) {
    //        _manager.path.DrawWithGizmos();
    //    }
    //}
    //#endif

    #endregion
}
