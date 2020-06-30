using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePATROL : DroneFSMState
{

    public float _scanTime = 10.0f;
    private float _time = 0.0f;
    private int _scanCount = 100;
    public Vector3 destination;
    public Transform t1, t2, t3, t4;
    int targetIndex;
    Vector3[] path;
    public const float minPathUpdateTime = .2f;
    public const float pathUpdateMoveThreshold = .5f;

    public override void BeginState()
    {
        StartCoroutine("UpdatePath");
        if (!_manager.isSecond)
        {
            if ((_manager._patrolcount % 2) == 0)
            {
                destination = new Vector3(t1.position.x, t1.position.y, t1.position.z);
            }
            else
                destination = new Vector3(t2.position.x, t2.position.y, t2.position.z);
        }
        if (_manager.isSecond)
        {
            if ((_manager._patrolcount % 2) == 0)
            {
                destination = new Vector3(t3.position.x, t3.position.y, t1.position.z);
            }
            else
                destination = new Vector3(t4.position.x, t4.position.y, t2.position.z);
        }
        base.BeginState();


    }

    public override void EndState()
    {        
        base.EndState();
       // StopAllCoroutines();
    }

    private void Update()
    {
        _time += Time.deltaTime;

        //if(_time >=_scanTime && _scanCount > 0)
        //{
        //    _manager.SetState(DroneState.SCANCHASE);
        //    _time = 0;
        //    --_scanCount;
        //    _manager.isScan = true;
        //}


        if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS) && !_manager.isScan)
        {
            _manager.SetState(DroneState.CHASE);
            return;
        }
        //if (!GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS) && !_manager.isScan)
        //{
        //    _manager.SetState(DroneState.IDLE);
        //    return;
        //}
        if (Vector3.Distance(destination, transform.position) < 3f)// && !_manager.isScan)
        {
            _manager.SetState(DroneState.IDLE);
            return;
        }
        if(_manager.PlayerTransform.position.y > transform.position.y + 9f)
        {
            _manager.SetState(DroneState.GOUPSTAIR);
            return;
        }

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
        PathRequestManager.RequestPath(new PathRequest(transform.position, destination, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = destination;

        while (true) {
            yield return new WaitForSeconds(minPathUpdateTime);
            print(((destination - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
            if ((destination - targetPosOld).sqrMagnitude > sqrMoveThreshold) {
                PathRequestManager.RequestPath(new PathRequest(transform.position, destination, OnPathFound));
                targetPosOld = destination;
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
    public void OnDrawGizmos() {
        if (_manager.path != null) {
            _manager.path.DrawWithGizmos();
        }
    }
    //#endif

    #endregion


}
