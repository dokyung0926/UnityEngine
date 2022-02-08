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
            // ==================== 방법 1 : 총알을 총구에 생성 =======================
            Instantiate(bullet,firePoint);
            firePoint.DetachChildren();

            // ============ 방법 2 : 총알을 생성 후 총구에 위치시킨다 =============
            // GameObject의 인스턴스화 :
            // GameObject tmpBullet = Instantiate(bullet);
            // 클래스의 인스턴스화 :
            // 클래스타입 변수이름 = new 클래스생성자;
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
