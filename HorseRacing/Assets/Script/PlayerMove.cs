using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Transform tr;
    [HideInInspector]public float distance;
    public Vector3 dir;
    public float minSpeed;
    public float maxSpeed;
    public bool doMove;

    void Start()
    {
        tr = gameObject.GetComponent<Transform>();
        RacingPlay.instance.Register(this);
        // �Ϲ������� Instance�� �ʱ�ȭ�� Awake �Լ�����,
        // Instance�� �����ϴ� ������ Start �Լ����� �����Ѵ�
    }

    // Update is called once per frame
    void Update()
    {
        if (doMove)
            Move();
    }

    private void Move()
    {
        float moveSpeed = Random.Range(minSpeed, maxSpeed);
        Vector3 moveVec = dir * moveSpeed * Time.deltaTime;
        tr.Translate(moveVec);
        distance += moveVec.magnitude;
    }
}
