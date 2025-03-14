using UnityEngine;
using System.Collections;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 200f; // ��]���x
    private bool isRotating = false; // ��]�����ǂ���

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isRotating)
        {
            StartCoroutine(Rotate90Degrees());
        }
    }

    IEnumerator Rotate90Degrees()
    {
        isRotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 90f, 0);
        float elapsedTime = 0f;
        float rotationTime = 90f / rotationSpeed; // 90�x��]�ɂ����鎞��

        while (elapsedTime < rotationTime)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationTime);
            yield return null;
        }

        transform.rotation = targetRotation; // �ŏI�p�x���m��
        isRotating = false;
    }
}
