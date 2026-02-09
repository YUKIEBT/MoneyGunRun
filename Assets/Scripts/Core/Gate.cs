using UnityEngine;
using TMPro; // 文字表示に必要

public enum GateType { FireRate, Range } // 今後種類を増やせるように

public class Gate : MonoBehaviour
{
    [Header("Gate Settings")]
    public GateType gateType;
    public float value = 0.1f; // 加算する値（例: -0.05f なら速くなる）
    [SerializeField] private TextMeshPro gateText;
    [SerializeField] private MeshRenderer gateRenderer;

    private void Start()
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        // 値に応じて色を変える（プラスなら青、マイナスなら赤）
        // FireRateは「値が小さいほど速い」ので注意が必要ですが、
        // ここではシンプルに「Valueがマイナス（短縮）なら青」とします
        bool isPositive = value < 0;

        if (gateRenderer != null)
        {
            // 緑(良)か赤(悪)か
            gateRenderer.material.color = isPositive ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
        }

        if (gateText != null)
        {
            // 表示テキスト更新
            string prefix = value > 0 ? "+" : "";
            gateText.text = $"{gateType}\n{prefix}{value:F2}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが通過したら
        if (other.CompareTag("Player"))
        {
            PlayerShooter shooter = other.GetComponent<PlayerShooter>();
            if (shooter != null)
            {
                shooter.ApplyUpgrade(value);
            }

            // 通過したら消す（または無効化）
            Destroy(gameObject);
        }
    }
}