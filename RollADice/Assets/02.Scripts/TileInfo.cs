using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public int index; // ���° ĭ����
    virtual public void TileEvent()
    {
        Debug.Log($"{index} ��° ĭ ����.");
    }
}
