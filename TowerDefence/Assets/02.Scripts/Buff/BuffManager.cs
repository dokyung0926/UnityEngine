using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    private void Awake()
    {
        instance = this;
    }

    public static IEnumerator ActiveBuff(Enemy enemy, Buff buff)
    {
        buff.OnActive(enemy);

        bool doBuff = true;
        float timeMark = Time.time;
        while (doBuff &&
               Time.time - timeMark < buff.duration && // ���ӽð� - �����ߵ��ð� < �������ӽð�
               enemy != null)
        {
            doBuff = buff.OnDuration(enemy);
            yield return null; // �ش� �ݺ����� �����Ӵ� �ѹ� ����ǰ� �ϱ�����.
        }

        if(enemy != null)
            buff.OnDeactive(enemy);        
    }
}
