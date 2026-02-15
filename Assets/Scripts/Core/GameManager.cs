using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Waiting, Playing, LevelComplete, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; }
    
    // ★追加：セーブされるデータ（お金、連射レベル、収入レベル）
    public int TotalMoney { get; private set; }
    public int FireRateLevel { get; private set; }
    public int IncomeLevel { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        CurrentState = GameState.Waiting;
        
        // ★起動時にスマホのストレージからデータを読み込む！
        LoadData(); 
    }

    private void Start()
    {
        // UIにセーブされていた所持金を表示する
        if (UIManager.Instance != null) UIManager.Instance.UpdateMoneyText(TotalMoney);
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
    }

    public void AddMoney(int amount)
    {
        // ★追加：IncomeLevel（収入レベル）に応じて獲得金額が倍増！
        int finalAmount = amount * IncomeLevel;
        
        TotalMoney += finalAmount;
        
        // ★お金が増えるたびに自動セーブ！
        SaveData(); 
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMoneyText(TotalMoney);
        }
    }

    // --- 後で使うショップ用の機能 ---
    public bool TryPurchase(int cost)
    {
        if (TotalMoney >= cost)
        {
            TotalMoney -= cost;
            SaveData();
            if (UIManager.Instance != null) UIManager.Instance.UpdateMoneyText(TotalMoney);
            return true;
        }
        return false; // お金が足りない
    }

    public void UpgradeFireRate()
    {
        FireRateLevel++;
        SaveData();
    }

    public void UpgradeIncome()
    {
        IncomeLevel++;
        SaveData();
    }

    // --- セーブ＆ロードの心臓部 (PlayerPrefs) ---
    private void LoadData()
    {
        TotalMoney = PlayerPrefs.GetInt("TotalMoney", 0);           // デフォルトは0円
        FireRateLevel = PlayerPrefs.GetInt("FireRateLevel", 1); // デフォルトはLv.1
        IncomeLevel = PlayerPrefs.GetInt("IncomeLevel", 1);     // デフォルトはLv.1
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("TotalMoney", TotalMoney);
        PlayerPrefs.SetInt("FireRateLevel", FireRateLevel);
        PlayerPrefs.SetInt("IncomeLevel", IncomeLevel);
        PlayerPrefs.Save(); // スマホのストレージに書き込む（超重要）
    }

    // --- 以下は元のまま ---
    public void LevelComplete()
    {
        CurrentState = GameState.LevelComplete;
        if (UIManager.Instance != null) UIManager.Instance.ShowLevelComplete();
    }

    public void GameOver()
    {
        CurrentState = GameState.GameOver;
        if (UIManager.Instance != null) UIManager.Instance.ShowGameOver();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}