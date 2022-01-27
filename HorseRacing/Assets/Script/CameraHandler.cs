using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    // ķ�� 1���� ���󰡰Բ� �ϰ� ����
    // �ʿ��Ѱ�
    // 1. ī�޶� ��ü�� Transform ������Ʈ
    // 2. ���ָ����� Transform ������Ʈ 

    // �� �ؾ� �ϴ°� ?
    // 1. ���ָ����� ����� �ǽð����� ����
    // 2. 1�� ���� ��ġ�� �����´� 
    // 3. ī�޶��� ��ġ�� 1�� ���� ��ġ�� Ư��  �Ÿ���ŭ ����Ʈ�� ��ġ��Ų��.

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

    // ���� �÷��̾�� Ÿ���� �����ϴ� ���
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
