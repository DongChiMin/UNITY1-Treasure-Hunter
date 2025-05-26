using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] private float parallaxEffect;
    [SerializeField] private Vector3 startPos;
    //private float startPos;
    
    void Start()
    {
        //startPos = transform.position.x;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos.x + distance, transform.position.y, transform.position.z);
    }
}
