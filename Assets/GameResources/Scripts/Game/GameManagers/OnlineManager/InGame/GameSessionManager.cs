using Cysharp.Threading.Tasks;
using Fusion;
using Game.Board;
using Game.Player;
using GameController.Game;
using GameUtils.SceneController;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameController.Online {
	/// <summary> �Q�[�����̃Z�b�V�����̊Ǘ��N���X </summary>
	public class GameSessionManager : MonoBehaviour {
		/* Fields */
		[SerializeField] BoardManager boardManager;
		[SerializeField] TurnManager turnManager;

		[Header("PlayerObject")]
		[SerializeField] NetworkPrefabRef playerObjectPrefab;
		[SerializeField] Transform playerParent;

		[Header("Debug")]
		[SerializeField] NetworkRunner runnerPrefab;

		bool isConnected;

		NetworkRunner runner;

		//-------------------------------------------------------------------
		/* Properties */

		//-------------------------------------------------------------------
		/* Messages */
		async void Awake()
		{
			runner = FindFirstObjectByType<NetworkRunner>();

			// �f�o�b�O���ɁA�ڑ��V�[�����o�R�����ɂ��̃V�[�������s�����ꍇ�A
			// ������NetworkRunner���m�ۂ���
			if (Debug.isDebugBuild && runner == null) {
				runner = await SessionConnecter.Connect(runnerPrefab);
			}

			// �ڑ������܂őҋ@
			await UniTask.WaitUntil(() => runner.ActivePlayers.ToList().Count >= 2);

			// �v���C���[�ݒ�
			foreach (var player in runner.ActivePlayers) {
				SpawnPlayerObject(runner, player);      // PlayerObject����(UI)
			}

			// �}�X�^�[�N���C�A���g���ڑ����ꂽ�琶��
			if (runner.IsSharedModeMasterClient && !isConnected) {
				boardManager.SetStartBoard(runner);
			}

			isConnected = true;
			print("connected!");
		}

		//--------------------------------------------------
		/* Callbacks */

		//-------------------------------------------------------------------
		/* Methods */
		// �v���C���[����
		void SpawnPlayerObject(NetworkRunner runner, PlayerRef player)
		{
			var playerUIObj = runner.Spawn(playerObjectPrefab);
			//runner.SetPlayerObject(player, playerUIObj);

			playerUIObj.transform.SetParent(playerParent);
			var playerRT = playerUIObj.GetComponent<RectTransform>();

			// �f�[�^�ݒ�
			var playerObj = playerUIObj.GetComponent<PlayerObject>();

			playerObj.NetworkData.IsFirstTurn = turnManager.SetTurnOrder(runner, player);   // �^�[�������߂�

			// ���g�̂Ƃ��A�����ɕ\��
			if (player == runner.LocalPlayer) {
				playerObj.NetworkData.NickName = "���Ԃ�";

				playerRT.anchorMin = Vector2.zero;
				playerRT.anchorMax = Vector2.zero;
				playerRT.pivot = Vector2.zero;
				playerRT.anchoredPosition = new Vector3(32, 32, 0);
			}

			// ����̂Ƃ��A�E��ɕ\��
			else {
				playerObj.NetworkData.NickName = "������";

				playerRT.anchorMin = Vector2.one;
				playerRT.anchorMax = Vector2.one;
				playerRT.pivot = Vector2.one;
				playerRT.anchoredPosition = new Vector3(-32, -32, 0);
			}

			playerObj.SetData(playerObj.NetworkData);
		}
	}
}