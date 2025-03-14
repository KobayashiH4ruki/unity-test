using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f; // 弾の寿命（秒）

    void Start()
    {
        Destroy(gameObject, lifetime); // 一定時間後に消滅
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject); // プレイヤーを消す
            Destroy(gameObject); // 弾も消す
        }
        else if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Destroy(gameObject); // 地面や壁に当たったら消える
        }
    }
}
