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
		public event System.Action OnCompleted;

		//-------------------------------------------------------------------
		/* Methods */
		public void SetDisk(DiskState state, bool aniamted, bool isLastDisk = false)
		{
			isBlack = (state == DiskState.black) ? true : false;

			Flip(isLastDisk, aniamted);
		}

		// ���Ԃ�����
		void Flip(bool isLastDisk = false, bool animated = false)
		{
			// �A�j���[�V������
			if (animated) {
				animator.PlayFlipAnim(GetTargetRotate(), () => {
					if (isLastDisk) {
						OnCompleted?.Invoke();
					}
				});
			}

			// �A�j���[�V�����Ȃ�
			else {
				transform.rotation = GetTargetRotate();
			}
		}

		// �ڕW��]�N�H�[�^�j�I���擾
		Quaternion GetTargetRotate()
		{
			Quaternion rotation = Quaternion.identity;

			if (isBlack) {
				rotation = new Quaternion(0, 0, 180, 0);
			}

			return rotation;
		}
	}
}