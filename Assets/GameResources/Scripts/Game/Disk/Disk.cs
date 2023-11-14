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
		public event System.Action OnCompleted;

		//-------------------------------------------------------------------
		/* Methods */
		public void SetDisk(DiskState state, bool aniamted, bool isLastDisk = false)
		{
			isBlack = (state == DiskState.black) ? true : false;

			Flip(isLastDisk, aniamted);
		}

		// 裏返し処理
		void Flip(bool isLastDisk = false, bool animated = false)
		{
			// アニメーションつき
			if (animated) {
				animator.PlayFlipAnim(GetTargetRotate(), () => {
					if (isLastDisk) {
						OnCompleted?.Invoke();
					}
				});
			}

			// アニメーションなし
			else {
				transform.rotation = GetTargetRotate();
			}
		}

		// 目標回転クォータニオン取得
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