using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab; // ���˂���e�̃v���n�u
    public Transform firePoint; // �e�̔��ˈʒu
    public float fireRate = 2f; // ���ˊԊu�i�b�j
    public float bulletSpeed = 10f; // �e�̑��x
    public Vector3 fireDirection = Vector3.forward; // �Œ�̔��˕����i�f�t�H���g�͑O���j

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
