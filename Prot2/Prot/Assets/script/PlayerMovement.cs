using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 8f; // 進む距離
    public float rotationAngle = 90f; // 回転角度
    public float moveDuration = 0.5f; // 移動時間
    public float rotateDuration = 0.5f; // 回転時間
    public float groundCheckInterval = 1.0f; // 地面チェックの間隔
    public LayerMask groundLayer; // 地面のレイヤー

    private bool isMoving = false; // 移動中フラグ
    private Rigidbody rb; // Rigidbody参照
    private Transform rotatingPlatform = null; // 乗っている回転床
    private Quaternion lastPlatformRotation; // 最後に記録した床の回転

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // 最初は重力オフ
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(MoveForward());
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (IsOnGroundDetailed()) // 1マスずつ地面チェック
                {
                    StartCoroutine(RotateRight());
                }
                else
                {
                    Debug.Log("回転できません！ 'Ground' タグの床がありません。");
                }
            }
        }

        // 床の回転を検知し、それに応じてプレイヤーも回転
        if (rotatingPlatform != null)
        {
            Quaternion platformRotationDelta = rotatingPlatform.rotation * Quaternion.Inverse(lastPlatformRotation);
            transform.rotation = platformRotationDelta * transform.rotation;
            lastPlatformRotation = rotatingPlatform.rotation;
        }
    }

    IEnumerator MoveForward()
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + transform.forward * moveDistance;
        Vector3 currentPosition = startPosition;

        float stepTime = moveDuration / moveDistance; // 1マス進む時間

        while (currentPosition != targetPosition)
        {
            Vector3 nextPosition = currentPosition + transform.forward * groundCheckInterval;
            float stepElapsedTime = 0;

            while (stepElapsedTime < stepTime)
            {
                transform.position = Vector3.Lerp(currentPosition, nextPosition, stepElapsedTime / stepTime);
                stepElapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = nextPosition;
            currentPosition = nextPosition;

            if (!IsGroundBelow())
            {
                Debug.Log("地面がない！ 落下開始");
                EnableGravity();
                yield break;
            }
        }

        transform.position = targetPosition;
        isMoving = false;
    }

    IEnumerator RotateRight()
    {
        isMoving = true;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, rotationAngle, 0);

        float elapsedTime = 0;
        while (elapsedTime < rotateDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotateDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        isMoving = false;
    }

    bool IsGroundBelow()
    {
        Vector3 checkPosition = transform.position;
        return Physics.Raycast(checkPosition, Vector3.down, 1.1f, groundLayer);
    }

    bool IsOnGroundDetailed()
    {
        float checkRadius = 0.5f;
        Vector3[] checkOffsets = {
            Vector3.zero,
            Vector3.forward * checkRadius,
            Vector3.back * checkRadius,
            Vector3.left * checkRadius,
            Vector3.right * checkRadius
        };

        foreach (var offset in checkOffsets)
        {
            Vector3 checkPosition = transform.position + offset;
            RaycastHit hit;
            if (Physics.Raycast(checkPosition, Vector3.down, out hit, 1.1f))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void EnableGravity()
    {
        isMoving = false;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RotatingPlatform"))
        {
            rotatingPlatform = other.transform;
            lastPlatformRotation = rotatingPlatform.rotation; // 乗った瞬間の回転を記録
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RotatingPlatform"))
        {
            rotatingPlatform = null;
        }
    }
}
