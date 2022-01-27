using UnityEngine;

public class CubeMove : MonoBehaviour
{
    Transform tr;
    // public ���� �����ڸ� ����ϸ� Inspector â�� ����ȴ�.
    // ���� �ٸ� Ŭ�����κ����� ������ �����ϸ鼭 Inspector â�� �����Ű�� ������ [SerializedField] �Ӽ��� ����Ѵ� .
    [SerializeField]private float speed = 1; // Inspector â�� ���� �ʱ�ȭ ������ �켱������. 
    public float rotateSpeed; 

    // Start is called before the first frame update
    void Start()
    {
        // transform�� �����ؼ� ��ǥ�� ���� �����͸� ������ѵ� ������ ���� Transform �⺭���� tr�� �����ؼ� 
        // Transform component�� ������ �Ŀ� ����ϴ� ����
        // ĳ�� �޸� ���� ���� ( ĳ�� : �ӽ÷� ������ �����ϵ��� ������ �޸� )
        // transform�� ����ϸ� �� ��������� ȣ���� �� ���� gameObject�� �����ؼ� getComponent��  Transform ������ ������ .
        // ������ Transform ������ tr���� �ѹ� �־���� ����ϸ� 
        // tr�� ����Ҷ����� ó���� �־���� Transform component�� ��ȣ �����ϱ� ������
        // ���ÿ� ���� ���� ���ӿ�����Ʈ���� Transform ������Ʈ�� ����ϸ� �׶��� �����ս����� ���̰� ���� .

        tr = gameObject.GetComponent<Transform>(); // ���ӿ�����Ʈ���Լ� Transform ������Ʈ�� �����´� .
        tr = this.gameObject.GetComponent<Transform>(); // �� Ŭ������ �����ϴ� ���ӿ�����Ʈ���Լ� Transform ������Ʈ�� �����´� .
        // tr = this.gameObject.transform; // ���ӿ�����Ʈ�� ������� transform�� �����Ѵ� . ( ������� transform�� ���� ����ִ��� ���� )
        // tr = gameObject.transform;
    }

    // Update�� �� �����Ӹ��� ȣ��Ǵ� �Լ� .
    void Update()
    {
        // Positions
        // ===================================================================================================
        // 1 �����Ӵ� z�� 1 ����
        // ���࿡ ��ǻ�� ����� �޶� �ϳ��� 60FPS, �ٸ� �ϳ��� 30FPS
        // >> 1�ʿ� �ϳ��� 60��ŭ �����ϰ�, �ٸ� �ϳ��� 30��ŭ �����ϰԵ� .
        // tr.position += new Vector3(0, 0, 1 * Time.deltaTime);
        // Time.deltaTime ���������Ӱ� ���� ������ ���� �ɸ� �ð�
        // ��, Time.deltaTime �� �����ָ� ��⼺�ɿ� ������� �ʴ� ���� ��ȭ���� ���� �� �ִ�.
        // tr.position += new Vector3(0, 0, 1) * Time.deltaTime;

        // Physics ���� �����͸� ó���� �� ���� ����� .
        // tr.position += new Vector3(0, 0, 1) * Time.fixedDeltaTime;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Debug.Log("h =" + h);
        Debug.Log($"v = {v}");
        // Z axis foward, back
        // X axis left, right
        // Y axis up, down


        // ��ó�� �� ��� ���ÿ� ���� ������ ���������� ���⺤���� ũ�Ⱑ 1�� �Ѿ�� ���⿡���� �ӵ��� �������� ���� ���� �߻� .
        // Vector3 movePos = new Vector3(h, 0,v) * speed * Time.deltaTime;
        // tr.Translate(movePos);


        // Vector : ����� ũ�⸦ ��� ������ ����
        // Ư�� Vectorũ�Ⱑ 1�� ���͸� �������� (Unit Vector)
        // �����̰� ���� ���⿡���� �������� * �ӵ� �� ��ü�� ������
        Vector3 dir = new Vector3(h, 0, v).normalized;
        Vector3 moveVec = dir * speed * Time.deltaTime;
        tr.Translate(moveVec);

        // tr.Translate(moveVec, Space.Self); // local ��ǥ�� ���� �̵�
        tr.Translate(moveVec, Space.World); // World ��ǥ�� ���� �̵�

        // Rotation
        // ================================================================================
        // tr.Rotate(new Vector3(0f, 30f, 0f)); // Y������ 30 radian ��ŭ ȸ���� . Degree 0~360 ���� ��Ÿ���� ���� , Radian 0 ~ 2 pi����

        float r = Input.GetAxis("Mouse X");
        Vector3 rotateVector = Vector3.up * rotateSpeed * r * Time.deltaTime;
        tr.Rotate(rotateVector);
    }
}
