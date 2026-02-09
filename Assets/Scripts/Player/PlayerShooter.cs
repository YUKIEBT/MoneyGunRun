using UnityEngine;

[RequireComponent(typeof(AudioSource))] // 自動でAudioSourceを追加するおまじない
public class PlayerShooter : MonoBehaviour
{
    [Header("Shooting Stats")]
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float minFireRate = 0.05f;
    [SerializeField] private Transform firePoint;

    // 音を鳴らすためのコンポーネント
    private AudioSource _audioSource;
    private float _timer;

    private void Start()
    {
        // 自分のAudioSourceを取得
        _audioSource = GetComponent<AudioSource>();

        // 音割れ防止のために少し音量を下げる
        _audioSource.volume = 0.5f;

        // 重ならないようにピッチ（音程）を少しランダムにする設定
        _audioSource.pitch = 1.0f;
    }

    private void Update()
    {
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

        // ★ここで音を鳴らす
        PlayShootSound();
    }

    private void PlayShootSound()
    {
        // SfxGeneratorが存在し、AudioClipが生成されていれば鳴らす
        if (SfxGenerator.Instance != null && SfxGenerator.Instance.ShootClip != null)
        {
            // PlayOneShotは「前の音が終わるのを待たずに」重ねて再生できる（連射に必須）
            _audioSource.PlayOneShot(SfxGenerator.Instance.ShootClip);
        }
    }

    public void ApplyUpgrade(float amount)
    {
        fireRate += amount;
        if (fireRate < minFireRate) fireRate = minFireRate;
    }
}