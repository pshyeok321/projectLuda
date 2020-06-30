using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPooling : MonoBehaviour
{
    public static ObjPooling _objPool; // 모든 클래스 직접 호출 가능
    public GameObject PoolObj_A;
    public GameObject PoolObj_B;
    public GameObject Play_Obj; // obj를 playobj 아래로 생성
    public int PoolAmount_A = 1;
    public int PoolAmount_B = 1;
    public List<GameObject> PoolObjs_A;
    public List<GameObject> PoolObjs_B;
  
    private void Awake()
    {
        _objPool = this;
    }

    void Start()
    {
        PoolObjs_A = new List<GameObject>();
        PoolObjs_B = new List<GameObject>();

        for(int i=0; i<PoolAmount_A; i++)
        {
            GameObject obj_A = (GameObject)Instantiate(PoolObj_A);
           
            obj_A.transform.parent = Play_Obj.transform; // 자식obj를 부모obj밑으로 생성

            //PoolObj->obj에 저장
            obj_A.SetActive(false);
            PoolObjs_A.Add(obj_A);
            //instantiate로 그려지고 비활성화된 상태의 오브젝트를 poolobjs에 차곡차곡 넣는다.
        }

        for (int i = 0; i < PoolAmount_B; i++)
        {
            GameObject obj_B = (GameObject)Instantiate(PoolObj_B);
            obj_B.transform.parent = Play_Obj.transform; // 자식obj를 부모obj밑으로 생성

            //PoolObj->obj에 저장
            obj_B.SetActive(false);
            PoolObjs_A.Add(obj_B);
            //instantiate로 그려지고 비활성화된 상태의 오브젝트를 poolobjs에 차곡차곡 넣는다.
        }
    }

    public GameObject GetPooledObject_A()
    {
        for(int i=0; i<PoolObjs_A.Count; i++)
        {
            //obj.setactive가 false면 실행
            if (!PoolObjs_A[i].activeInHierarchy)
            {
                //cube_a()에서 넘어온 obj.setactive에 true 된 A 호출
                return PoolObjs_A[i];
            }
        }
        return null;
    }

    public GameObject GetPooledObject_B()
    {
        for (int i = 0; i < PoolObjs_B.Count; i++)
        {
            //obj.setactive가 false면 실행
            if (!PoolObjs_B[i].activeInHierarchy)
            {
                //cube_a()에서 넘어온 obj.setactive에 true 된 A 호출
                return PoolObjs_A[i];

                
            }
        }
        return null;
    }


    void Update()
    {
        
    }
}
