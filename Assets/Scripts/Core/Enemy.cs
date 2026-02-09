using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth = 10; // ブロックの体力
    [SerializeField] private GameObject dieEffect; // 死んだ時のエフェクト（後で追加）

    private int _currentHealth;
    private Vector3 _initialScale;

    private void OnEnable()
    {
        _currentHealth = maxHealth;
        _initialScale = transform.localScale;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        // 【演出】撃たれるたびに少し震える・縮む（ヒット感の演出）
        transform.localScale = _initialScale * 0.9f;
        Invoke(nameof(ResetScale), 0.05f);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void ResetScale()
    {
        transform.localScale = _initialScale;
    }

    private void Die()
    {
        // ここでお金（スコア）を加算する処理を後で入れます
        Debug.Log("Money +10!");

        // 破壊！（消滅）
        Destroy(gameObject);
    }
}