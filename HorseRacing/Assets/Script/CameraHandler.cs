using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    #region �̱���
    static public CameraHandler instance;
    private void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    Transform tr;
    Transform target;
    int targetIndex;
    public Vector3 offset;
    [SerializeField]public Transform platformCamPoint;
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

    public void MoveToPlatform()
    {
        tr.position =  platformCamPoint.position;
        tr.rotation = platformCamPoint.rotation;
    }
}
