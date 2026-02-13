using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 5f;  // 前進スピード
    [SerializeField] private float swerveSpeed = 30f;  // 左右移動の敏感さ
    [SerializeField] private float maxX = 4.5f;
    [SerializeField] private float minX = -4.5f;

    private void Update()
    {
        // ゲーム中のみ動く（待機中やクリア後は止まる）
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        MoveForward();
        HandleSwerve();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }

    private void HandleSwerve()
    {
        if (InputManager.Instance == null) return;

        float swerveAmount = InputManager.Instance.GetSwerveAmount();
        Vector3 currentPos = transform.position;
        float newX = currentPos.x + (swerveAmount * swerveSpeed);
        newX = Mathf.Clamp(newX, minX, maxX);

        transform.position = new Vector3(newX, currentPos.y, currentPos.z);
    }

    // ★追加: ゴール判定（何かにぶつかった時に呼ばれる）
    private void OnTriggerEnter(Collider other)
    {
        // ぶつかった相手が "Finish" タグを持っていたら
        if (other.CompareTag("Finish"))
        {
            // GameManagerに「クリア！」と報告する
            GameManager.Instance.LevelComplete();
        }
    }
}