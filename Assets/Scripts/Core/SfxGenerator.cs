using UnityEngine;

public class SfxGenerator : MonoBehaviour
{
    // どこからでもアクセスできる「Instance（自分自身）」
    public static SfxGenerator Instance { get; private set; }

    // ★重要：[SerializeField]をつけることでUnityの画面に枠が出現します！
    [SerializeField] private AudioClip shootClip;

    // Playerなどが「ShootClip」という名前で音を読み取れるようにする設定
    public AudioClip ShootClip => shootClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}