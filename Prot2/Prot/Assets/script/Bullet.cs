using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f; // �e�̎����i�b�j

    void Start()
    {
        Destroy(gameObject, lifetime); // ��莞�Ԍ�ɏ���
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject); // �v���C���[������
            Destroy(gameObject); // �e������
        }
        else if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Destroy(gameObject); // �n�ʂ�ǂɓ��������������
        }
    }
}
