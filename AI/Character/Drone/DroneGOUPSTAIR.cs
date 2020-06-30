using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;
public class DroneGOUPSTAIR : DroneFSMState
{
    private List<GameObject> _obj = new List<GameObject>();
    public List<GameObject> Obj { get { return _obj; } }

    public ObjectType _objType;

    public List<GameObject> list = new List<GameObject>();

    float _objDistance;

    public const float minPathUpdateTime = .2f;
    public const float pathUpdateMoveThreshold = .5f;

    Vector3 destination;
    public override void BeginState()
    {        
        base.BeginState();
        ListSetting();
        StartCoroutine("UpdatePath");
        Debug.Log("STartCourtine");
        destination = new Vector3(list[0].transform.position.x, 1.18f, list[0].transform.position.z);
    }

    public override void EndState()
    {
        base.EndState();
        StopAllCoroutines();
        Debug.Log("StopCoroutine");

    }

    private void Update()
    {
        if(Vector3.Distance(destination, transform.position) < 2f)
        {
            _manager.SetState(DroneState.WAITUPSTAIR);
            return;            
        }
    }
    void ListSetting()
    {
        _obj.Clear();
        list.Clear();
        _obj.AddRange(GameObject.FindGameObjectsWithTag("Object"));         // 오브젝트 갯수 파악
        Debug.Log(_obj.Count);
        for (int i = 0; i < _obj.Count; i++)
        { //오브젝트 갯수 만큼 for문
            ObjectInfo[] _objectInfo = new ObjectInfo[_obj.Count]; // 오브젝트 갯수만큼 
            _objectInfo[i] = _obj[i].GetComponent<ObjectInfo>(); // objectinfo 생성

            if (_objectInfo[i] == null)
                continue;

            Debug.Log(_objectInfo[i].transform.name); // objectinfo 갖고있는거 체크

            _objType = _objectInfo[i].ObjectType; // 타입체크

            if (_objType == ObjectType.Platforms) //만약 그게 클로킹박스면
            {
                _objDistance = Vector3.Distance(transform.position, _obj[i].transform.position);// 드론과 박스의 거리차이 확인
                list.Add(_obj[i]); // 리스트 추가
                list.Sort(delegate (GameObject g1, GameObject g2) { return Vector3.Distance(g1.transform.position, transform.position).CompareTo(Vector3.Distance(g2.transform.position, transform.position)); });
            }
        }
    }
    #region Astar
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
        PathRequestManager.RequestPath(new PathRequest(transform.position, list[0].transform.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = list[0].transform.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            print(((list[0].transform.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
            if ((list[0].transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, list[0].transform.position, OnPathFound));
                targetPosOld = list[0].transform.position;
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
                transform.Translate(Vector3.forward * Time.deltaTime * _manager.Stat.MoveSpeed, Space.Self);
            }

            yield return null;

        }
    }

    //#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (_manager.path != null)
        {
            _manager.path.DrawWithGizmos();
        }
    }
    //#endif

    #endregion
}
