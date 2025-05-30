using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAnimationController : MonoBehaviour
{
    public Animator animator;
    private string currentAnim;

    private void Start()
    {
        ChangeAnim("Idle");
    }

    public void ChangeAnim(string newAnim)
    {
        if (currentAnim == newAnim)
        {
            return;
        }
        animator.Play(newAnim);
        currentAnim = newAnim;
    }
}
