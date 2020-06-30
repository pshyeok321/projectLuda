using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects;
public class MP01SIREN : MP01FSMState {
    float _time = 0;
    float IncapTime = 7f;

    private List<GameObject> _obj = new List<GameObject>();
    public List<GameObject> Obj { get { return _obj; } }

    public ObjectType _objType;

    public List<GameObject> list = new List<GameObject>();

    float _objDistance;

    public override void BeginState() {
        base.BeginState();

        _obj.Clear();
        list.Clear();
        _obj.AddRange(GameObject.FindGameObjectsWithTag("Siren"));         // 오브젝트 갯수 파악


        for (int i = 0; i < _obj.Count; i++) { //오브젝트 갯수 만큼 for문

            ObjectInfo[] _objectInfo = new ObjectInfo[_obj.Count]; // 오브젝트 갯수만큼 
            _objectInfo[i] = _obj[i].GetComponent<ObjectInfo>(); // objectinfo 생성

            if (_objectInfo[i] == null)
                continue;

            _objType = _objectInfo[i].ObjectType; // 타입체크

            if (_objType == ObjectType.NonType) //만약 그게 클로킹박스면
            {
                _objDistance = Vector3.Distance(transform.position, _obj[i].transform.position);// 드론과 박스의 거리차이 확인
                list.Add(_obj[i]); // 리스트 추가
                list.Sort(delegate (GameObject g1, GameObject g2) { return Vector3.Distance(g1.transform.position, transform.position).CompareTo(Vector3.Distance(g2.transform.position, transform.position)); });
            }
        }
    }

    public override void EndState() {
        base.EndState();

    }
    private void Update() {
        _time += Time.deltaTime;

        if (_time > IncapTime) {
            _manager.SetState(MP01State.PATROL);
            return;
        }
        if (Vector3.Distance(list[0].transform.position, transform.position) > 0.3f)
            _manager.CC.CKMove(list[0].transform.position, _manager.Stat);
    }
}
