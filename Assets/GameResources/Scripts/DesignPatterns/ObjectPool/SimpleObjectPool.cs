using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatterns.ObjectPool {
    public class SimpleObjectPool<T> : ObjectPoolBase<T> where T : PooledObject<T> {
        [SerializeField] T prefab;                      // 生成するオブジェクトのプレハブ
        [SerializeField] Transform generatedParent;     // 生成される親Transform		

        //-------------------------------------------------------------------
        /* Properties */
        public ObjectPool<T> Pool { get; protected set; }

        //-------------------------------------------------------------------
        /* Events */
        protected override void Init(bool isCheck, int defaultCapacity, int maxSize)
        {
            Pool = new ObjectPool<T>(() => OnCreate(prefab, generatedParent), OnGetFromPool, OnReleaseToPool, OnDestroyObject,
                                        isCheck, defaultCapacity, maxSize);
        }

        protected override T OnCreate(T prefab, Transform parent)
        {
            CheckActiveCount(Pool.CountActive);

            T obj = Instantiate(prefab, generatedParent);
            obj.OnCreated();

            return obj;
        }

        //-------------------------------------------------------------------
        /* Methods */

    }
}