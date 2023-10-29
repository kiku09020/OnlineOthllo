using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatterns.ObjectPool {
    /// <summary> 複数のプレハブから、複数のオブジェクトプールを作成して管理する </summary>
    public class ObjectPoolList<T> : ObjectPoolBase<T> where T : PooledObject<T> {
        /* Fields */
        [SerializeField] protected List<T> prefabs;
        protected Transform parent;

        protected Dictionary<T, ObjectPool<T>> objectPools = new Dictionary<T, ObjectPool<T>>();
        //-------------------------------------------------------------------
        /* Properties */

        //-------------------------------------------------------------------
        /* Events */
        event System.Action OnCreateAction;

        protected override void Init(bool isCheck, int defaultCapacity, int maxSize)
        {
            // プレハブの数ごとにプールを作成
            foreach (var prefab in prefabs) {
                var pool = new ObjectPool<T>(() => OnCreate(prefab, parent), OnGetFromPool, OnReleaseToPool, OnDestroyObject,
                                            isCheck, defaultCapacity, maxSize);

                OnCreateAction += () => CheckActiveCount(pool.CountActive);
                objectPools.Add(prefab, pool);
            }
        }

        protected override T OnCreate(T prefab, Transform parent)
        {
            OnCreateAction?.Invoke();

            T obj = Instantiate(prefab, parent);
            obj.OnCreated();

            return obj;
        }

        //-------------------------------------------------------------------
        /* Methods */
        // アイテム取得時の例外処理
        T GetItemCommonFunc(System.Func<T> getItemFunc)
        {
            try {
                return getItemFunc?.Invoke();
            }

            catch (System.Exception e) {
                Debug.LogException(e);
                return null;
            }
        }

        /// <summary> 要素番号を指定してプールを取得する </summary>
        public T GetItem(int index)
        {
            return GetItemCommonFunc(
                () => objectPools[prefabs[index]]?.Get());
        }

        /// <summary> プレハブ名を指定してプールを取得 </summary>
        public T GetItem(string prefabName)
        {
            return GetItemCommonFunc(
                () => objectPools[prefabs.Find(prefab => prefab.name == prefabName)]?.Get());
        }

        /// <summary> ランダム要素を取得 </summary>
        public T GetRandomPool()
        {
            var randomIndex = Random.Range(0, prefabs.Count);
            return GetItem(randomIndex);
        }
    }
}