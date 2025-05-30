using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool detected;

    private GameObject target;
    void Update()
    {
        Collider2D playerCol = Physics2D.OverlapBox(transform.position, new Vector2(10, 3), 0f, LayerMask.GetMask("Player"));
        if(playerCol != null)
        {
            detected = true;
            target = playerCol.gameObject;
        }
        else
        {
            detected = false;
            target = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(10,3,0.1f));
    }

    public bool GetDetected()
    {
        return detected;
    }

    public GameObject GetTarget()
    {
        return target;
    }
}
