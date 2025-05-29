using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private List<AttackBox> attackBox = new List<AttackBox>();

    public void SetAttackBox(bool boolValue, AttackType attackType)
    {
        foreach (var box in attackBox)
        {
            if (box.GetAttackType() == attackType)
            {
                box.gameObject.SetActive(boolValue);
                return;
            }
        }
        Debug.LogWarning("Không tồn tại attackType thỏa mãn trong list attackBox");
    }
}
