using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
    /// <summary> マスの入力クラス </summary>
    public class BoardCellInputProvider : NetworkBehaviour {

        /* Fields */
        bool isClicked;

        //-------------------------------------------------------------------
        /* Properties */
        [Networked]
        public bool IsSelectable { get; private set; } = true;

        //-------------------------------------------------------------------
        /* Events */
        public event System.Func<bool> OnInitEvent;
        public event System.Action<bool> SelectedEvent;
        public event System.Action DeselectedEvent;
        public event System.Action ClickedDownEvent;
        public event System.Action ClickedUpEvent;

        //--------------------------------------------------
        /* Messages */
		public override void Spawned()
		{
			if (OnInitEvent != null)
				IsSelectable = OnInitEvent.Invoke();

            
		}

		private void Update()
		{
            if (Input.GetMouseButton(0)) {
                isClicked = true;
            }

            else if (Input.GetMouseButtonUp(0)) {
                isClicked = false;
            }
		}

        //-------------------------------------------------------------------
        /* Methods */
        /// <summary> 選択状態を無効化 </summary>
        public void InvalidateSelectable()
        { IsSelectable = false; }

        public void OnSelected()
        {
            if (IsSelectable) {
                SelectedEvent?.Invoke(isClicked);
            }
        }

        public void OnDeselected()
        {
            if (IsSelectable) {
                DeselectedEvent?.Invoke();
            }
        }

        public void OnClickedDown()
        {
            if (IsSelectable) {
                ClickedDownEvent?.Invoke();
            }
        }

        public void OnClickedUp()
        {
            if (IsSelectable) {
                ClickedUpEvent?.Invoke();
            }
        }
    }
}