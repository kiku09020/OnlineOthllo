using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Player {
	public class PlayerInfoPresenter : MonoBehaviour {
		/* Fields */
		[Header("Components")]
		[SerializeField] PlayerObject player;

		[Header("UI")]
		[SerializeField] Image backImage;
		[SerializeField] TextMeshProUGUI playerNameText;
		[SerializeField] TextMeshProUGUI playerTurnOrderText;

		[Header("Color")]
		[SerializeField] Color firstColor = Color.black;
		[SerializeField] Color secoundColor= Color.white;

		//-------------------------------------------------------------------
		/* Properties */
		private void Awake()
		{
			player.OnSetDataSync += OnSetData;
		}
		//-------------------------------------------------------------------
		/* Messages */

		//-------------------------------------------------------------------
		/* Methods */
		public void OnSetData(PlayerNetworkData data)
		{
            playerNameText.text = player.PersonalName;

            playerTurnOrderText.text = (data.IsFirstTurn) ? "êÊçU" : "å„çU";

			// èáî‘Ç…âûÇ∂ÇƒîwåiêFïœçX
			backImage.color = (data.IsFirstTurn) ? firstColor : secoundColor;
		}
	}
}