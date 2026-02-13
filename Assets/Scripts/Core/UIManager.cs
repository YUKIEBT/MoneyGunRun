using UnityEngine;
using TMPro; // 文字表示に必須
using UnityEngine.UI; // ボタンなどに必要

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI moneyText;

    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // 初期状態のUIセットアップ
        UpdateMoneyText(0);
        ShowPanel(startPanel);
    }

    public void UpdateMoneyText(int amount)
    {
        if (moneyText != null)
        {
            moneyText.text = $"$ {amount}";
        }
    }

    // ゲーム開始ボタンから呼ばれる
    public void OnTapToStart()
    {
        GameManager.Instance.StartGame();
        startPanel.SetActive(false); // スタート画面を隠す
    }

    // リトライ/ネクストボタンから呼ばれる
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
        // 全パネルを一旦隠す
        startPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // 指定されたものだけ出す
        if (panelToShow != null) panelToShow.SetActive(true);
    }
}