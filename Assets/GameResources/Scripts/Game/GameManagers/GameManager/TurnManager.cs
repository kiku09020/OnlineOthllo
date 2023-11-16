using Fusion;
using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.Game {
	public class TurnManager : NetworkBehaviour {
		/* Fields */
		[SerializeField, Tooltip("ターン順を決めるかどうか")] bool isSetTurnOrder = true;

		//-------------------------------------------------------------------
		/* Properties */
		/// <summary> ターン数 </summary>
		public int TurnCount { get; private set; }

		//--------------------------------------------------
		/* Events */
		public event System.Action OnTurnStart;
		public event System.Action OnTurnEnd;

		//-------------------------------------------------------------------
		/* Messages */


		//-------------------------------------------------------------------
		/* Methods */
		/// <summary> ターン開始処理 </summary>
		public void TurnStart()
		{
			TurnCount++;

			OnTurnStart?.Invoke();
		}

		/// <summary> ターン終了処理 </summary>
		public void TurnEnd()
		{
			OnTurnEnd?.Invoke();
		}

		//--------------------------------------------------
		// ターン順ルーレット
		void TurnRourellte()
		{

		}

		/// <summary> ターン順を決める </summary>
		public bool SetTurnOrder(NetworkRunner runner, PlayerRef player)
		{
			// ターン順決める
			if (isSetTurnOrder) {
				TurnRourellte();

				return true;
			}

			// ターン順決めない(debug)
			else {
				bool isLocalPlayer = (player == runner.LocalPlayer);

				return (runner.IsSharedModeMasterClient) ? isLocalPlayer : !isLocalPlayer;
			}
		}
	}
}