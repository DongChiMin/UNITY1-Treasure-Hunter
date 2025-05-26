using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] PlayerMovement player;

    [SerializeField] CinemachineVirtualCamera rightCamera;
    [SerializeField] CinemachineVirtualCamera leftCamera;
    [SerializeField] CinemachineVirtualCamera fallCamera;
    private CinemachineVirtualCamera currentCVCamera;

    [Header("DEBUG")]
    //DEBUG
    [SerializeField] private CinemachineFramingTransposer currentTransposer;
    //transposer
    [SerializeField] private CinemachineFramingTransposer rightTransposer;
    [SerializeField] private CinemachineFramingTransposer leftTransposer;
    [SerializeField] private CinemachineFramingTransposer fallTransposer;

    private Dictionary<CinemachineVirtualCamera, CinemachineFramingTransposer> cameraMap = new Dictionary<CinemachineVirtualCamera, CinemachineFramingTransposer>();

    //Xử lý delay tránh spam đổi hướng nhanh
    private Coroutine directionCoroutine;
    [SerializeField] private int currentFacingDirection = 1;

    private void Start()
    {
        //Phải enable VC thì mới get được, nếu không sẽ bị null á bro

        rightTransposer = rightCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        leftTransposer = leftCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        fallTransposer = fallCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        cameraMap.Add(rightCamera, rightTransposer);
        cameraMap.Add(leftCamera, leftTransposer);
        cameraMap.Add(fallCamera, fallTransposer);

        SetCamera(rightCamera);
    }

    // Update is called once per frame
    void Update()
    {
        //Nếu người chơi đổi hướng liên tục thì không đổi camera, chỉ đổi khi người chơi vẫn giữ đúng hướng hơn 0.2s
        //Chỉ đổi camera sau một khoảng thời gian (ví dụ: 0.5s) nếu hướng không thay đổi trong thời gian đó.

        int inputDirection = player.transform.localScale.x > 0 ? 1 : -1;
        //nếu input khác hướng hiện tại --> chạy code
        if (inputDirection != currentFacingDirection)
        {
            //Trước đó đang chuẩn bị đổi hướng --> hủy
            if(directionCoroutine != null)
            {
                StopCoroutine(directionCoroutine);
            }
            currentFacingDirection = inputDirection;
            directionCoroutine = StartCoroutine(DelayedCameraSwitch(inputDirection));
        }
    }

    IEnumerator DelayedCameraSwitch(int newDirection)
    {
        yield return new WaitForSeconds(0.3f);
        if(player.transform.localScale.x > 0.1f && newDirection == 1)
        {
            SetCamera(rightCamera);
            currentFacingDirection = 1;
        }
        else if (player.transform.localScale.x < -0.1f && newDirection == -1)
        {
            SetCamera(leftCamera);
            currentFacingDirection = -1;
        }

        directionCoroutine = null;
    }

    private void SetCamera(CinemachineVirtualCamera targetCam)
    {
        foreach(var pair in cameraMap)
        {
            if(pair.Key == targetCam)
            {
                targetCam.gameObject.SetActive(true);
                currentCVCamera = targetCam;
                currentTransposer = pair.Value;
            }
            else
            {
                pair.Key.gameObject.SetActive(false);
            }
        }
    }

    public void LookDownCamera(float lookDownDistance)
    {
        //Kiểm tra nếu trước mặt nhân vật không có nền đất --> Hạ camera xuống một chút
        //Sử dụng overlapSphere để check nếu không có nền đất

        Vector2 spherePosition = (Vector2)(player.transform.position + player.transform.localScale.x * Vector3.right * 1.5f + Vector3.down * 2f);
        float radius = 0.1f;
        Collider2D hit = Physics2D.OverlapCircle(spherePosition, radius, LayerMask.GetMask("Ground"));
        //Nếu trước mặt không có mặt đất --> có thể nhìn xuống
        if (hit == null)
        {
            currentTransposer.m_ScreenY = lookDownDistance;
        }
        else
        {
            LookNormalCamera();
        }
    }

    public void LookNormalCamera()
    {
        currentTransposer.m_ScreenY = 0.7f;
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

