using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    private bool canStartAttack;

    private void Start()
    {
        canStartAttack = true;
    }
    public void Attack()
    {
        canStartAttack=false;
        StartCoroutine(Cooldown(attackCooldown));

        //Xử lý attack
        Debug.Log("Cannon Attack");
    }

    IEnumerator Cooldown(float attackCooldown)
    {
        yield return new WaitForSeconds(attackCooldown);
        canStartAttack = true;
    }

    public bool GetCanStartAttack()
    {
        return canStartAttack;
    }

    public void SetCanStartAttack(bool newBool)
    {
        canStartAttack = newBool;
    }
}
