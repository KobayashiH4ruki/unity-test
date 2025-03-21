using UnityEngine;
using System.Collections;

public class AutoRotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 90f; // 回転速度 (度/秒)
    public Vector3 rotationAxis = Vector3.up; // 回転軸 (デフォルトはY軸)
    public float waitTime = 2f; // 停止する時間

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

            // 90度回転する
            while (rotatedAngle < 90f)
            {
                float step = rotationSpeed * Time.deltaTime;
                transform.Rotate(rotationAxis * step);
                rotatedAngle += step;
                yield return null;
            }

            // 角度を調整してピッタリ90度にする
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.y = Mathf.Round(currentRotation.y / 90f) * 90f; // Y軸の場合
            transform.eulerAngles = currentRotation;

            // 2秒間停止
            yield return new WaitForSeconds(waitTime);
        }
    }
}
