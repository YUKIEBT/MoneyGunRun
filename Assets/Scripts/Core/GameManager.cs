using UnityEngine;
using UnityEngine.SceneManagement; // リスタートに必要

public enum GameState { Waiting, Playing, LevelComplete, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; }
    public int Money { get; private set; } // 現在の所持金

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        CurrentState = GameState.Waiting;
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
        // UIに通知（後で実装）
        Debug.Log("Game Started!");
    }

    public void AddMoney(int amount)
    {
        Money += amount;
        // UI更新（後で実装）
        Debug.Log($"Money: {Money}");
        
        // UIマネージャーがいれば更新を依頼する
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMoneyText(Money);
        }
    }

    public void LevelComplete()
    {
        CurrentState = GameState.LevelComplete;
        Debug.Log("Level Complete! YOU WIN!");
        // ここでリザルト画面を出す
        if (UIManager.Instance != null) UIManager.Instance.ShowLevelComplete();
    }

    public void GameOver()
    {
        CurrentState = GameState.GameOver;
        Debug.Log("Game Over...");
        // ここでリトライ画面を出す
        if (UIManager.Instance != null) UIManager.Instance.ShowGameOver();
    }

    public void RestartLevel()
    {
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}