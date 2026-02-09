using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Header("Settings")]
    [SerializeField] private GameObject bulletPrefab; // 弾の元データ
    [SerializeField] private int poolSize = 50;       // 最初に用意する数

    private Queue<GameObject> _poolQueue = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        // 事前に50個作って、非表示にして待機させておく
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            obj.transform.SetParent(transform); // 散らからないようにこのオブジェクトの子にする
            _poolQueue.Enqueue(obj);
        }
    }

    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        if (_poolQueue.Count == 0)
        {
            // 足りなくなったら臨時で作る
            GameObject newObj = Instantiate(bulletPrefab);
            newObj.SetActive(false);
            _poolQueue.Enqueue(newObj);
        }

        GameObject obj = _poolQueue.Dequeue();

        // 使う前に位置と回転をセット
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        // 使い終わったら（非アクティブになったら）またキューに戻す仕組みが必要だが
        // 簡易的に「次に使うとき」にキューに戻す実装にする
        _poolQueue.Enqueue(obj);

        return obj;
    }
}