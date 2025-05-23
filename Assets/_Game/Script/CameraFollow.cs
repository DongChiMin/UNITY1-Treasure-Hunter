using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Vector3 offset;
    [SerializeField] GameObject target;
    [SerializeField] float moveSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LerpCamera(offset);
    }

    void LerpCamera(Vector3 offset)
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, moveSpeed * Time.deltaTime);
    }
}
