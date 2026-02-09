using UnityEngine;

// 責務: ユーザーの入力（ドラッグ操作）を検知し、数値を返すだけ
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Settings")]
    [SerializeField] private float moveSensitivity = 1.0f; // 感度調整

    private float _lastFrameFingerPositionX;
    private float _moveFactorX;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // マウス左クリック or タッチ中
        if (Input.GetMouseButtonDown(0))
        {
            _lastFrameFingerPositionX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            // 前フレームとの差分を計算
            float delta = Input.mousePosition.x - _lastFrameFingerPositionX;
            _lastFrameFingerPositionX = Input.mousePosition.x;

            // 画面幅に対する割合で移動量を正規化 (-1.0 ~ 1.0 の範囲に収めやすくする)
            _moveFactorX = delta * moveSensitivity / Screen.width;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _moveFactorX = 0f;
        }
        else
        {
            _moveFactorX = 0f;
        }
    }

    // 外部（Player）はこのメソッドを呼んで移動量をもらう
    public float GetSwerveAmount()
    {
        return _moveFactorX;
    }
}