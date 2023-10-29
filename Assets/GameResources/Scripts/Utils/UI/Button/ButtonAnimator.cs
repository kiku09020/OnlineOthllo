using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils.UI.Button {
    public class ButtonAnimator : MonoBehaviour {
        /* Fields */
        [Header("Component")]
        [SerializeField] protected UnityEngine.UI.Button button;

        [Header("Animator")]
        [SerializeField] float clickedAnimDuration = .5f;
        [SerializeField] float clickedAnimEndValue = 1.25f;
        [SerializeField] Ease clickedAnimEase;

        Tween clickedAnim;

        //-------------------------------------------------------------------
        /* Properties */

        //-------------------------------------------------------------------
        /* Messages */

        //-------------------------------------------------------------------
        /* Methods */
        public void PlayClickedAnim()
        {
            clickedAnim?.Complete();
            clickedAnim = button.transform.DOScale(clickedAnimEndValue, clickedAnimDuration)
                .SetEase(clickedAnimEase)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}