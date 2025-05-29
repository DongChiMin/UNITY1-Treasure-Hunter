using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class PlayerItemPickup : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool haveSword;
    [SerializeField] private PlayerContext playerContext;

    private void Start()
    {
        playerContext = GetComponent<PlayerContext>();
        haveSword = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Sword Destroy" && !haveSword)
        {
            //Chuyển sang hình ảnh cầm kiếm
            playerContext.playerMovement.animator.SetLayerWeight(1, 1);
            haveSword = true;
            playerContext.playerMovement.ResetSwordAnim();

            //Xử lý item kiếm
            Destroy(collision.gameObject);

        }
        else if (collision.tag == "Sword No Destroy" & !haveSword)
        {
            //Chuyển sang hình ảnh cầm kiếm
            playerContext.playerMovement.animator.SetLayerWeight(1, 1);
            haveSword = true;
            playerContext.playerMovement.ResetSwordAnim();
        }
    }

    public bool GetHaveSword()
    {
        return haveSword;
    }

    public void SetHaveSword(bool newBool)
    {
        haveSword = newBool;
    }
}
