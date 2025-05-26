using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] CinemachineVirtualCamera rightCamera;
    [SerializeField] CinemachineVirtualCamera leftCamera;
    [SerializeField] CinemachineVirtualCamera fallCamera;

    private CinemachineVirtualCamera currentCVCamera;
    private CinemachineFramingTransposer currentTransposer;

    //transposer
    private CinemachineFramingTransposer rightTransposer;
    private CinemachineFramingTransposer leftTransposer;
    private CinemachineFramingTransposer fallTransposer;

    private void Start()
    {
        rightTransposer = rightCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        leftTransposer = leftCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        fallTransposer = fallCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.localScale.x == -1)
        {
      
            rightCamera.gameObject.SetActive(false);
            leftCamera.gameObject.SetActive(true);
            fallCamera.gameObject.SetActive(false);

            currentCVCamera = leftCamera;
            currentTransposer = leftTransposer;
        }
        else
        {
            leftCamera.gameObject.SetActive(false);
            rightCamera.gameObject.SetActive(true);
            fallCamera.gameObject.SetActive(false);

            currentCVCamera = rightCamera;
            currentTransposer = rightTransposer;
        }
        //Kiểm tra nếu trước mặt nhân vật không có nền đất --> Hạ camera xuống một chút
        //Sử dụng overlapSphere để check nếu không có nền đất

        Vector2 spherePosition = (Vector2)(player.transform.position + player.transform.localScale.x * Vector3.right * 1.5f + Vector3.down * 2f);
        float radius = 0.1f;
        Collider2D hit = Physics2D.OverlapCircle(spherePosition, radius, LayerMask.GetMask("Ground"));
        //Nếu trước mặt không có mặt đất --> có thể nhìn xuống
        if (hit == null)
        {
            if (player.isLookDown)
            {
                currentTransposer.m_ScreenY = player.lookDownDistance;
            }
            else
            {
                currentTransposer.m_ScreenY = 0.7f;
            }
        }
        else
        {
            currentTransposer.m_ScreenY = 0.7f;
        }
    }

    void OnDrawGizmos()
    {
        // Tính toán vị trí điểm bắt đầu cho OverlapSphere (giống như Raycast).
        Vector2 spherePosition = (Vector2)(player.transform.position + player.transform.localScale.x * Vector3.right * 1.5f + Vector3.down*2f);

        // Kích thước của sphere (bán kính)
        float radius = 0.1f;

        // Vẽ Gizmos (vẽ một vòng tròn trong Scene view)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePosition, 0.1f);

        // Dùng OverlapCircle để kiểm tra các collider trong phạm vi
        Collider2D hit = Physics2D.OverlapCircle(spherePosition, radius, LayerMask.GetMask("Ground"));
    
        // Hiển thị tên collider nếu có va chạm
        //if(hit != null)
        //{
        //    Debug.Log("Hit: " + hit.name);
        //}
  
    }
} 

