using UnityEngine;
using System.Collections;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 200f; // 回転速度
    private bool isRotating = false; // 回転中かどうか

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
        float rotationTime = 90f / rotationSpeed; // 90度回転にかかる時間

        while (elapsedTime < rotationTime)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationTime);
            yield return null;
        }

        transform.rotation = targetRotation; // 最終角度を確定
        isRotating = false;
    }
}
