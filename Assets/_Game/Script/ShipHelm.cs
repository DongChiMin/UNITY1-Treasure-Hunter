using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHelm : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("Turn", true);
            animator.SetBool("Idle", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("Turn", false);
            animator.SetBool("Idle", true);

        }
    }
}
