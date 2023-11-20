using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
	public class BoardCellPresenter : NetworkBehaviour {
		/* Fields */
		[SerializeField] MeshRenderer rend;
		[SerializeField] BoardCell boardCell;

		[Header("Color")]
		[SerializeField] Color defaultColor = Color.white;
		[SerializeField] Color selectedColor = Color.white;
		[SerializeField] Color clickedColor = Color.yellow;

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

			boardCell.OnChecked += (value) => rend.enabled = value;
			InputProvider.SelectedEvent += SelectedEventHandler;
			InputProvider.DeselectedEvent += DeselectedEventHandler;
			InputProvider.ClickedDownEvent += ClickedDownEvnetHandler;
			InputProvider.ClickedUpEvent += ClickedUpEvnetHandler;
		}

		void SelectedEventHandler(bool isClicked)
		{
			// クリック中に選択されている場合、選択色にする
			rend.material.color = (isClicked) ? clickedColor : selectedColor;
		}

		void DeselectedEventHandler()
		{
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