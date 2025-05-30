using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FragmentsEffect : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] float piecesForce;     //Lực đẩy cho các mảnh vỡ
    [SerializeField] float mainHideDelay;    //Thời gian trước khi ẩn hình ảnh chính
    [SerializeField] float fragmentsOnGroundDuration;    //Thời gian mảnh vỡ nằm trên mặt đất 
    [SerializeField] float fragmentsFallingDuration;    //Thời gian các mảnh vỡ rơi tự do

    [Header("Drag")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D coll;
    [SerializeField] private GameObject mainSprite;
    [SerializeField] private Rigidbody2D[] cratePieces;

    [Header("Events")]
    public UnityEvent OnDestroy;

    //Lưu các giá trị mặc định ban đầu để reset mỗi lần được gọi enable lại 
    private float previousGravityScale;
    private Vector2 previousVelocity;
    private void Awake()
    {
        previousGravityScale = rb.gravityScale;
        previousVelocity = rb.velocity;
    }

    private void OnEnable()
    {
        coll.enabled = true;
        rb.velocity = previousVelocity;
        rb.gravityScale = previousGravityScale;
        mainSprite.SetActive(true);
        foreach(var piece in cratePieces)
        {
            piece.gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }
    public void Explode()
    {
        //Đếm ngược thời gian hủy gameObject
        StartCoroutine(DestroyCrate());
        //reset vị trí, enable các mảnh vỡ và tạo lực đẩy ngẫu nhiên cho từng mảnh
        foreach (Rigidbody2D rb in cratePieces)
        {
            rb.gameObject.transform.position = transform.position;
            rb.gameObject.SetActive(true);
            Vector2 direction = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 4f));
            rb.AddForce(direction * piecesForce, ForceMode2D.Impulse);
        }
    }

    IEnumerator DestroyCrate()
    {
        //Đợi một khoảng ngắn sau đó ẩn hình ảnh chính
        yield return new WaitForSeconds(mainHideDelay);
        coll.enabled = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        mainSprite.SetActive(false);

        //Các mảnh nằm trên mặt đất một khoảng thời gian, sau đó rơi tự do
        yield return new WaitForSeconds(fragmentsOnGroundDuration);
        StartCoroutine(FragmentsFalling());
    }

    IEnumerator FragmentsFalling()
    {
        for (int i = 0; i < cratePieces.Length; i++)
        {
            cratePieces[i].gameObject.GetComponent<Collider2D>().enabled = false;
        }
        coll.enabled = false;
        //sau khi rơi tự do 0.5s
        yield return new WaitForSeconds(fragmentsFallingDuration);
        OnDestroy?.Invoke();
    }
}
