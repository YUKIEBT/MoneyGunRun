using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerShooter : MonoBehaviour
{
    [Header("Shooting Stats")]
    [SerializeField] private float fireRate = 0.6f;
    [SerializeField] private float minFireRate = 0.02f;
    [SerializeField] private Transform firePoint;

    private AudioSource _audioSource;
    private float _timer;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0.5f; 
        _audioSource.pitch = 1.0f;

        // ★追加：GameManagerからレベルを読み取り、初期スピードを上げる
        if (GameManager.Instance != null)
        {
            // レベル1ならボーナス0。レベルが上がるごとに0.05秒速くなる
            float upgradeBonus = (GameManager.Instance.FireRateLevel - 1) * 0.05f;
            fireRate -= upgradeBonus;
            
            // 限界値より速くならないように制限
            if (fireRate < minFireRate) fireRate = minFireRate;
        }
    }

    private void Update()
    {
        // ★修正：ゲーム中以外（スタート前やクリア後）は撃たないようにする
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing) return;

        _timer += Time.deltaTime;

        if (_timer >= fireRate)
        {
            Shoot();
            _timer = 0f;
        }
    }

    private void Shoot()
    {
        if (ObjectPool.Instance == null) return;

        Vector3 spawnPos = firePoint ? firePoint.position : transform.position;
        ObjectPool.Instance.GetBullet(spawnPos, transform.rotation);

        PlayShootSound();
    }

    private void PlayShootSound()
    {
        if (SfxGenerator.Instance != null && SfxGenerator.Instance.ShootClip != null)
        {
            _audioSource.PlayOneShot(SfxGenerator.Instance.ShootClip);
        }
    }

    public void ApplyUpgrade(float amount)
    {
        fireRate += amount;
        if (fireRate < minFireRate) fireRate = minFireRate;
    }
}