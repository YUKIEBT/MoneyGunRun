using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int moneyReward = 1;

    // ★追加：爆発エフェクトを入れる箱
    [Header("Effects")]
    [SerializeField] private GameObject explosionPrefab;

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

        // 撃たれるたびに少し震える・縮む
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
        // お金を加算する
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddMoney(moneyReward);
        }

        // ★追加：爆発エフェクトを出現させる！
        if (explosionPrefab != null)
        {
            // 敵のいた場所にエフェクトをスポーンさせる
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            
            // 2秒後にエフェクトをゴミ箱へ捨てる（スマホが重くならないための必須テクニック！）
            Destroy(fx, 2f);
        }

        // 敵自身は消滅！
        Destroy(gameObject);
    }
}