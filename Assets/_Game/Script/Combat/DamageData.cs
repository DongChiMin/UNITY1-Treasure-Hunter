using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageData
{
    public float damageAmount;
    public bool knockbackDirectionRight; //=1 --> Knockback sang bên phải 
    public float knockbackForce;
    public GameObject source; // Nguồn sát thương đến từ source
}
