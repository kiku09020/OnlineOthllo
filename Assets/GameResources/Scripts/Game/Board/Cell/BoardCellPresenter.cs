using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
	public class BoardCellPresenter : NetworkBehaviour {
		/* Fields */
		[SerializeField] MeshRenderer rend;

		[Header("Color")]
		[SerializeField] Color clickedColor = Color.yellow;
		[SerializeField] Color defaultColor = Color.white;

		//-------------------------------------------------------------------
		/* Properties */
		public BoardCellInputProvider InputProvider { get; private set; }
		//-------------------------------------------------------------------
		/* Messages */

		//-------------------------------------------------------------------
		/* Methods */
		public override void Spawned()
		{
			rend.enabled = false;
			rend.material.color = defaultColor;

			InputProvider = GetComponent<BoardCellInputProvider>();

			InputProvider.SelectedEvent += SelectedEventHandler;
			InputProvider.DeselectedEvent += DeselectedEventHandler;
			InputProvider.ClickedDownEvent += ClickedDownEvnetHandler;
			InputProvider.ClickedUpEvent += ClickedUpEvnetHandler;
		}

		void SelectedEventHandler(bool isClicked)
		{
			rend.enabled = true;

			// クリック中に選択されている場合、選択色にする
			rend.material.color = (isClicked) ? clickedColor : defaultColor;
		}

		void DeselectedEventHandler()
		{
			rend.enabled = false;
			rend.material.color = defaultColor;
		}

		void ClickedDownEvnetHandler()
		{
			rend.material.color = clickedColor;
		}

		void ClickedUpEvnetHandler()
		{
			rend.enabled = false;
		}
	}
}