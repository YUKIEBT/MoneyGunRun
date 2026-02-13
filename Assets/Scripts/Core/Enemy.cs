using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth = 3; // ブロックの体力
    [SerializeField] private int moneyReward = 1; // ★追加：倒した時にもらえるお金

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
        // ★追加：GameManagerが存在すれば、お金を加算する！
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddMoney(moneyReward);
        }

        // 破壊！（消滅）
        Destroy(gameObject);
    }
}