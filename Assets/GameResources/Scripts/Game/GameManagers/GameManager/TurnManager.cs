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
		[SerializeField, Tooltip("�^�[���������߂邩�ǂ���")] bool isSetTurnOrder = true;
		[SerializeField] PlayerManager playerManager;

		//-------------------------------------------------------------------
		/* Properties */
		/// <summary> �^�[���� </summary>
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
		/// <summary> �^�[���J�n���� </summary>
		public void TurnStart(PlayerObject player)
		{
			TurnCount++;

			OnTurnStart?.Invoke(player);
		}

		/// <summary> �^�[���I������ </summary>
		public void TurnEnd(PlayerObject player)
		{
			OnTurnEnd?.Invoke(player);
		}

		//--------------------------------------------------
		// �^�[�������[���b�g
		void TurnRourellte()
		{

		}

		/// <summary> �^�[���������߂� </summary>
		public bool SetTurnOrder(NetworkRunner runner, PlayerRef player)
		{
			// �^�[�������߂�
			if (isSetTurnOrder) {
				TurnRourellte();

				return true;
			}

			// �^�[�������߂Ȃ�(debug)
			else {
				bool isLocalPlayer = (player == runner.LocalPlayer);

				return (runner.IsSharedModeMasterClient) ? isLocalPlayer : !isLocalPlayer;
			}
		}
	}
}