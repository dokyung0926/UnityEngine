using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoarder : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        Destroy(collision.gameObject);
    }
}
