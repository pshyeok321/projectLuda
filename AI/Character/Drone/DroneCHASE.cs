using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCHASE : DroneFSMState
{  

    public override void BeginState()
    {
        StartCoroutine("UpdatePath");
        base.BeginState();
    }

    public override void EndState()
    {

        base.EndState();
        StopAllCoroutines();
    }

    private void Update()
    {

        // 시야에 없으면서 동시에 추적 시간 5초가 넘었을 시에 아이들로 돌아온다?
        // 복귀타이머 -> 복귀
        // 추적상태에서 attack으로 넘어갈 때 2초 후 공격 시작하는 형식으로 그 사이 애니메이션 작동

        //if (Vector3.Distance(_manager.PlayerTransform.position, transform.position) >= _manager.Stat.AttackRange)
        if (!GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS)) 
        {
            _manager.SetState(DroneState.IDLE);
            return;
        }
        //if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        //{
        //    _manager.SetState(DroneState.ATTACKDELAY);
        //    return;
        //}
        if (Vector3.Distance(_manager.PlayerTransform.position, transform.position) < _manager.Stat.AttackRange)
        {

            _manager.SetState(DroneState.ATTACKDELAY);
            return;
        }
        //_manager.CC.CKMove(_manager.PlayerTransform.position, _manager.Stat);
        //transform.LookAt(_manager.PlayerTransform);
    }

    #region Astar


    public const float minPathUpdateTime = .2f;
    public const float pathUpdateMoveThreshold = .5f;

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _manager.path = new Path(waypoints, transform.position, _manager.Stat.TurnDst, _manager.Stat.StoppingDst);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public IEnumerator UpdatePath()
    {

        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, _manager.PlayerTransform.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = _manager.PlayerTransform.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            print(((_manager.PlayerTransform.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
            if ((_manager.PlayerTransform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, _manager.PlayerTransform.position, OnPathFound));
                targetPosOld = _manager.PlayerTransform.position;
            }
        }
    }

    IEnumerator FollowPath()
    {

        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(_manager.path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (_manager.path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == _manager.path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {

                if (pathIndex >= _manager.path.slowDownIndex && _manager.Stat.StoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(_manager.path.turnBoundaries[_manager.path.finishLineIndex].DistanceFromPoint(pos2D) / _manager.Stat.StoppingDst);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                Quaternion targetRotation = Quaternion.LookRotation(_manager.path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _manager.Stat.TurnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * _manager.Stat.MoveSpeed * speedPercent, Space.Self);
            }

            yield return null;

        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (_manager.path != null)
        {
            _manager.path.DrawWithGizmos();
        }
    }
#endif

#endregion
}
