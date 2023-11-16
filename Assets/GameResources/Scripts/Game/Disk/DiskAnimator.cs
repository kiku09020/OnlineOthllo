using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Disk {
    public class DiskAnimator : MonoBehaviour {
        /* Fields */

        [Header("SetAnim")]
        [SerializeField] float setAnimStartPosY = 1;
        [SerializeField] float setAnimDuration = .5f;
        [SerializeField] Ease setAnimEase;

        [Header("FlipAnim")]
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
        public void PlaySetAnim(Quaternion targetRotate, System.Action onComplete)
        {
            var animEndPosY = transform.position.y;

            transform.rotation = targetRotate;
            transform.position = new Vector3(transform.position.x, setAnimStartPosY,transform.position.z);

            transform.DOMoveY(animEndPosY, setAnimDuration)
                .SetEase(setAnimEase);
        }

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