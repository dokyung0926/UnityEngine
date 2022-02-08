using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerfire : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bomb;
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            // ==================== ��� 1 : �Ѿ��� �ѱ��� ���� =======================
            Instantiate(bullet,firePoint);
            firePoint.DetachChildren();

            // ============ ��� 2 : �Ѿ��� ���� �� �ѱ��� ��ġ��Ų�� =============
            // GameObject�� �ν��Ͻ�ȭ :
            // GameObject tmpBullet = Instantiate(bullet);
            // Ŭ������ �ν��Ͻ�ȭ :
            // Ŭ����Ÿ�� �����̸� = new Ŭ����������;
            // tmpBullet.transform.position = firePoint.position;
            // tmpBullet.transform.rotation = firePoint.rotation;
        }
        if (Input.GetKeyDown("b"))
        {
            Instantiate(bomb,firePoint);
            firePoint.DetachChildren();
            
        }
    }
}
