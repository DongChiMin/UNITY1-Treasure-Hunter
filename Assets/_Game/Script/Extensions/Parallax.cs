using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private Vector3 startPos;
    [SerializeField] private CameraFollow cam;
    [SerializeField] private float parallaxEffect;

    private void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float distX = (cam.transform.position.x - cam.offset.x)* parallaxEffect;
        transform.position = new Vector3 (startPos.x + distX, transform.position.y, transform.position.z);
    }
}
