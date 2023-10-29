using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatterns.ObjectPool {
    public class PooledObject<T> : MonoBehaviour where T : PooledObject<T> {
        public event System.Func<T> OnGettingEvent;
        public event System.Action OnReleasingEvent;

        //--------------------------------------------------
        /// <summary> 作成されるときの処理 </summary>
        public virtual void OnCreated()
        {
            OnGetted();
        }

        /// <summary> プールから取得されたときの処理 </summary>
        public virtual void OnGetted()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// プールに戻されるときの処理
        /// </summary>
        public virtual void OnReleased()
        {
            gameObject.SetActive(false);
        }

        /// <summary> 複製 </summary>
        public virtual T Duplicate()
        {
            var obj = OnGettingEvent?.Invoke();

            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.transform.localScale = transform.localScale;

            return obj;
        }

        /// <summary> 自身をプールに戻す </summary>
        public void Release()
        {
            OnReleasingEvent?.Invoke();
        }
    }
}