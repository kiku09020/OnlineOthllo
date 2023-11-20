using Cysharp.Threading.Tasks;
using Fusion;
using Game.Player;
using GameController.Online;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.Game {
	public class TurnManager : MonoBehaviour {
		/* Fields */
		[SerializeField, Tooltip("ターン順を決めるかどうか")] bool isSetTurnOrder = true;
		[SerializeField] PlayerManager playerManager;

		//-------------------------------------------------------------------
		/* Properties */
		/// <summary> ターン数 </summary>
		public int TurnCount { get; private set; }

		//--------------------------------------------------
		/* Events */
		public event System.Action<PlayerObject> OnTurnStart;
		public event System.Action<PlayerObject> OnTurnEnd;

		//-------------------------------------------------------------------
		/* Messages */
		private async void Awake()
		{
			playerManager.OnSetTurnOrder += SetTurnOrder;

			await UniTask.WaitUntil(() => OnTurnStart != null);

			playerManager.OnSetData +=(player)=>{
				player.OnTurnStart += OnTurnStart;
				player.OnTurnEnd += OnTurnEnd;
			};
		}

		//-------------------------------------------------------------------
		/* Methods */
		/// <summary> ターン開始処理 </summary>
		public void TurnStart(PlayerObject player)
		{
			TurnCount++;

			OnTurnStart?.Invoke(player);
		}

		/// <summary> ターン終了処理 </summary>
		public void TurnEnd(PlayerObject player)
		{
			OnTurnEnd?.Invoke(player);
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