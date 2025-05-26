using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float horizontal;
    public bool jumpKeyPressed;
    public bool attackKeyPressed;
    public bool throwKeyPressed;
    public bool dashKeyPressed;
    public bool lookDownKeyPressed;
    
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        jumpKeyPressed = Input.GetKeyDown(KeyCode.Space);
        attackKeyPressed = Input.GetMouseButtonDown(0);
        throwKeyPressed = Input.GetMouseButtonDown(1);
        dashKeyPressed = Input.GetKeyDown(KeyCode.LeftShift);
        lookDownKeyPressed = Input.GetKey(KeyCode.S);
    }
}
