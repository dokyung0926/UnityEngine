using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float _HP; 
    public float HP
    {
        set
        {
            _HP = value;
            int HPint = (int)value;
            HPSlider.value = _HP / HPMax;
            if(_HP <= 0)
            {
                DestroyByPlayerWeapon();
            }
        }
        get
        {
            return _HP;
        }
    }
    [SerializeField] private float HPInit;
    [SerializeField] private float HPMax;
    [SerializeField] private Slider HPSlider;



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
        HP = HPInit;
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
            Player player =collision.gameObject.GetComponent<Player>();
            player.HP -= damage;

            GameObject effectGO = Instantiate(destroyEffect);
            effectGO.transform.position = tr.position;
            Destroy(this.gameObject);
        }
    }

    public void DestroyByPlayerWeapon()
    {
        GameObject effectGO = Instantiate(destroyEffect);
        effectGO.transform.position = tr.position;

        GameObject playerGO = GameObject.Find("Player");
        playerGO.GetComponent<Player>().score += score;

        Destroy(this.gameObject);
    }
}
