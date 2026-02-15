using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float swerveSpeed = 30f;
    [SerializeField] private float maxX = 4.5f;
    [SerializeField] private float minX = -4.5f;

    private void Update()
    {
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

    // --- ここから下が衝突判定 ---

    // ① ゴール（すり抜ける壁）に入った時
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GameManager.Instance.LevelComplete();
        }
        // ※念のため、敵がTriggerになっていた場合の保険
        else if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.GameOver();
        }
    }

    // ② 敵（硬いブロック）に激突した時（★今回追加！）
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // GameManagerに「ゲームオーバー！」と報告する
            GameManager.Instance.GameOver();
        }
    }
}