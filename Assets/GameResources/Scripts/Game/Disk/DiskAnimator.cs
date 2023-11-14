using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Disk {
	public class DiskAnimator : MonoBehaviour {
		/* Fields */
		[SerializeField] float flipAnimDuration = .5f;
		[SerializeField] Ease flipAnimeEase;
		[SerializeField] float flipUpAnimEndValue = 1;
		[SerializeField] Ease flipUpAnimEase;

		//-------------------------------------------------------------------
		/* Properties */

		//-------------------------------------------------------------------
		/* Messages */
		void Awake()
		{

		}

		void FixedUpdate()
		{

		}

		//-------------------------------------------------------------------
		/* Methods */
		public void PlayFlipAnim(Quaternion targetRotate, System.Action onComplete)
		{
			Sequence sequence = DOTween.Sequence();
			var rotateTween = transform.DORotate(targetRotate.eulerAngles, flipAnimDuration)
				.SetEase(flipAnimeEase);

			var upTween = transform.DOMoveY(flipUpAnimEndValue, flipAnimDuration / 2)
				.SetEase(flipUpAnimEase)
				.SetLoops(2, LoopType.Yoyo);

			sequence.Append(rotateTween)
				.Join(upTween)
				.OnComplete(() => onComplete?.Invoke());
		}
	}
}