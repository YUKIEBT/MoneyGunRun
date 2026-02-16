using UnityEngine;

public class RandomDecoration : MonoBehaviour
{
    [Header("左側の設定")]
    [SerializeField] private GameObject[] leftPrefabs;      // 左用のガチャ中身
    [SerializeField] private Transform[] leftSpawnPoints;   // 左の目印

    [Header("右側の設定")]
    [SerializeField] private GameObject[] rightPrefabs;     // 右用のガチャ中身
    [SerializeField] private Transform[] rightSpawnPoints;  // 右の目印

    private void Start()
    {
        // 左側のガチャを回す
        SpawnDecorations(leftPrefabs, leftSpawnPoints);
        
        // 右側のガチャを回す
        SpawnDecorations(rightPrefabs, rightSpawnPoints);
    }

    // ガチャを回して配置する共通ルール（同じコードを2回書かないための工夫です！）
    private void SpawnDecorations(GameObject[] prefabs, Transform[] points)
    {
        if (prefabs.Length == 0 || points.Length == 0) return;

        foreach (Transform point in points)
        {
            int randomIndex = Random.Range(0, prefabs.Length);
            GameObject selectedPrefab = prefabs[randomIndex];

            // 選ばれたプレハブがちゃんとセットされていれば配置する
            if (selectedPrefab != null)
            {
                Instantiate(selectedPrefab, point.position, point.rotation, point);
            }
        }
    }
}