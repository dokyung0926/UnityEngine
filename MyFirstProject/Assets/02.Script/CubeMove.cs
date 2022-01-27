using UnityEngine;

public class CubeMove : MonoBehaviour
{
    Transform tr;
    // public 접근 제한자를 사용하면 Inspector 창에 노출된다.
    // 만약 다른 클래스로부터의 접근은 제한하면서 Inspector 창에 노출시키고 싶으면 [SerializedField] 속성을 사용한다 .
    [SerializeField]private float speed = 1; // Inspector 창의 값이 초기화 값보다 우선순위다. 
    public float rotateSpeed; 

    // Start is called before the first frame update
    void Start()
    {
        // transform에 접근해서 좌표에 대한 데이터를 변경시켜도 되지만 굳이 Transform 멤벼변수 tr을 선언해서 
        // Transform component를 대입한 후에 사용하는 이유
        // 캐시 메모리 문제 때문 ( 캐시 : 임시로 연산이 용이하도록 생성한 메모리 )
        // transform을 사용하면 이 멤버변수를 호출할 때 마다 gameObject에 접근해서 getComponent로  Transform 성분을 가져옴 .
        // 하지만 Transform 멤버면수 tr에다 한번 넣어놓고 사용하면 
        // tr은 사용할때마다 처음에 넣어줬더 Transform component에 바호 접근하기 때문에
        // 동시에 아주 많은 게임오브젝트들의 Transform 컴포넌트를 써야하면 그때는 퍼포먼스에서 차이가 난다 .

        tr = gameObject.GetComponent<Transform>(); // 게임오브젝트에게서 Transform 컴포넌트를 가져온다 .
        tr = this.gameObject.GetComponent<Transform>(); // 이 클래스를 포함하는 게임오브젝트에게서 Transform 컴포넌트를 가져온다 .
        // tr = this.gameObject.transform; // 게임오브젝트의 멤버변수 transform을 대입한다 . ( 멤버변수 transform에 뭐가 들어있는지 내부 )
        // tr = gameObject.transform;
    }

    // Update는 매 프레임마다 호출되는 함수 .
    void Update()
    {
        // Positions
        // ===================================================================================================
        // 1 프레임당 z축 1 전진
        // 만약에 컴퓨터 사양이 달라서 하나는 60FPS, 다른 하나는 30FPS
        // >> 1초에 하나는 60만큼 전진하고, 다를 하나는 30만큼 전진하게됨 .
        // tr.position += new Vector3(0, 0, 1 * Time.deltaTime);
        // Time.deltaTime 직전프레임과 현재 프레임 사이 걸린 시간
        // 즉, Time.deltaTime 을 곱해주면 기기성능에 관계없이 초당 같은 변화량을 가질 수 있다.
        // tr.position += new Vector3(0, 0, 1) * Time.deltaTime;

        // Physics 관련 데이터를 처리할 때 자주 사용함 .
        // tr.position += new Vector3(0, 0, 1) * Time.fixedDeltaTime;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Debug.Log("h =" + h);
        Debug.Log($"v = {v}");
        // Z axis foward, back
        // X axis left, right
        // Y axis up, down


        // 이처럼 쓸 경우 동시에 여러 축으로 움직였을때 방향벡터의 크기가 1이 넘어가서 방향에따라 속도가 일정하지 않은 현상 발생 .
        // Vector3 movePos = new Vector3(h, 0,v) * speed * Time.deltaTime;
        // tr.Translate(movePos);


        // Vector : 방향과 크기를 모두 가지는 성질
        // 특히 Vector크기가 1인 벡터를 단위벡터 (Unit Vector)
        // 움직이고 싶은 방향에대한 단위벡터 * 속도 로 물체를 움직임
        Vector3 dir = new Vector3(h, 0, v).normalized;
        Vector3 moveVec = dir * speed * Time.deltaTime;
        tr.Translate(moveVec);

        // tr.Translate(moveVec, Space.Self); // local 좌표계 기준 이동
        tr.Translate(moveVec, Space.World); // World 좌표계 기준 이동

        // Rotation
        // ================================================================================
        // tr.Rotate(new Vector3(0f, 30f, 0f)); // Y축으로 30 radian 만큼 회전함 . Degree 0~360 까지 나타내는 단위 , Radian 0 ~ 2 pi단위

        float r = Input.GetAxis("Mouse X");
        Vector3 rotateVector = Vector3.up * rotateSpeed * r * Time.deltaTime;
        tr.Rotate(rotateVector);
    }
}
