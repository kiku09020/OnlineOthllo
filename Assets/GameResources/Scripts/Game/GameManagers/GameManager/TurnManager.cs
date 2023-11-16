using Fusion;
using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameController.Game {
	public class TurnManager : NetworkBehaviour {
		/* Fields */
		[SerializeField, Tooltip("�^�[���������߂邩�ǂ���")] bool isSetTurnOrder = true;

		//-------------------------------------------------------------------
		/* Properties */
		/// <summary> �^�[���� </summary>
		public int TurnCount { get; private set; }

		//--------------------------------------------------
		/* Events */
		public event System.Action OnTurnStart;
		public event System.Action OnTurnEnd;

		//-------------------------------------------------------------------
		/* Messages */


		//-------------------------------------------------------------------
		/* Methods */
		/// <summary> �^�[���J�n���� </summary>
		public void TurnStart()
		{
			TurnCount++;

			OnTurnStart?.Invoke();
		}

		/// <summary> �^�[���I������ </summary>
		public void TurnEnd()
		{
			OnTurnEnd?.Invoke();
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