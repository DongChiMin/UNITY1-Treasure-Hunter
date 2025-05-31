using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private float speed;
    
    private void Start()
    {
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if(transform.position.x - target.position.x < 0.1f)
        {
            transform.position = new Vector3(transform.position.x + 30, Random.Range(1f, 8f), 0);
        }
    }
}
