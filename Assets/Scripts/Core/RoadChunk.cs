using UnityEngine;
using System.Collections.Generic;

public class RoadChunk : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private Transform endPoint;
    [SerializeField] private float length = 20f;

    [Header("Spawning Settings")]
    [SerializeField] private GameObject enemyPrefab; // 敵のプレハブ
    [SerializeField] private GameObject gatePrefab;  // ゲートのプレハブ
    [SerializeField] private float spawnChance = 0.5f; // 何かが出る確率 (50%)

    // アイテムを配置するレーン（左、中央、右）
    private float[] lanes = { -2.5f, 0f, 2.5f };

    private void Start()
    {
        SpawnItems();
    }

    public Vector3 GetEndPoint()
    {
        return endPoint.position;
    }

    private void SpawnItems()
    {
        // プレハブがセットされていなければ何もしない（安全策）
        if (enemyPrefab == null || gatePrefab == null) return;

        // 3つのレーンそれぞれについて判定
        foreach (float x in lanes)
        {
            // サイコロを振る
            if (Random.value < spawnChance)
            {
                // さらに50%の確率で「敵」か「ゲート」か決める
                GameObject prefabToSpawn = (Random.value > 0.5f) ? enemyPrefab : gatePrefab;

                // 生成位置を決定 (この道のローカル座標系で)
                // Z軸はランダムにずらして、一列に並ばないようにする
                Vector3 spawnPos = new Vector3(x, 0.5f, Random.Range(2f, length - 2f));
                
                // 生成！
                GameObject obj = Instantiate(prefabToSpawn, transform);
                obj.transform.localPosition = spawnPos;

                // もしゲートなら、向きを修正（ゲートが横向いてたら困るので）
                if (prefabToSpawn == gatePrefab)
                {
                    obj.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (endPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(endPoint.position, 0.5f);
        }
    }
}