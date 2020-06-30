using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSCANCHASE : DroneFSMState
{
    int min, max;

   
  
    public override void BeginState()
    {
        base.BeginState();

        StartCoroutine(UpdatePath());

    }

    public override void EndState()
    {
        StopCoroutine(UpdatePath());
        base.EndState();
    }

    private void Update()
    {
        if (_manager.isScan)
        {            
             
                if (Vector3.Distance(_manager.list[_manager._objTypeCount].transform.position, transform.position) < _manager.Stat.AttackRange)
                {
                    _manager.SetState(DroneState.SCAN);
                    return;
                }
                   

        }

        
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
        PathRequestManager.RequestPath(new PathRequest(transform.position, _manager.list[_manager._objTypeCount].transform.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = _manager.list[_manager._objTypeCount].transform.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            print(((_manager.list[_manager._objTypeCount].transform.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
            if ((_manager.list[_manager._objTypeCount].transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, _manager.list[_manager._objTypeCount].transform.position, OnPathFound));
                targetPosOld = _manager.list[_manager._objTypeCount].transform.position;
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
                transform.Translate(Vector3.forward * Time.deltaTime * _manager.Stat.MoveSpeed* speedPercent, Space.Self);
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
