using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    // 캠이 1등을 따라가게끔 하고 싶음
    // 필요한것
    // 1. 카메라 자체의 Transform 컴포넌트
    // 2. 경주말들의 Transform 컴포넌트 

    // 뭘 해야 하는가 ?
    // 1. 경주말들의 등수를 실시간으로 갱신
    // 2. 1등 말의 위치를 가져온다 
    // 3. 카메라의 위치를 1등 말의 위치에 특정  거리만큼 떨어트려 위치시킨다.

    Transform tr;

    public Vector3 offset;
    Transform target;
    int targetIndex;

    private void Start()
    {
        tr = this.gameObject.GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("tab"))
            SwitchNextTarget();

        if (target == null)
            SwitchNextTarget();
        else
        tr.position = target.position + offset;
    }

    // 다음 플레이어로 타겟을 변경하는 기능
    public void SwitchNextTarget()
    {
        targetIndex++;
        if (targetIndex > RacingPlay.instance.GetTotalPlayerNumber() - 1)
            targetIndex = 0;
        target = RacingPlay.instance.GetPlayer(targetIndex);
    }

    public void SwitchTargetTo1Grade()
    {
        target = RacingPlay.instance.Get1GradePlayer();
    }
}
