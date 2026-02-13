using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private RoadChunk roadPrefab;   // 道のプレハブ
    [SerializeField] private RoadChunk finishPrefab; // ゴールのプレハブ（後で作る）
    [SerializeField] private int levelLength = 10;   // 道を何個繋げるか

    private Vector3 _nextSpawnPoint;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        // 1. 通常の道を生成
        for (int i = 0; i < levelLength; i++)
        {
            SpawnChunk(roadPrefab);
        }

        // 2. 最後にゴールを生成
        if (finishPrefab != null)
        {
            SpawnChunk(finishPrefab);
        }
    }

    private void SpawnChunk(RoadChunk chunkPrefab)
    {
        // 次の生成地点に新しい道を置く
        RoadChunk newChunk = Instantiate(chunkPrefab, _nextSpawnPoint, Quaternion.identity);
        
        // 生成した道の子要素にならないように、Hierarchyを整理（任意）
        newChunk.transform.SetParent(transform);

        // 次の生成地点を更新（今置いた道の端っこ）
        _nextSpawnPoint = newChunk.GetEndPoint();
    }
}