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

    /// <summary> �� </summary>
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

        // ���Ԃ�����
        void Flip(bool isLastDisk = false, bool animated = false)
        {
            // �A�j���[�V������
            if (animated) {
                animator.PlayFlipAnim(GetTargetRotate(true), () => {
                    if (isLastDisk) {
                        OnCompletedFlip?.Invoke();
                    }
                });
            }

            // �A�j���[�V�����Ȃ�
            else {
                transform.rotation = GetTargetRotate(true);
            }
        }

        // �ڕW��]�N�H�[�^�j�I���擾
        Quaternion GetTargetRotate(bool isFliped)
        {
            Quaternion rotation = Quaternion.identity;

            if (!isBlack) {
                rotation = new Quaternion(0, 0, 180, 0);
            }

            // ���]
            if (isFliped) {
                rotation = Quaternion.AngleAxis(180, Vector3.forward);
            }

            return rotation;
        }
    }
}