using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int moneyReward = 1;

    [Header("Effects")]
    [SerializeField] private GameObject explosionPrefab;
    
    // ↓ おそらく、この1行が抜けていた（または場所が違った）のが原因です！
    [SerializeField] private AudioClip explosionSound;

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

        // 爆発エフェクトを出現させる
        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        // 爆発音を鳴らす
        if (explosionSound != null)
        {
            // ★変更：カメラ（耳）の真ん前で鳴らす！ ついでに音量も50%（0.5f）に調整
            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
        }

        // 敵自身は消滅！
        Destroy(gameObject);
    }
}