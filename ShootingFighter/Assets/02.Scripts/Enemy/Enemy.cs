using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float score;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private GameObject destroyEffect;
    Transform tr;
    Vector3 dir;
    Vector3 deltaMove;
    [SerializeField] private int AIPercent;


    private void Awake()
    {
        tr =gameObject.GetComponent<Transform>();    
    }


    private void Start()
    {
        SetTarget_RandomlyToPlayer(AIPercent);
    }


    private void Update()
    {
        Move();
    }


    public void Move()
    {
        deltaMove = dir * speed * Time.deltaTime;
        tr.Translate(deltaMove, Space.World);
    }


    private void SetTarget_RandomlyToPlayer(int percent)
    {
        int tmpRandomValue = Random.Range(0, 100);
        if (percent > tmpRandomValue)
        {
            GameObject target = GameObject.Find("Player");
            if (target == null)
            {
                dir = Vector3.back;
            }
            else
            {
                dir = (target.transform.position - tr.position).normalized;
            }
        }
        else
        {
            dir = Vector3.back;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 플레이어의 체력 감소
            Player player =collision.gameObject.GetComponent<Player>();
            player.HP -= damage;

            // 파괴이펙트
            GameObject effectGO = Instantiate(destroyEffect);
            effectGO.transform.position = tr.position;
            Destroy(this.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerWeapon"))
        {
            // todo => 파괴 이펙트
            GameObject effectGO = Instantiate(destroyEffect);
            effectGO.transform.position = tr.position;
            
            // Enemy가 총알에 의해 파괴 될 때 점수 증가
            GameObject playerGO = GameObject.Find("Player");
            playerGO.GetComponent<Player>().score += score;

            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
