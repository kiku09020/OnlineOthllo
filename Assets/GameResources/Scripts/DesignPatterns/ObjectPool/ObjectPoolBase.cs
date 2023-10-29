using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns.ObjectPool {
    public abstract class ObjectPoolBase<T> : MonoBehaviour where T : PooledObject<T> {
        [Header("ObjectPool")]
        [SerializeField, Tooltip("")]
        bool checkCollection = true;
        [SerializeField, Tooltip("アクティブなオブジェクト数もチェックするか")]
        bool checkActiveCount = true;
        [SerializeField, Tooltip("初期プール容量")] int defaultCapacity = 20;
        [SerializeField, Tooltip("最大プール容量")] int maxSize = 100;
        [SerializeField, Tooltip("最大アクティブ数")] int maxActiveCount = 100;

        //--------------------------------------------------
        protected virtual void Awake()
        {
            Init(checkCollection, defaultCapacity, maxSize);
        }

        //--------------------------------------------------
        /// <summary> プールの初期化処理 </summary>
        protected abstract void Init(bool isCheck, int defaultCapacity, int maxSize);

        /// <summary> オブジェクトを生成するときの処理 </summary>
        protected abstract T OnCreate(T prefab, Transform parent);

        /// <summary> オブジェクトをプールから取得するときの処理 </summary>
        protected virtual void OnGetFromPool(T obj)
        {
            obj.OnGetted();
        }

        /// <summary> オブジェクトをプールに返すときの処理 </summary>
        protected virtual void OnReleaseToPool(T obj)
        {
            obj.OnReleased();
        }

        /// <summary> プール内のオブジェクトを削除するときの処理 </summary>
        protected virtual void OnDestroyObject(T obj)
        {
            Destroy(obj.gameObject);
        }

        //--------------------------------------------------
        /// <summary> アクティブなオブジェクトの数をチェックする </summary>
        protected void CheckActiveCount(int count)
        {
            if (count >= maxActiveCount && checkActiveCount) {
                throw new System.Exception("最大数に達しました");
            }
        }
    }
}