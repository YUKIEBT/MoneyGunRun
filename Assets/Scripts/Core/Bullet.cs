using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1; // 弾の威力

    private void OnEnable()
    {
        Invoke(nameof(Deactivate), lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // ★追加: 何かに当たった時の処理
    private void OnTriggerEnter(Collider other)
    {
        // 当たった相手が "Enemy" コンポーネントを持っているか確認
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            // ダメージを与える
            enemy.TakeDamage(damage);

            // 弾は消える（プールに戻る）
            Deactivate();
        }
        // もしゲートなら何もしない（ゲートは通り抜ける）
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}