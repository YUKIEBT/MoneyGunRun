using UnityEngine;

// 責務: キャラクターを走らせる、左右に動かす
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 10f;  // 前進スピード
    [SerializeField] private float swerveSpeed = 30f;   // 左右移動の敏感さ
    [SerializeField] private float maxX = 4.5f;         // 道路の右端制限
    [SerializeField] private float minX = -4.5f;        // 道路の左端制限

    private void Update()
    {
        MoveForward();
        HandleSwerve();
    }

    private void MoveForward()
    {
        // 常にZ軸プラス方向（奥）へ進む
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    private void HandleSwerve()
    {
        // InputManagerがない場合は何もしない（エラー防止）
        if (InputManager.Instance == null) return;

        // 入力値を取得
        float swerveAmount = InputManager.Instance.GetSwerveAmount();

        // 現在の位置を取得
        Vector3 currentPos = transform.position;

        // 新しいX座標を計算 (現在のX + 移動量)
        float newX = currentPos.x + (swerveAmount * swerveSpeed);

        // 道路からはみ出さないようにClamp（値を制限）
        newX = Mathf.Clamp(newX, minX, maxX);

        // 位置を更新 (YとZはそのまま、Xだけ変える)
        transform.position = new Vector3(newX, currentPos.y, currentPos.z);
    }
}