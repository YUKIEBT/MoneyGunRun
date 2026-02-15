using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;

    // ★追加：ショップUI用のパーツ
    [Header("Shop UI")]
    [SerializeField] private TextMeshProUGUI fireRateLevelText;
    [SerializeField] private TextMeshProUGUI fireRateCostText;
    [SerializeField] private TextMeshProUGUI incomeLevelText;
    [SerializeField] private TextMeshProUGUI incomeCostText;

    private int baseFireRateCost = 10; // 連射強化の基本価格
    private int baseIncomeCost = 10;   // 収入強化の基本価格

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        ShowPanel(startPanel);
        UpdateShopUI(); // ★追加：スタート画面が出たらショップの値段を計算して表示
    }

    public void UpdateMoneyText(int amount)
    {
        if (moneyText != null) moneyText.text = $"$ {amount}";
    }

    public void OnTapToStart()
    {
        GameManager.Instance.StartGame();
        ShowPanel(null); // 全部隠す
    }

    public void OnRestartButton()
    {
        GameManager.Instance.RestartLevel();
    }

    public void ShowLevelComplete()
    {
        ShowPanel(levelCompletePanel);
    }

    public void ShowGameOver()
    {
        ShowPanel(gameOverPanel);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        if (startPanel != null) startPanel.SetActive(false);
        if (levelCompletePanel != null) levelCompletePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        if (panelToShow != null) panelToShow.SetActive(true);
    }

    // ==========================================
    // ★追加：ここから下がショップ（お買い物）の処理
    // ==========================================
    public void UpdateShopUI()
    {
        if (GameManager.Instance == null) return;

        int fireRateLv = GameManager.Instance.FireRateLevel;
        int incomeLv = GameManager.Instance.IncomeLevel;

        // レベルと値段のテキストを更新（例: レベル2なら値段は20ドル）
        if (fireRateLevelText != null) fireRateLevelText.text = $"Fire Rate Lv.{fireRateLv}";
        if (fireRateCostText != null) fireRateCostText.text = $"$ {fireRateLv * baseFireRateCost}";

        if (incomeLevelText != null) incomeLevelText.text = $"Income Lv.{incomeLv}";
        if (incomeCostText != null) incomeCostText.text = $"$ {incomeLv * baseIncomeCost}";
    }

    public void OnBuyFireRate()
    {
        int cost = GameManager.Instance.FireRateLevel * baseFireRateCost;
        if (GameManager.Instance.TryPurchase(cost)) // お金が足りるかチェック！
        {
            GameManager.Instance.UpgradeFireRate(); // レベルアップ！
            UpdateShopUI(); // 表示を更新
        }
    }

    public void OnBuyIncome()
    {
        int cost = GameManager.Instance.IncomeLevel * baseIncomeCost;
        if (GameManager.Instance.TryPurchase(cost))
        {
            GameManager.Instance.UpgradeIncome();
            UpdateShopUI();
        }
    }
}