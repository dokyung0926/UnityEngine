using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoarder : MonoBehaviour
{
    private void OnCollisonEnter(Collision collider)
    {

        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(collider.gameObject);
        }
    }
}
