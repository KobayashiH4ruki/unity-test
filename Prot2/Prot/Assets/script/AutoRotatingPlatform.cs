using UnityEngine;
using System.Collections;

public class AutoRotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 90f; // ��]���x (�x/�b)
    public Vector3 rotationAxis = Vector3.up; // ��]�� (�f�t�H���g��Y��)
    public float waitTime = 2f; // ��~���鎞��

    private bool isRotating = true;

    void Start()
    {
        StartCoroutine(RotatePlatform());
    }

    IEnumerator RotatePlatform()
    {
        while (true)
        {
            float rotatedAngle = 0f;

            // 90�x��]����
            while (rotatedAngle < 90f)
            {
                float step = rotationSpeed * Time.deltaTime;
                transform.Rotate(rotationAxis * step);
                rotatedAngle += step;
                yield return null;
            }

            // �p�x�𒲐����ăs�b�^��90�x�ɂ���
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.y = Mathf.Round(currentRotation.y / 90f) * 90f; // Y���̏ꍇ
            transform.eulerAngles = currentRotation;

            // 2�b�Ԓ�~
            yield return new WaitForSeconds(waitTime);
        }
    }
}
