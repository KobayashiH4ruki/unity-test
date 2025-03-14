using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 8f; // �i�ދ���
    public float rotationAngle = 90f; // ��]�p�x
    public float moveDuration = 0.5f; // �ړ�����
    public float rotateDuration = 0.5f; // ��]����
    public float groundCheckInterval = 1.0f; // �n�ʃ`�F�b�N�̊Ԋu
    public LayerMask groundLayer; // �n�ʂ̃��C���[

    private bool isMoving = false; // �ړ����t���O
    private Rigidbody rb; // Rigidbody�Q��
    private Transform rotatingPlatform = null; // ����Ă����]��
    private Quaternion lastPlatformRotation; // �Ō�ɋL�^�������̉�]

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // �ŏ��͏d�̓I�t
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
                if (IsOnGroundDetailed()) // 1�}�X���n�ʃ`�F�b�N
                {
                    StartCoroutine(RotateRight());
                }
                else
                {
                    Debug.Log("��]�ł��܂���I 'Ground' �^�O�̏�������܂���B");
                }
            }
        }

        // ���̉�]�����m���A����ɉ����ăv���C���[����]
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

        float stepTime = moveDuration / moveDistance; // 1�}�X�i�ގ���

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
                Debug.Log("�n�ʂ��Ȃ��I �����J�n");
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
            lastPlatformRotation = rotatingPlatform.rotation; // ������u�Ԃ̉�]���L�^
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
