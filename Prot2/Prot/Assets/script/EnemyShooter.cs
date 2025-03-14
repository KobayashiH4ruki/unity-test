using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab; // 発射する弾のプレハブ
    public Transform firePoint; // 弾の発射位置
    public float fireRate = 2f; // 発射間隔（秒）
    public float bulletSpeed = 10f; // 弾の速度
    public Vector3 fireDirection = Vector3.forward; // 固定の発射方向（デフォルトは前方）

    void Start()
    {
        StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            Fire();
        }
    }

    void Fire()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = firePoint.TransformDirection(fireDirection.normalized * bulletSpeed);
        }
    }
}
