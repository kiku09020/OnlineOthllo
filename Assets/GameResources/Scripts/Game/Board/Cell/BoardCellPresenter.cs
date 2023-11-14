using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Board {
	public class BoardCellPresenter : MonoBehaviour {
		/* Fields */
		[SerializeField] IBoardCellInputEvents inputProvider;
		[SerializeField] MeshRenderer rend;

		[Header("Color")]
		[SerializeField] Color clickedColor = Color.yellow;
		[SerializeField] Color defaultColor = Color.white;

		//-------------------------------------------------------------------
		/* Properties */

		//-------------------------------------------------------------------
		/* Messages */

		//-------------------------------------------------------------------
		/* Methods */
		private void Awake()
		{
			rend.enabled = false;
			rend.material.color = defaultColor;

			inputProvider = GetComponent<IBoardCellInputEvents>();

			inputProvider.SelectedEvent += SelectedEventHandler;
			inputProvider.DeselectedEvent += DeselectedEventHandler;
			inputProvider.ClickedDownEvent += ClickedDownEvnetHandler;
			inputProvider.ClickedUpEvent += ClickedUpEvnetHandler;
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