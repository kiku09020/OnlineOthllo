using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Disk {
    public enum DiskState {
        empty,
        black,
        white,
    }

    /// <summary> 石 </summary>
    public class Disk : NetworkBehaviour {
        /* Fields */
        bool isBlack;

        [SerializeField] DiskAnimator animator;
        //-------------------------------------------------------------------
        /* Properties */

        //-------------------------------------------------------------------
        /* Events */
        public event System.Action OnCompletedSet;
        public event System.Action OnCompletedFlip;

        //-------------------------------------------------------------------
        /* Methods */
        public void SetDisk(DiskState state, bool aniamted = true)
        {
            isBlack = (state == DiskState.black) ? true : false;

            if (aniamted) {
                animator.PlaySetAnim(GetTargetRotate(false), () => {
                    OnCompletedSet?.Invoke();
                });
            }

            else {
                transform.rotation = GetTargetRotate(false);
                OnCompletedSet?.Invoke();
            }
        }

        // 裏返し処理
        void Flip(bool isLastDisk = false, bool animated = false)
        {
            // アニメーションつき
            if (animated) {
                animator.PlayFlipAnim(GetTargetRotate(true), () => {
                    if (isLastDisk) {
                        OnCompletedFlip?.Invoke();
                    }
                });
            }

            // アニメーションなし
            else {
                transform.rotation = GetTargetRotate(true);
            }
        }

        // 目標回転クォータニオン取得
        Quaternion GetTargetRotate(bool isFliped)
        {
            Quaternion rotation = Quaternion.identity;

            if (!isBlack) {
                rotation = new Quaternion(0, 0, 180, 0);
            }

            // 反転
            if (isFliped) {
                rotation = Quaternion.AngleAxis(180, Vector3.forward);
            }

            return rotation;
        }
    }
}