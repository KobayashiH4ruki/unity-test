using UnityEngine;
using System.Collections;

public class AutoRotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 90f; // ‰ñ“]‘¬“x (“x/•b)
    public Vector3 rotationAxis = Vector3.up; // ‰ñ“]Ž² (ƒfƒtƒHƒ‹ƒg‚ÍYŽ²)
    public float waitTime = 2f; // ’âŽ~‚·‚éŽžŠÔ

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

            // 90“x‰ñ“]‚·‚é
            while (rotatedAngle < 90f)
            {
                float step = rotationSpeed * Time.deltaTime;
                transform.Rotate(rotationAxis * step);
                rotatedAngle += step;
                yield return null;
            }

            // Šp“x‚ð’²®‚µ‚Äƒsƒbƒ^ƒŠ90“x‚É‚·‚é
            Vector3 currentRotation = transform.eulerAngles;
            currentRotation.y = Mathf.Round(currentRotation.y / 90f) * 90f; // YŽ²‚Ìê‡
            transform.eulerAngles = currentRotation;

            // 2•bŠÔ’âŽ~
            yield return new WaitForSeconds(waitTime);
        }
    }
}
